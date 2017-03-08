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
using System;
using System.Security;
using System.Reflection;

namespace Fudge.Mapping
{



    /// <summary>
    /// <para>Attempt to create an object using a no-arg constructor and Java-bean style
    /// setX methods.</para>
    /// 
    /// <para>This has been superceded by the DotNetAssemblyBuilder which uses the BeanUtils package.</para>
    /// </summary>
    /// @param <T> class that can be deserialized using this builder
    /// @author Andrew Griffin </param>
    /* package */
    internal class ReflectionObjectBuilder<T> : ReflectionBuilderBase<T>, IFudgeObjectBuilder
    {

        private readonly Type _assemblyType;

        /// <summary>
        /// Creates a new <seealso cref="ReflectionObjectBuilder"/> for a class if possible. This is only possible if the class
        /// has a visible no-arg constructor.
        /// </summary>
        /// @param <T> class to build objects of </param>
        /// <param name="clazz"> class to build objects of </param>
        /// <returns> the {@code ReflectionObjectBuilder} </returns>
        /* package */
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: static <T> ReflectionObjectBuilder<T> create(final Class clazz)
        internal static ReflectionObjectBuilder<T> create<T>(Type clazz)
        {
            try
            {
                //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final Constructor<T> constructor = clazz.getConstructor();
                Type constructor = clazz;
                return new ReflectionObjectBuilder<T>(clazz);
            }
            catch (SecurityException)
            {
                // ignore
            }
            catch (NotImplementedException)
            {
                // ignore
            }
            return null;
        }

        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: private ReflectionObjectBuilder(final Class clazz, final Constructor<T> constructor)
        private ReflectionObjectBuilder(Type clazz)
            : base(clazz, "set", 1, null)
        {
            _assemblyType = clazz;
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: @Override public T buildObject(final FudgeDeserializationContext context, final org.fudgemsg.FudgeFieldContainer message)
        public override T BuildObject(FudgeDeserializer context, IFudgeFieldContainer message)
        {
            try
            {
                //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final T base = _constructor.newInstance();
                T @base = (T)Activator.CreateInstance(_assemblyType.GetType());

                foreach (IFudgeField field in message.GetAllFields())
                {
                    //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    //ORIGINAL LINE: final Method method = getMethods().get(field.getName());
                    MethodInfo method = Methods[field.Name];
                    if (method != null)
                    {
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final Class[] params = method.getParameterTypes();
                        ParameterInfo[] @params = method.GetParameters();
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final Object v;
                        object v;
                        if (@params[0] is object)
                        {
                            v = context.fieldValueToObject(field);
                        }
                        else
                        {
                            v = context.fieldValueToObject(field);
                        }
                        //System.out.println ("m: " + method + " v:" + v);
                        method.Invoke(@base, new Object[] { v });
                    }
                }
                return @base;
            }
            catch (System.ArgumentException e)
            {
                throw new FudgeRuntimeException("Couldn't create " + _assemblyType.FullName + " object", e);
            }
            catch (TypeLoadException e)
            {
                throw new FudgeRuntimeException("Couldn't create " + _assemblyType.FullName + " object", e);
            }
            catch (MissingMethodException e)
            {
                throw new FudgeRuntimeException("Couldn't create " + _assemblyType.FullName + " object", e);
            }
            catch (TargetInvocationException e)
            {
                throw new FudgeRuntimeException("Couldn't create " + _assemblyType.FullName + " object", e);
            }
        }
    }
}