// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Incd and the OpenGamma group of companies
//
// Please see distribution for license.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
//     
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FudgeMessage;
using FudgeMessage.Serialization;
using Mercury.Language;

namespace FudgeMessage.Mapping
{
    /// <summary>
    /// Extensible dictionary of types that Fudge can convert to and from wire format.
    /// <p>
    /// This class contains a cache of mappings from Java types to Fudge messages.
    /// There is one instance of the dictionary per {@link FudgeContext context}.
    /// <p>
    /// Mappings may be added in three main ways.
    /// <p>
    /// The simplest way is to create an instance of {@link FudgeBuilder} and call the
    /// {@code addBuilder} methodd This will register an instance of the builder for a specific type.
    /// Subclasses of the type will not use the builder.
    /// <p>
    /// The second mechanism is classpath scanningd Simply annotate the builder class with
    /// {@link FudgeBuilderFor}, and call {@code addAllClasspathBuilders}.
    /// The method can be slow when operating on a large classpath.
    /// The system property {@code org.fudgemsg.autoscan} allows this to be done automatically.
    /// <p>
    /// The third method is generic buildersd This class contains a single instance of
    /// {@link FudgeBuilderFactory}, which is capable of creating builders on demand.
    /// See {@link FudgeDefaultBuilderFactory} for the default list of handled types.
    /// Further generic builders can be registered with the factory.
    /// These generic builders will handle subclasses of the registered type.
    /// <p>
    /// All builder caching is done in this class.
    /// The factory is not responsible for caching.
    /// <p>
    /// Registering a different factory, or registering additional/different generic builders can
    /// change the default behavior for unrecognized typesd As such, it is recommended to only
    /// initialize the dictionary at system startupd However, the cache is concurrent, so will
    /// handle later additions.
    /// 
    /// @author Andrew Griffin
    /// </summary>
    public class FudgeObjectDictionary
    {
        /// <summary>
        /// The Name of the property to be set (to any value) to automatically scan the classpath
        /// for builders on startup.
        /// </summary>
        public static String AUTO_CLASSPATH_SCAN_PROPERTY = "FudgeMessage.AutoScan";

        private static IFudgeMessageBuilder<Object> NULL_MESSAGEBUILDER = new FudgeMessageBuilder();

        private static IFudgeObjectBuilder<Object> NULL_OBJECTBUILDER = new FudgeObjectBuilder();

        private ConcurrentDictionary<Type, IFudgeObjectBuilder> _objectBuilders;
        private ConcurrentDictionary<Type, IFudgeMessageBuilder> _messageBuilders;
        private AtomicBoolean _haveScannedClasspath = new AtomicBoolean(false);

        private IFudgeBuilderFactory _defaultBuilderFactory;

        /// <summary>
        /// Constructs a new (initially empty) {@link FudgeObjectDictionary}.
        /// </summary>
        public FudgeObjectDictionary()
        {
            _objectBuilders = new ConcurrentDictionary<Type, IFudgeObjectBuilder>();
            _messageBuilders = new ConcurrentDictionary<Type, IFudgeMessageBuilder>();
            _defaultBuilderFactory = new FudgeDefaultBuilderFactory();

            if (Core.NamespaceExists(AUTO_CLASSPATH_SCAN_PROPERTY))
            {
                AddAllAnnotatedBuilders();
            }
        }

        /// <summary>
        /// Constructs a new {@link FudgeObjectDictionary} as a clone of another.
        /// 
        /// </summary>
        /// <param Name="other">the {@code FudgeObjectDictionary} to clone</param>
        public FudgeObjectDictionary(FudgeObjectDictionary other)
        {
            _objectBuilders = new ConcurrentDictionary<Type, IFudgeObjectBuilder>(other._objectBuilders);
            _messageBuilders = new ConcurrentDictionary<Type, IFudgeMessageBuilder>(other._messageBuilders);
            _defaultBuilderFactory = new ImmutableFudgeBuilderFactory(other._defaultBuilderFactory);
        }

        /// <summary>
        /// Gets/sets the builder factory to use for types that are not explicitly registered hered It is recommended that {@link FudgeBuilderFactory}
        /// implementations are made using the {@link FudgeBuilderFactoryAdapter}, constructed with the previously set factory so that the behaviours
        /// can be chained.
        /// </summary>
        /// <returns>the current {@link FudgeBuilderFactory}.</returns>
        public IFudgeBuilderFactory DefaultBuilderFactory
        {
            get { return _defaultBuilderFactory; }
            set { _defaultBuilderFactory = value; }
        }

        /// <summary>
        /// Registers a new {@link FudgeObjectBuilder} with this dictionary to be used for a given classd The same builder can be registered against
        /// multiple classesd A class can only have one registered {@code FudgeObjectBuilder} - registering a second will overwrite the previous
        /// registration.
        /// 
        /// </summary>
        /// <typeparam Name="T">Java type of the objects created by the builder</typeparam>
        /// <param Name="clazz">the Java class to register the builder against</param>
        /// <param Name="builder">the builder to register</param>
        public void AddObjectBuilder<T>(IFudgeObjectBuilder<T> builder)
        {
            _objectBuilders.AddOrUpdate(typeof(T), (IFudgeObjectBuilder)builder);
        }

        /// <summary>
        /// Registers a new {@link FudgeMessageBuilder} with this dictionary to be used for a given classd The same builder can be registered against
        /// multiple classesd A class can only have one registered {@code FudgeMessageBuilder} - registering a second will overwrite the previous
        /// registration.
        /// 
        /// </summary>
        /// <typeparam Name="T">Java type of the objects processed by the builder</typeparam>
        /// <param Name="clazz">the Java class to register the builder against</param>
        /// <param Name="builder">builder to register</param>
        public void AddMessageBuilder<T>(IFudgeMessageBuilder<T> builder)
        {
            _messageBuilders.AddOrUpdate(typeof(T), builder);
        }

        /// <summary>
        /// Registers a new {@link FudgeBuilder} with this dictionary to be used for a given classd A {@code FudgeBuilder} is simply a combined {@link FudgeMessageBuilder}
        /// and {@link FudgeObjectBuilder} so this method is the same as calling {@link #addMessageBuilder(Class,FudgeMessageBuilder)} and {@link #addObjectBuilder(Class,FudgeObjectBuilder)}.
        /// 
        /// </summary>
        /// <typeparam Name="T">Java type of the objects processed by the builder</typeparam>
        /// <param Name="clazz">the Java class to register the builder against</param>
        /// <param Name="builder">builder to register</param>
        public void AddBuilder<T>(IFudgeBuilder<T> builder)
        {
            AddMessageBuilder(builder);
            AddObjectBuilder(builder);
        }

        /// <summary>
        /// Returns a {@link FudgeObjectBuilder} for the given class to convert a Fudge message to a Java objectd If none is already registered for the class,
        /// it will attempt to create one using the registered {@link FudgeBuilderFactory}d If it is not possible to create a builder (e.gd for an interface) returns {@code null}.
        /// 
        /// </summary>
        /// <typeparam Name="T">Java type of the objects to be built</typeparam>
        /// <param Name="clazz">the Java class to look up</param>
        /// <returns>the builder, or {@code null} if none is available</returns>
        // @SuppressWarnings("unchecked")
        public IFudgeObjectBuilder<T> GetObjectBuilder<T>()
        {
            IFudgeObjectBuilder<T> builder = (IFudgeObjectBuilder<T>)_objectBuilders[typeof(T)];
            if (builder == null)
            {
                IFudgeObjectBuilder<T> freshBuilder = DefaultBuilderFactory.CreateObjectBuilder<T>(typeof(T));
                if (freshBuilder == null) freshBuilder = (IFudgeObjectBuilder<T>)NULL_OBJECTBUILDER;
                builder = (IFudgeObjectBuilder<T>)_objectBuilders.PutIfAbsent(typeof(T), freshBuilder);
                if (builder == null)
                {
                    // we used to store a reference at this point if it also implemented FudgeMessageBuilder , but there might be better message builders available than this one
                    builder = freshBuilder;
                }
            }
            return (builder == NULL_OBJECTBUILDER) ? null : builder;
        }

        /// <summary>
        /// Returns a {@link FudgeMessageBuilder} for the given class to convert a Fudge message to a Java objectd If none is already registered for the class,
        /// it will attempt to create one using the registered {@link FudgeBuilderFactory}d If it is not possible to create a builder returns {@code null}.
        /// 
        /// </summary>
        /// <typeparam Name="T">Java type of the objects to be built</typeparam>
        /// <param Name="clazz">the Java class to look up</param>
        /// <returns>the builder, or {@code null} if none is available</returns>
        // @SuppressWarnings("unchecked")
        public IFudgeMessageBuilder<T> GetMessageBuilder<T>()
        {
            IFudgeMessageBuilder<T> builder = (IFudgeMessageBuilder<T>)_messageBuilders[typeof(T)];
            if (builder == null)
            {
                IFudgeMessageBuilder<T> freshBuilder = DefaultBuilderFactory.CreateMessageBuilder<T>(typeof(T));
                if (freshBuilder == null) freshBuilder = (IFudgeMessageBuilder<T>)NULL_MESSAGEBUILDER;
                builder = (IFudgeMessageBuilder<T>)_messageBuilders.PutIfAbsent(typeof(T), freshBuilder);
                if (builder == null)
                {
                    // we used to store a reference at this point if it also implemented FudgeObjectBuilder, but there might be better object builders available than this one
                    builder = freshBuilder;
                }
            }
            return (builder == NULL_MESSAGEBUILDER) ? null : builder;
        }

        /// <summary>
        /// Tests if the specification requires a default serialization, for example lists, maps, sets and arraysd Class headers
        /// are never needed and must be suppressed for default objectsd The objects are just written with ordinal field values
        /// greater than {@code 0}.
        /// 
        /// </summary>
        /// <param Name="clazz">the class to test, not {@code null}</param>
        /// <returns>{@code true} if the object has a default serialization scheme.</returns>
        public Boolean IsDefaultObject<T, V>()
        {
            // TODO move this logic to the builder factory so that it can be overridden
            return typeof(List<T>).IsAssignableFrom(typeof(T)) || typeof(HashSet<T>).IsAssignableFrom(typeof(T)) || typeof(Dictionary<T, V>).IsAssignableFrom(typeof(T)) || typeof(T).IsArray;
        }

        /// <summary>
        /// Returns the class indicated by a default serialization scheme.
        /// 
        /// </summary>
        /// <param Name="maxOrdinal">the highest ordinal used, or {@code 0} if no field ordinals were present.</param>
        /// <returns>the class to deserialize to, or {@code null} if the ordinal is not recognised</returns>
        public Type GetDefaultObjectClass<T, V>(int maxOrdinal)
        {
            // TODO move this logic to the builder factory so that it can be overridden
            switch (maxOrdinal)
            {
                case 0:
                    return typeof(List<T>);
                case 1:
                    return typeof(HashSet<T>);
                case 2:
                    return typeof(Dictionary<T, V>);
            }
            return typeof(T);
        }

        /// <summary>
        /// Scans all files available to common classpath loading system heuristics to determine
        /// which ones have the {@link FudgeBuilderFor} annotation, and registers those as appropriate
        /// builders.
        /// This is potentially a <em>very</em> expensive operation, and as such is optional.
        /// </summary>
        public void AddAllAnnotatedBuilders()
        {
            if (_haveScannedClasspath.GetAndSet(true))
            {
                return;
            }

            HashSet<String> classNamesWithAnnotation = ClassUtility.GetClassAttributeValues(typeof(IFudgeBuilderFor<>));
            if (classNamesWithAnnotation == null)
            {
                return;
            }
            foreach (String className in classNamesWithAnnotation)
            {
                AddAnnotatedBuilderClass(className);
            }
            classNamesWithAnnotation = ClassUtility.GetClassAttributeValues(typeof(IGenericFudgeBuilderFor));
            if (classNamesWithAnnotation == null)
            {
                return;
            }
            foreach (String className in classNamesWithAnnotation)
            {
                AddAnnotatedGenericBuilderClass(className);
            }
        }

        /// <summary>
        /// Add a class which is known to have a {@link FudgeBuilderFor} annotation as an object or message
        /// builder (or both).
        /// 
        /// </summary>
        /// <param Name="className">The fully qualified Name of the builder class.</param>
        // @SuppressWarnings("unchecked")
        public void AddAnnotatedBuilderClass(String className)
        {
            dynamic builderClass = InstantiateBuilderClass(className);
            Type t = builderClass.GetType();

            if ((builderClass == null)
                || !builderClass.IsImplementType(typeof(IFudgeBuilderFor<>)))
            {
                return;
            }

            Object builderInstance = null;
            try
            {
                builderInstance = builderClass.NewInstance();
            }
            catch (Exception e)
            {
                // Do nothing other than stack trace.
                e.PrintStackTrace();
                return;
            }

            if (builderInstance is FudgeMessageBuilder)
            {
                var method = this.GetType().GetMethod("AddMessageBuilder").MakeGenericMethod(t);

                method.Invoke(this, new object[] { builderInstance });
            }

            if (builderInstance is FudgeObjectBuilder)
            {
                var method = this.GetType().GetMethod("AddObjectBuilder").MakeGenericMethod(t);

                method.Invoke(this, new object[] { builderInstance });
            }
        }

        /// <summary>
        /// Add a class which is known to have a {@link GenericFudgeBuilderFor} annotation as builder.
        /// 
        /// </summary>
        /// <param Name="className">The fully qualified Name of the builder class.</param>
        // @SuppressWarnings("unchecked")
        public void AddAnnotatedGenericBuilderClass(String className)
        {

            var builderClass = InstantiateBuilderClass(className);

            if ((builderClass == null)
                || !builderClass.IsImplementType(typeof(IGenericFudgeBuilderFor)))
            {
                return;
            }

            Type t = builderClass.GetGenericArguments()[0];

            Object builderInstance = null;
            try
            {
                builderInstance = builderClass.NewInstance();
            }
            catch (Exception e)
            {
                // Do nothing other than stack trace.
                e.PrintStackTrace();
                return;
            }
            Type forClass = builderClass.GetGenericArguments()[0];

            if (!Core.IsSameClassOrImplementedInterface(builderInstance.GetType(), typeof(IFudgeBuilder<>), new Type[] { forClass }))
            {
                throw new ArgumentException("Annotated a generic builder " + builderClass + " but not a full FudgeBuilder<> implementation.");
            }

            Type constructedType = typeof(IFudgeBuilder<>);
            var method = constructedType.GetMethod("AddGenericBuilder").MakeGenericMethod(t);

            method.Invoke(DefaultBuilderFactory, new object[] { forClass, builderInstance });
        }

        /// <summary>
        /// </summary>
        /// <param Name="className"></param>
        /// <returns></returns>
        private Type InstantiateBuilderClass(String className)
        {
            Type builderClass = null;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                try
                {
                    var t = assembly.GetType(className);
                    if (t != null)
                    {
                        builderClass = t;
                        break;
                    }
                }
                catch (Exception e)
                {
                    // Silently swallowd Can't actually populate it.
                    // This should be rare, and you can just stop at this breakpoint
                    // (which is why the stack trace is here at all).
                    e.PrintStackTrace();
                }
            }
            return builderClass;
        }

        public class FudgeMessageBuilder : IFudgeMessageBuilder<Object>
        {

            public IMutableFudgeFieldContainer BuildMessage(FudgeSerializationContext context, Object value)
            {
                return null;
            }
        }

        public class FudgeObjectBuilder : IFudgeObjectBuilder<Object>
        {

            public Object BuildObject(FudgeDeserializationContext context, IFudgeFieldContainer message)
            {
                return null;
            }
        }
    }
}
