using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Fudge.Serialization;
using System.Reflection;
using System.IO;
using System.Linq;
using Fudge.Util;

/// <summary>
/// Copyright (C) 2009 - present by OpenGamma Inc. and other contributors.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///     http://www.apache.org/licenses/LICENSE-2.0
///     
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>

namespace Fudge.Mapping
{



	/// <summary>
	/// Extensible dictionary of types that Fudge can convert to and from wire format.
	/// <para>
	/// This class contains a cache of mappings from Java types to Fudge messages.
	/// There is one instance of the dictionary per <seealso cref="FudgeContext context"/>.
	/// </para>
	/// <para>
	/// Mappings may be added in three main ways.
	/// </para>
	/// <para>
	/// The simplest way is to create an instance of <seealso cref="FudgeBuilder"/> and call the
	/// {@code addBuilder} method. This will register an instance of the builder for a specific type.
	/// Subclasses of the type will not use the builder.
	/// </para>
	/// <para>
	/// The second mechanism is classpath scanning. Simply annotate the builder class with
	/// <seealso cref="FudgeBuilderFor"/>, and call {@code addAllClasspathBuilders}.
	/// The method can be slow when operating on a large classpath.
	/// The system property {@code org.fudgemsg.autoscan} allows this to be done automatically.
	/// </para>
	/// <para>
	/// The third method is generic builders. This class contains a single instance of
	/// <seealso cref="FudgeBuilderFactory"/>, which is capable of creating builders on demand.
	/// See <seealso cref="FudgeDefaultBuilderFactory"/> for the default list of handled types.
	/// Further generic builders can be registered with the factory.
	/// These generic builders will handle subclasses of the registered type.
	/// </para>
	/// <para>
	/// All builder caching is done in this class.
	/// The factory is not responsible for caching.
	/// </para>
	/// <para>
	/// Registering a different factory, or registering additional/different generic builders can
	/// change the default behavior for unrecognized types. As such, it is recommended to only
	/// initialize the dictionary at system startup. However, the cache is concurrent, so will
	/// handle later additions.
	/// 
	/// @author Andrew Griffin
	/// </para>
	/// </summary>
    public class FudgeObjectDictionary
    {
        /// <summary>
        /// The name of the property to be set (to any value) to automatically scan the classpath
        /// for builders on startup.
        /// </summary>
        public const string AUTO_CLASSPATH_SCAN_PROPERTY = "org.fudgemsg.autoscan";

        //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
        //ORIGINAL LINE: private static final FudgeMessageBuilder NULL_MESSAGEBUILDER = new FudgeMessageBuilder<Object> ()
        private static readonly IFudgeMessageBuilder<Object> NULL_MESSAGEBUILDER = new FudgeMessageBuilderAnonymousInnerClassHelper();

        private class FudgeMessageBuilderAnonymousInnerClassHelper : IFudgeMessageBuilder<Object>
        {
            public FudgeMessageBuilderAnonymousInnerClassHelper()
            {
            }

            public override IMutableFudgeFieldContainer BuildMessage(FudgeSerializer context, object @object)
            {
                return null;
            }
        }

        //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
        //ORIGINAL LINE: private static final FudgeObjectBuilder NULL_OBJECTBUILDER = new FudgeObjectBuilder<Object> ()
        private static readonly IFudgeObjectBuilder<Object> NULL_OBJECTBUILDER = new FudgeObjectBuilderAnonymousInnerClassHelper();

        private class FudgeObjectBuilderAnonymousInnerClassHelper : IFudgeObjectBuilder<Object>
        {
            public FudgeObjectBuilderAnonymousInnerClassHelper()
            {
            }

            public override object BuildObject(FudgeDeserializer context, IFudgeFieldContainer message)
            {
                return null;
            }
        }

        //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
        //ORIGINAL LINE: private final java.util.concurrent.ConcurrentMap<Class, FudgeObjectBuilder> _objectBuilders;
        private readonly ConcurrentDictionary<Type, IFudgeObjectBuilder> _objectBuilders;
        //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
        //ORIGINAL LINE: private final java.util.concurrent.ConcurrentMap<Class, FudgeMessageBuilder> _messageBuilders;
        private readonly ConcurrentDictionary<Type, IFudgeMessageBuilder> _messageBuilders;
        private readonly AtomicBoolean _haveScannedClasspath = AtomicBoolean.FromValue(false);

        private IFudgeBuilderFactory _defaultBuilderFactory;

        /// <summary>
        /// Constructs a new (initially empty) <seealso cref="FudgeObjectDictionary"/>.
        /// </summary>
        public FudgeObjectDictionary()
        {
            //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
            //ORIGINAL LINE: _objectBuilders = new java.util.concurrent.ConcurrentHashMap<Class, FudgeObjectBuilder> ();
            _objectBuilders = new ConcurrentDictionary<Type, IFudgeObjectBuilder>();
            //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
            //ORIGINAL LINE: _messageBuilders = new java.util.concurrent.ConcurrentHashMap<Class, FudgeMessageBuilder> ();
            _messageBuilders = new ConcurrentDictionary<Type, IFudgeMessageBuilder>();
            _defaultBuilderFactory = new FudgeDefaultBuilderFactory();

            if (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) != null)
            {
                addAllAnnotatedBuilders();
            }
        }

        /// <summary>
        /// Constructs a new <seealso cref="FudgeObjectDictionary"/> as a clone of another.
        /// </summary>
        /// <param name="other"> the {@code FudgeObjectDictionary} to clone </param>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: public FudgeObjectDictionary(final FudgeObjectDictionary other)
        public FudgeObjectDictionary(FudgeObjectDictionary other)
        {
            //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
            //ORIGINAL LINE: _objectBuilders = new java.util.concurrent.ConcurrentHashMap<Class, FudgeObjectBuilder> (other._objectBuilders);
            _objectBuilders = new ConcurrentDictionary<Type, IFudgeObjectBuilder>(other._objectBuilders);
            //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
            //ORIGINAL LINE: _messageBuilders = new java.util.concurrent.ConcurrentHashMap<Class, FudgeMessageBuilder> (other._messageBuilders);
            _messageBuilders = new ConcurrentDictionary<Type, IFudgeMessageBuilder>(other._messageBuilders);
            _defaultBuilderFactory = new ImmutableFudgeBuilderFactory(other._defaultBuilderFactory);
        }

        /// <summary>
        /// Returns the current builder factory for unregistered types.
        /// </summary>
        /// <returns> the current <seealso cref="FudgeBuilderFactory"/>. </returns>
        public virtual IFudgeBuilderFactory DefaultBuilderFactory
        {
            get
            {
                return _defaultBuilderFactory;
            }
            set
            {
                _defaultBuilderFactory = value;
            }
        }


        /// <summary>
        /// Registers a new <seealso cref="FudgeObjectBuilder"/> with this dictionary to be used for a given class. The same builder can be registered against
        /// multiple classes. A class can only have one registered {@code FudgeObjectBuilder} - registering a second will overwrite the previous
        /// registration.
        /// </summary>
        /// @param <T> Java type of the objects created by the builder </param>
        /// <param name="clazz"> the Java class to register the builder against </param>
        /// <param name="builder"> the builder to register </param>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: public <T> void addObjectBuilder(final Class clazz, final FudgeObjectBuilder<? extends T> builder)
        public virtual void AddObjectBuilder<T, T1>(Type clazz, IFudgeObjectBuilder<T1> builder) where T1 : T
        {
            _objectBuilders.TryAdd(clazz, builder);
        }

        public virtual void AddObjectBuilder(Type clazz, IFudgeObjectBuilder builder)
        {
            _objectBuilders.TryAdd(clazz, builder);
        }

        /// <summary>
        /// Registers a new <seealso cref="FudgeMessageBuilder"/> with this dictionary to be used for a given class. The same builder can be registered against
        /// multiple classes. A class can only have one registered {@code FudgeMessageBuilder} - registering a second will overwrite the previous
        /// registration.
        /// </summary>
        /// @param <T> Java type of the objects processed by the builder </param>
        /// <param name="clazz"> the Java class to register the builder against </param>
        /// <param name="builder"> builder to register </param>
        //JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
        //ORIGINAL LINE: public <T> void addMessageBuilder(final Class clazz, final FudgeMessageBuilder<? base T> builder)
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        public virtual void AddMessageBuilder(Type clazz, IFudgeMessageBuilder builder)
        {
            _messageBuilders.TryAdd(clazz, builder);
        }

        public virtual void AddMessageBuilder<T, T1>(Type clazz, IFudgeMessageBuilder<T1> builder)
        {
            _messageBuilders.TryAdd(clazz, builder);
        }

        /// <summary>
        /// Registers a new <seealso cref="FudgeBuilder"/> with this dictionary to be used for a given class. A {@code FudgeBuilder} is simply a combined <seealso cref="FudgeMessageBuilder"/>
        /// and <seealso cref="FudgeObjectBuilder"/> so this method is the same as calling <seealso cref="#addMessageBuilder(Class,FudgeMessageBuilder)"/> and <seealso cref="#addObjectBuilder(Class,FudgeObjectBuilder)"/>.
        /// </summary>
        /// @param <T> Java type of the objects processed by the builder </param>
        /// <param name="clazz"> the Java class to register the builder against </param>
        /// <param name="builder"> builder to register </param>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: public <T> void addBuilder(final Class clazz, final FudgeBuilder<T> builder)
        public virtual void AddBuilder<T>(Type clazz, IFudgeBuilder<T> builder)
        {
            AddMessageBuilder<T, T>(clazz, (IFudgeMessageBuilder<T>)builder);
            AddObjectBuilder<T, T>(clazz, (IFudgeObjectBuilder<T>)builder);
        }

        /// <summary>
        /// Returns a <seealso cref="FudgeObjectBuilder"/> for the given class to convert a Fudge message to a Java object. If none is already registered for the class,
        /// it will attempt to create one using the registered <seealso cref="FudgeBuilderFactory"/>. If it is not possible to create a builder (e.g. for an interface) returns {@code null}.
        /// </summary>
        /// @param <T> Java type of the objects to be built </param>
        /// <param name="clazz"> the Java class to look up </param>
        /// <returns> the builder, or {@code null} if none is available </returns>
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") public <T> FudgeObjectBuilder<T> getObjectBuilder(final Class clazz)
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        public virtual IFudgeObjectBuilder<T> GetObjectBuilder<T>(Type clazz)
        {
            IFudgeObjectBuilder buf;

            _objectBuilders.TryGetValue(clazz, out buf);
            IFudgeObjectBuilder<T> builder = (IFudgeObjectBuilder<T>)buf;

            if (builder == null)
            {
                IFudgeObjectBuilder<T> freshBuilder = IFudgeBuilderFactory.CreateObjectBuilder<T>(clazz);
                if (freshBuilder == null)
                {
                    freshBuilder = (IFudgeObjectBuilder<T>)NULL_OBJECTBUILDER;
                }
                builder = (IFudgeObjectBuilder<T>)_objectBuilders.AddOrUpdate(clazz, freshBuilder, (key, oldValue) => freshBuilder);
                if (builder == null)
                {
                    // we used to store a reference at this point if it also implemented FudgeMessageBuilder , but there might be better message builders available than this one
                    builder = freshBuilder;
                }
            }
            return (builder == NULL_OBJECTBUILDER) ? null : builder;
        }

        /// <summary>
        /// Returns a <seealso cref="FudgeMessageBuilder"/> for the given class to convert a Fudge message to a Java object. If none is already registered for the class,
        /// it will attempt to create one using the registered <seealso cref="FudgeBuilderFactory"/>. If it is not possible to create a builder returns {@code null}.
        /// </summary>
        /// @param <T> Java type of the objects to be built </param>
        /// <param name="clazz"> the Java class to look up </param>
        /// <returns> the builder, or {@code null} if none is available </returns>
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") public <T> IFudgeMessageBuilder getMessageBuilder(final Class clazz)
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        public virtual IFudgeMessageBuilder getMessageBuilder<T>(Type clazz)
        {
            IFudgeMessageBuilder builder;
            _messageBuilders.TryGetValue(clazz, out builder);

            if (builder == null)
            {
                IFudgeMessageBuilder freshBuilder = IFudgeBuilderFactory.CreateMessageBuilder<T>(clazz);
                if (freshBuilder == null)
                {
                    freshBuilder = (IFudgeMessageBuilder)NULL_MESSAGEBUILDER;
                }
                builder = (IFudgeMessageBuilder)_messageBuilders.AddOrUpdate(clazz, freshBuilder, (key, oldValue) => freshBuilder);
                if (builder == null)
                {
                    // we used to store a reference at this point if it also implemented FudgeObjectBuilder, but there might be better object builders available than this one
                    builder = freshBuilder;
                }
            }
            return (builder == NULL_MESSAGEBUILDER) ? null : builder;
        }

        /// <summary>
        /// Tests if the specification requires a default serialization, for example lists, maps, sets and arrays. Class headers
        /// are never needed and must be suppressed for default objects. The objects are just written with ordinal field values
        /// greater than {@code 0}.
        /// </summary>
        /// <param name="clazz"> the class to test, not {@code null} </param>
        /// <returns> {@code true} if the object has a default serialization scheme. </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: public boolean isDefaultObject(final Class clazz)
        public virtual bool isDefaultObject(Type clazz)
        {
            // TODO move this logic to the builder factory so that it can be overridden
            return clazz.IsSubclassOf(typeof(IList)) || clazz.IsSubclassOf(typeof(IEnumerable)) || clazz.IsSubclassOf(typeof(IDictionary)) || clazz.IsArray;
        }

        /// <summary>
        /// Returns the class indicated by a default serialization scheme.
        /// </summary>
        /// <param name="maxOrdinal"> the highest ordinal used, or {@code 0} if no field ordinals were present. </param>
        /// <returns> the class to deserialize to, or {@code null} if the ordinal is not recognised </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: public Class getDefaultObjectClass(final int maxOrdinal)
        public virtual Type getDefaultObjectClass(int maxOrdinal)
        {
            // TODO move this logic to the builder factory so that it can be overridden
            switch (maxOrdinal)
            {
                case 0:
                    return typeof(IList);
                case 1:
                    return typeof(IEnumerable);
                case 2:
                    return typeof(IDictionary);
            }
            return null;
        }

        /// <summary>
        /// Scans all files available to common classpath loading system heuristics to determine
        /// which ones have the <seealso cref="FudgeBuilderFor"/> annotation, and registers those as appropriate
        /// builders.
        /// This is potentially a <em>very</em> expensive operation, and as such is optional.
        /// </summary>
        public virtual void addAllAnnotatedBuilders()
        {
            if (_haveScannedClasspath.CompareAndExchange(true, true)) //.getAndSet(true)
            {
                return;
            }

            IEnumerable<string> classNamesWithAnnotation = ClasspathUtilities.getClassNamesWithAnnotation<FudgeBuilderFor>();
            if (classNamesWithAnnotation == null)
            {
                return;
            }
            foreach (string className in classNamesWithAnnotation)
            {
                addAnnotatedBuilderClass(className);
            }
            classNamesWithAnnotation = ClasspathUtilities.getClassNamesWithAnnotation<GenericFudgeBuilderFor>();
            if (classNamesWithAnnotation == null)
            {
                return;
            }
            foreach (string className in classNamesWithAnnotation)
            {
                addAnnotatedGenericBuilderClass(className);
            }
        }

        /// <summary>
        /// Add a class which is known to have a <seealso cref="FudgeBuilderFor"/> annotation as an object or message
        /// builder (or both). 
        /// </summary>
        /// <param name="className"> The fully qualified name of the builder class. </param>
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") public void addAnnotatedBuilderClass(String className)
        public virtual void addAnnotatedBuilderClass(string className)
        {
            Type builderClass = instantiateBuilderClass(className);

            if ((builderClass == null) || !(builderClass.GetCustomAttributes(typeof(FudgeBuilderFor), false).Length > 0))
            {
                return;
            }

            object builderInstance = null;
            try
            {
                builderInstance = Activator.CreateInstance(builderClass);
            }
            catch (Exception e)
            {
                // Do nothing other than stack trace.
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                return;
            }
            Type forClass = builderClass.GetCustomAttributes(typeof(FudgeBuilderFor), false).FirstOrDefault() as Type;

            if (builderInstance is IFudgeMessageBuilder)
            {
                AddMessageBuilder(forClass, (IFudgeMessageBuilder)builderInstance);
            }

            if (builderInstance is IFudgeObjectBuilder)
            {
                AddObjectBuilder(forClass, (IFudgeObjectBuilder)builderInstance);
            }
        }

        /// <summary>
        /// Add a class which is known to have a <seealso cref="GenericFudgeBuilderFor"/> annotation as builder. 
        /// </summary>
        /// <param name="className"> The fully qualified name of the builder class. </param>
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") public void addAnnotatedGenericBuilderClass(String className)
        public virtual void addAnnotatedGenericBuilderClass(string className)
        {
            Type builderClass = instantiateBuilderClass(className);

            if ((builderClass == null) || !(builderClass.GetCustomAttributes(typeof(GenericFudgeBuilderFor), false).Length > 0))
            {
                return;
            }

            object builderInstance = null;
            try
            {
                builderInstance = Activator.CreateInstance(builderClass);
            }
            catch (Exception e)
            {
                // Do nothing other than stack trace.
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                return;
            }

            Type forClass = builderClass.GetCustomAttributes(typeof(GenericFudgeBuilderFor), false).FirstOrDefault() as Type;

            if (!(builderInstance is IFudgeBuilder))
            {
                throw new System.ArgumentException("Annotated a generic builder " + builderClass + " but not a full FudgeBuilder<> implementation.");
            }
            DefaultBuilderFactory.AddGenericBuilder(forClass, (IFudgeBuilder)builderInstance);
        }

        /// <param name="className">
        /// @return </param>
        private Type instantiateBuilderClass(string className)
        {
            Type builderClass = null;
            try
            {
                builderClass = Type.GetType(className);
            }
            catch (Exception e)
            {
                // Silently swallow. Can't actually populate it.
                // This should be rare, and you can just stop at this breakpoint
                // (which is why the stack trace is here at all).
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
            return builderClass;
        }

    }

}