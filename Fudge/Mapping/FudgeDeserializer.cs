using System;
using System.Collections.Generic;
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
    /// <para>The central point for Fudge message to Java Object deserialization on a given stream.
    /// Note that the deserializer cannot process cyclic object graphs at the moment because
    /// of the way the builder interfaces are structured (i.e. we don't have access to an
    /// outer object until it's builder returned).</para>
    /// 
    /// <para>The object builder framework methods all take a deserialization context so that a
    /// deserializer can refer any sub-messages to this for construction if it does not have
    /// sufficient information to process them directly.</para> 
    /// 
    /// @author Andrew Griffin
    /// </summary>
    public class FudgeDeserializer
    {

        private readonly FudgeContext _fudgeContext;
        private readonly SerializationBuffer _serialisationBuffer = new SerializationBuffer();

        /// <summary>
        /// Creates a new <seealso cref="FudgeDeserializer"/> for the given <seealso cref="FudgeContext"/>.
        /// </summary>
        /// <param name="fudgeContext"> the {@code FudgeContext} to use </param>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: public FudgeDeserializationContext(final org.fudgemsg.FudgeContext fudgeContext)
        public FudgeDeserializer(FudgeContext fudgeContext)
        {
            _fudgeContext = fudgeContext;
        }

        /// <summary>
        /// Resets the buffers used for object graph logics. Calling {@code reset()} on this context
        /// should match a call to <seealso cref="FudgeSerializer#reset()"/> on the context used by the serialiser
        /// to keep the states of both sender and receiver consistent.
        /// </summary>
        public virtual void reset()
        {
            SerialisationBuffer.reset();
        }

        private SerializationBuffer SerialisationBuffer
        {
            get
            {
                return _serialisationBuffer;
            }
        }

        /// <summary>
        /// Returns the associated <seealso cref="FudgeContext"/>.
        /// </summary>
        /// <returns> the {@code FudgeContext}. </returns>
        public virtual FudgeContext FudgeContext
        {
            get
            {
                return _fudgeContext;
            }
        }

        /// <summary>
        /// Converts a field value to a Java object. This may be a base Java type supported by the current <seealso cref="FudgeTypeDictionary"/>
        /// or if it is a sub-message will be expanded through <seealso cref="#fudgeMsgToObject(FudgeFieldContainer)"/>.
        /// </summary>
        /// <param name="field"> field to convert </param>
        /// <returns> the deserialized object </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: public Object fieldValueToObject(final org.fudgemsg.FudgeField field)
        public virtual object fieldValueToObject(IFudgeField field)
        {
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final Object o = field.getValue();
            object o = field.Value;
            if (o is IFudgeFieldContainer)
            {
                return fudgeMsgToObject((IFudgeFieldContainer)o);
            }
            else
            {
                return o;
            }
        }

        /// <summary>
        /// Converts a field value to a Java object with a specific type. This may be a base Java type supported by the current 
        /// <seealso cref="FudgeTypeDictionary"/> or if it is a sub-message will be expanded through <seealso cref="#fudgeMsgToObject(Class,FudgeFieldContainer)"/>.
        /// </summary>
        /// @param <T> target Java type to decode to </param>
        /// <param name="clazz"> class of the target Java type to decode to </param>
        /// <param name="field"> value to decode </param>
        /// <returns> the deserialized object </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: public <T> T fieldValueToObject(final Class clazz, final org.fudgemsg.FudgeField field)
        public virtual T fieldValueToObject<T>(Type clazz, IFudgeField field)
        {
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final Object o = field.getValue();
            object o = field.Value;
            if (o is IFudgeFieldContainer)
            {
                return fudgeMsgToObject<T>(clazz, (IFudgeFieldContainer)o);
            }
            else
            {
                return (T)(Object)FudgeContext.GetFieldValue(clazz, field);
            }
        }

        /// <summary>
        /// Converts a Fudge message to a best guess Java object. <seealso cref="List"/> and <seealso cref="Map"/> encodings are recognized and inflated. Any other encodings
        /// require field ordinal 0 to include possible class names to use.
        /// </summary>
        /// <param name="message"> message to deserialize </param>
        /// <returns> the Java object </returns>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: public Object fudgeMsgToObject(final org.fudgemsg.FudgeFieldContainer message)
        public virtual Object fudgeMsgToObject(IFudgeFieldContainer message)
        {
            IList<IFudgeField> types = message.GetAllByOrdinal(0);
            if (types.Count == 0)
            {
                int maxOrdinal = 0;
                foreach (IFudgeField field in message)
                {
                    if (field.Ordinal == null)
                    {
                        continue;
                    }
                    if (field.Ordinal < 0)
                    {
                        // not a list/set/map
                        return message;
                    }
                    if (field.Ordinal > maxOrdinal)
                    {
                        maxOrdinal = (int)field.Ordinal;
                    }
                }
                //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final Class defaultClass = getFudgeContext().getObjectDictionary().getDefaultObjectClass(maxOrdinal);
                Type defaultClass = FudgeContext.ObjectDictionary.getDefaultObjectClass(maxOrdinal);
                if (defaultClass != null)
                {
                    return fudgeMsgToObject<Object>(defaultClass, message);
                }
            }
            else
            {
                foreach (IFudgeField type in types)
                {
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final Object o = type.getValue();
                    object o = type.Value;
                    if (o.IsNumber())
                    {
                        throw new System.NotSupportedException("Serialisation framework doesn't support back/forward references");
                    }
                    else if (o is string)
                    {
                        try
                        {
                            //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
                            //ORIGINAL LINE: FudgeObjectBuilder builder = getFudgeContext().getObjectDictionary().getObjectBuilder(Class.forName((String)o));
                            IFudgeObjectBuilder<Object> builder = FudgeContext.ObjectDictionary.GetObjectBuilder<Object>(o.GetType());
                            if (builder != null)
                            {
                                return builder.BuildObject(this, message);
                            }
                        }
                        catch (TypeLoadException)
                        {
                            // ignore
                        }
                    }
                }
            }
            // couldn't process - return the raw message
            return message;
        }

        /// <summary>
        /// Converts a Fudge message to a specific Java type. The <seealso cref="FudgeObjectDictionary"/> is used to identify a builder to delegate to. If
        /// the message includes class names in ordinal 0, these will be tested for a valid builder and used if they will provide a subclass of
        /// the requested class.
        /// </summary>
        /// @param <T> target Java type to decode to </param>
        /// <param name="clazz"> class of the target Java type to decode to </param>
        /// <param name="message"> message to deserialise </param>
        /// <returns> the deserialised Java object </returns>
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") public <T> T fudgeMsgToObject(final Class clazz, final org.fudgemsg.FudgeFieldContainer message)
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        public virtual T fudgeMsgToObject<T>(Type clazz, IFudgeFieldContainer message)
        {
            IFudgeObjectBuilder<T> builder;
            Exception lastError = null;
            /*if (clazz == Object.class) {
              System.out.println(message);
            }*/
            IList<IFudgeField> types = message.GetAllByOrdinal(0);
            if (types.Count != 0)
            {
                // message contains type information - use it if we can
                foreach (IFudgeField type in types)
                {
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final Object o = type.getValue();
                    object o = type.Value;
                    if (o.IsNumber())
                    {
                        throw new System.NotSupportedException("Serialisation framework doesn't support back/forward references");
                    }
                    else if (o is string)
                    {
                        try
                        {
                            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                            //ORIGINAL LINE: final Class possibleClazz = Class.forName((String)o);
                            Type possibleClazz = Type.GetType((string)o);
                            // System.out.println("Trying " + possibleClazz);
                            if (clazz.IsAssignableFrom(possibleClazz))
                            {
                                builder = (FudgeObjectBuilder<T>)FudgeContext.ObjectDictionary.GetObjectBuilder<T>(possibleClazz);
                                // System.out.println("Builder " + builder);
                                if (builder != null)
                                {
                                    return builder.BuildObject(this, message);
                                }
                            }
                        }
                        catch (TypeLoadException)
                        {
                            // ignore
                        }
                        catch (Exception e)
                        {
                            //e.printStackTrace();
                            lastError = e;
                        }
                    }
                }
            }
            // try the requested type
            //System.out.println ("fallback to " + clazz);
            builder = FudgeContext.ObjectDictionary.GetObjectBuilder<T>(clazz);
            if (builder != null)
            {
                try
                {
                    return builder.BuildObject(this, message);
                }
                catch (Exception e)
                {
                    lastError = e;
                }
            }
            // nothing matched
            if (lastError != null)
            {
                throw new FudgeRuntimeException("Don't know how to create " + clazz + " from " + message, lastError);
            }
            else
            {
                throw new System.ArgumentException("Don't know how to create " + clazz + " from " + message);
            }
        }
    }
}