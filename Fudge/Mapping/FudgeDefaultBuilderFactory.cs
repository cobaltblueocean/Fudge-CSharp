using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Resources;
using System.Reflection;
using System.Globalization;
using Fudge;
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



    //using DBObject = MongoDB.Driver.DBObject;

    /// <summary>
    /// <para>Default factory for building Fudge message encoders and decoders.</para>
    /// 
    /// <para>Building a Fudge message:</para>
    /// <ul>
    ///   <li>If the object has a public {@code toFudgeMsg} method, that will be used</li>
    ///   <li>Otherwise the <seealso cref="DotNetAssemblyBuilder"/> will be used</li>
    /// </ul>
    ///    
    /// <para>Building an object:</para>
    /// <ul>
    ///   <li>If the object has a public {@code fromFudgeMsg} method, that will be used</li>
    ///   <li>If the object has a public constructor that takes a <seealso cref="IFudgeFieldContainer"/>, that will be used</li>
    ///   <li>Otherwise the <seealso cref="DotNetAssemblyBuilder"/> will be used</li>
    /// </ul>
    ///  
    /// <para>Generic builders are provided for <seealso cref="Map"/>, <seealso cref="List"/> (and <seealso cref="Set"/>), <seealso cref="IFudgeFieldContainer"/>, <seealso cref="DBObject"/> and array types.</para>
    /// 
    /// @author Andrew Griffin
    /// </summary>
    public class FudgeDefaultBuilderFactory : IFudgeBuilderFactory
    {


        private readonly ConcurrentDictionary<Type, IFudgeObjectBuilder> _genericObjectBuilders;
        private readonly IList<MessageBuilderMapEntry<Type, IFudgeObjectBuilder>> _genericMessageBuilders;

        // TODO 2010-01-29 Andrew -- we could have a builder builder, e.g. search for static methods that return a FudgeObjectBuilder/FudgeMessageBuilder/FudgeBuilder instance for that class

        /// <summary>
        /// Creates a new factory. {@code org.fudgemsg.mapping.FudgeDefaultBuilderFactory.properties} will be read and used to initialize
        /// the generic builders.
        /// </summary>

        public FudgeDefaultBuilderFactory()
        {

            _genericObjectBuilders = new ConcurrentDictionary<Type, IFudgeObjectBuilder>();
            _genericMessageBuilders = new SynchronizedCollection<MessageBuilderMapEntry<Type, IFudgeObjectBuilder>>();

            #region "Original Java Code"
            //_genericObjectBuilders = new ConcurrentHashMap<Class<?>,FudgeObjectBuilder<?>> ();
            //_genericMessageBuilders = new CopyOnWriteArrayList<MessageBuilderMapEntry> ();
            //final ResourceBundle genericBuilders = ResourceBundle.getBundle (getClass ().getName ());
            //for (final String javaClassName : genericBuilders.keySet ()) {
            //  final String builderName = genericBuilders.getString (javaClassName);
            //  try {
            //    addGenericBuilderInternal (Class.forName (javaClassName), (FudgeBuilder)Class.forName (builderName).getDeclaredField ("INSTANCE").get (null));
            //  } catch (ClassNotFoundException e) {
            //    // ignore; e.g. if DBObject isn't in the classpath
            //  } catch (Exception e) {
            //    throw new FudgeRuntimeException ("couldn't register builder for " + javaClassName + " (" + builderName + ")", e);
            //  }
            //}
            #endregion

            var asm = Assembly.GetExecutingAssembly();
            Type[] ts = asm.GetTypes();
            foreach (Type t in ts)
            {
                string builderName = t.FullName;
                string javaClassName = t.FullName;


            }


            ResourceManager genericBuilders = new ResourceManager(this.GetType().FullName, Assembly.GetExecutingAssembly());
            System.Collections.IDictionaryEnumerator id = genericBuilders.GetResourceSet(CultureInfo.CurrentCulture, true, true).GetEnumerator();



            //foreach (ResourceSet javaClassName in (genericBuilders.GetResourceSet(CultureInfo.CurrentCulture, true, true).GetEnumerator() as Enumerable))
            while (id.MoveNext())
            {
                string builderName = id.Key.ToString();
                string javaClassName = id.Value.GetType().FullName;
                try
                {
                    //.getDeclaredField("INSTANCE").get(null)
                    //AddGenericBuilderInternal<Type, IFudgeObjectBuilder>(id.Value.GetType(), (IFudgeObjectBuilder)id.Value.GetType().GetField("INSTANCE").DeclaringType);
                    AddGenericBuilderInternal<id.Value.GetType()> ((IFudgeObjectBuilder<id.Value.GetType()>)id.Value.GetType().GetField("INSTANCE").DeclaringType);
                }
                catch (TypeLoadException)
                {
                    // ignore; e.g. if DBObject isn't in the classpath
                }
                catch (Exception e)
                {
                    throw new FudgeRuntimeException("couldn't register builder for " + javaClassName + " (" + builderName + ")", e);
                }
            }
        }

        /// <summary>
        /// Creates a new factory as a clone of another.
        /// </summary>
        /// <param name="other"> the factory to clone </param>
        /* package *///JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
                     //ORIGINAL LINE: FudgeDefaultBuilderFactory(final FudgeDefaultBuilderFactory other)
        internal FudgeDefaultBuilderFactory(FudgeDefaultBuilderFactory other)
        {
            //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
            //ORIGINAL LINE: _genericObjectBuilders = new java.util.concurrent.ConcurrentHashMap<Class,FudgeObjectBuilder> (other._genericObjectBuilders);
            _genericObjectBuilders = new ConcurrentDictionary<Type, IFudgeObjectBuilder>(other._genericObjectBuilders);
            _genericMessageBuilders = new SynchronizedCollection<MessageBuilderMapEntry<Type, IFudgeObjectBuilder>>(other._genericMessageBuilders);
        }

        //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
        //ORIGINAL LINE: private java.util.Map<Class,FudgeObjectBuilder> getGenericObjectBuilders()
        private IDictionary<Type, IFudgeObjectBuilder> GenericObjectBuilders
        {
            get
            {
                return _genericObjectBuilders;
            }
        }

        private IList<MessageBuilderMapEntry<Type, IFudgeObjectBuilder>> GenericMessageBuilders
        {
            get
            {
                return _genericMessageBuilders;
            }
        }

        /// <summary>
        /// If the object has a public fromFudgeMsg method, that will be used. Otherwise, if it has a
        /// public constructor that takes a FudgeFieldContainer, that will be used. Registered default
        /// builders for classes list Map and List will be tried, failing that the DotNetAssemblyBuilder will
        /// be used.
        /// </summary>
        /// @param <T> Java type of the class a builder is requested for </param>
        /// <param name="clazz"> Java class a builder is requested for </param>
        /// <returns> a <seealso cref="FudgeObjectBuilder"/> or {@code null} if no suitable builder can be created </returns>
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> FudgeObjectBuilder<T> createObjectBuilder(final Class clazz)
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        public virtual IFudgeObjectBuilder<T> CreateObjectBuilder<T>(Type clazz)
        {
            IFudgeObjectBuilder builder;
            if ((builder = CreateObjectBuilderFromAnnotation<T>(clazz)) != null)
            {
                return (IFudgeObjectBuilder<T>)builder;
            }
            if ((builder = (IFudgeObjectBuilder)FromFudgeMsgObjectBuilder<T>.create<T>(clazz)) != null)
            {
                return (IFudgeObjectBuilder<T>)builder;
            }
            if ((builder = (IFudgeObjectBuilder)FudgeMsgConstructorObjectBuilder<T>.create<T>(clazz)) != null)
            {
                return (IFudgeObjectBuilder<T>)builder;
            }
            if (clazz.IsArray)
            {
                return new ArrayBuilder<T>(clazz.GetElementType());
            }
            if (clazz.IsSubclassOf(typeof(Enum)))
            {
                return (IFudgeObjectBuilder<T>)(IFudgeObjectBuilder)(new EnumBuilder<T>((Enum)(Object)clazz));
            }
            if ((builder = (IFudgeObjectBuilder)GenericObjectBuilders[clazz]) != null)
            {
                return (IFudgeObjectBuilder<T>)builder;
            }
            if (clazz.IsInterface)
            {
                return null;
            }
            //return ReflectionObjectBuilder.create (clazz);
            return DotNetAssemblyBuilder<T>.create<T>(clazz);
        }

        /// <summary>
        /// Attempt to construct a <seealso cref="FudgeObjectBuilder"/> for the specified type based on the presence
        /// of a <seealso cref="HasFudgeBuilder"/> annotation on that type.
        /// </summary>
        /// @param <T> Java type of the class a builder is requested for </param>
        /// <param name="clazz"> Java class a builder is requested for </param>
        /// <returns> A <seealso cref="FudgeObjectBuilder"/> based on <seealso cref="HasFudgeBuilder"/> annotation, or {@code null}. </returns>
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") protected <T> FudgeObjectBuilder<T> createObjectBuilderFromAnnotation(final Class clazz)
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        protected internal virtual IFudgeObjectBuilder CreateObjectBuilderFromAnnotation<T>(Type clazz)
        {
            if (clazz.GetCustomAttributes(typeof(HasFudgeBuilder)) == null)
            {
                return null;
            }
            HasFudgeBuilder annotation = (HasFudgeBuilder)clazz.GetCustomAttributes(typeof(HasFudgeBuilder));
            Type objectBuilderClass = null;
            if (!typeof(object).Equals(annotation.builder))
            {
                objectBuilderClass = annotation.builder;
            }
            else if (!typeof(object).Equals(annotation.objectBuilder))
            {
                objectBuilderClass = annotation.objectBuilder;
            }

            if (objectBuilderClass == null)
            {
                return null;
            }

            if (!objectBuilderClass.IsSubclassOf(typeof(IFudgeObjectBuilder)))
            {
                return null;
            }

            IFudgeObjectBuilder result = null;
            try
            {
                result = (IFudgeObjectBuilder)objectBuilderClass.newInstance();
            }
            catch (Exception e)
            {
                throw new FudgeRuntimeException("Unable to instantiate annotated object builder class " + objectBuilderClass, e);
            }

            return result;
        }

        /// <summary>
        /// If the object has a public toFudgeMsg method, that will be used. Otherwise the
        /// DotNetAssemblyBuilder will be used.
        /// </summary>
        /// @param <T> Java type of the class a builder is requested for </param>
        /// <param name="clazz"> Java class a builder is requested for </param>
        /// <returns> a <seealso cref="FudgeMessageBuilder"/> or {@code null} if no suitable builder can be created </returns>
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> FudgeMessageBuilder<T> createMessageBuilder(final Class clazz)
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        public virtual IFudgeMessageBuilder CreateMessageBuilder<T>(Type clazz)
        {
            IFudgeMessageBuilder builder;
            if ((builder = CreateMessageBuilderFromAnnotation<T>(clazz)) != null)
            {
                return builder;
            }
            if ((builder = ToFudgeMsgMessageBuilder<T>.create(clazz)) != null)
            {
                return builder;
            }
            if (clazz.IsArray)
            {
                return new ArrayBuilder<T>(clazz.GetElementType());
            }
            if (clazz.IsSubclassOf(typeof(Enum)))
            {
                return new EnumBuilder<T>((Enum)(Object)clazz);
            }
            foreach (IMessageBuilderMapEntry defaultBuilder in GenericMessageBuilders)
            {
                if (defaultBuilder.GetType().IsAssignableFrom(clazz))
                {
                    return (IFudgeMessageBuilder)(defaultBuilder.MessageBuilder());
                }
            }
            //return ReflectionMessageBuilder.create (clazz);
            return DotNetAssemblyBuilder<T>.create<T>(clazz);
        }

        /// <summary>
        /// Attempt to construct a <seealso cref="FudgeObjectBuilder"/> for the specified type based on the presence
        /// of a <seealso cref="HasFudgeBuilder"/> annotation on that type.
        /// </summary>
        /// @param <T> Java type of the class a builder is requested for </param>
        /// <param name="clazz"> Java class a builder is requested for </param>
        /// <returns> A <seealso cref="FudgeObjectBuilder"/> based on <seealso cref="HasFudgeBuilder"/> annotation, or {@code null}. </returns>
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") protected <T> FudgeMessageBuilder<T> createMessageBuilderFromAnnotation(final Class clazz)
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        protected internal virtual IFudgeMessageBuilder CreateMessageBuilderFromAnnotation<T>(Type clazz)
        {
            if (clazz.GetCustomAttribute(typeof(HasFudgeBuilder)) != null)
            {
                return null;
            }
            HasFudgeBuilder attribute = (HasFudgeBuilder)clazz.GetCustomAttribute(typeof(HasFudgeBuilder));
            Type messageBuilderClass = null;
            if (!typeof(object).Equals(attribute.builder))
            {
                messageBuilderClass = attribute.builder;
            }
            else if (!typeof(object).Equals(attribute.messageBuilder))
            {
                messageBuilderClass = attribute.messageBuilder;
            }

            if (messageBuilderClass == null)
            {
                return null;
            }

            if (!messageBuilderClass.IsSubclassOf(typeof(IFudgeMessageBuilder)))
            {
                return null;
            }

            IFudgeMessageBuilder result = null;
            try
            {
                result = (IFudgeMessageBuilder)messageBuilderClass.newInstance();
            }
            catch (Exception e)
            {
                throw new FudgeRuntimeException("Unable to instantiate annotated message builder class " + messageBuilderClass, e);
            }

            return result;
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: @Override public <T> void addGenericBuilder(final Class clazz, final FudgeBuilder<T> builder)
        public virtual void AddGenericBuilder<T>(IFudgeBuilder<T> builder)
        {
            AddGenericBuilderInternal <T> (builder);
        }

        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: private <T> void addGenericBuilderInternal(final Class clazz, final FudgeBuilder<? extends T> builder)
        private void AddGenericBuilderInternal<T>(IFudgeBuilder<T> builder)
        {
            GenericObjectBuilders[typeof(T)] = (IFudgeObjectBuilder)builder;
            GenericMessageBuilders.Insert(0, new MessageBuilderMapEntry<T, IFudgeObjectBuilder<T>> (typeof(T), (IFudgeObjectBuilder<T>)builder));
        }

        IFudgeObjectBuilder<T> IFudgeBuilderFactory.CreateObjectBuilder<T>(Type clazz)
        {
            throw new NotImplementedException();
        }

        IFudgeMessageBuilder<T> IFudgeBuilderFactory.CreateMessageBuilder<T>(Type clazz)
        {
            throw new NotImplementedException();
        }

        void IFudgeBuilderFactory.AddGenericBuilder<T>(Type clazz, IFudgeBuilder<T> builder)
        {
            throw new NotImplementedException();
        }

        internal interface IMessageBuilderMapEntry
        {
            Type Clazz();
            IFudgeMessageBuilder MessageBuilder();

        }

        internal class MessageBuilderMapEntry<T, T1> : IMessageBuilderMapEntry
        {
            internal Type _clazz;
            internal IFudgeMessageBuilder _builder;

            internal MessageBuilderMapEntry(Type clazz, IFudgeMessageBuilder builder) //where T1 : T
            {
                _clazz = clazz;
                _builder = builder;
            }

            public virtual Type Clazz()
            {
                return _clazz;
            }

            public virtual IFudgeMessageBuilder MessageBuilder()
            {
                return _builder;
            }
        }
    }
}