// <summary>
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
    /// Implementation of FudgeObjectBuilder for a class which supports a fromFudgeMsg
    /// function of the form:
    /// 
    ///    static <T> fromFudgeMsg (FudgeFieldContainer) or
    ///    static <T> fromFudgeMsg (FudgeDeserialisationContext, FudgeFieldContainer)
    /// </summary>
    /// @param <T> class supporting a {@code fromFudgeMsg} method which can be deserialised by this builder
    /// @author Andrew Griffin </param>
    /* package */
    internal class FromFudgeMsgObjectBuilder<T> : IFudgeObjectBuilder
    {

        /// <summary>
        /// Creates a new <seealso cref="FromFudgeMsgObjectBuilder"/> if possible.
        /// </summary>
        /// @param <T> target class to build from the message </param>
        /// <param name="clazz"> target class to build from the message </param>
        /// <returns> the <seealso cref="FromFudgeMsgObjectBuilder"/> or {@code null} if none is available </returns>
        /* package */
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: static <T> FromFudgeMsgObjectBuilder<T> create(final Class clazz)
        internal static FromFudgeMsgObjectBuilder<T> create<T>(Type clazz)
        {
            try
            {
                return new FromFudgeMsgObjectBuilder<T>(clazz.GetMethod("fromFudgeMsg"), true);
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

        private readonly MethodInfo _fromFudgeMsg;
        private readonly bool _passContext;

        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        //ORIGINAL LINE: private FromFudgeMsgObjectBuilder(final Method fromFudgeMsg, final boolean passContext)
        private FromFudgeMsgObjectBuilder(MethodInfo fromFudgeMsg, bool passContext)
        {
            _fromFudgeMsg = fromFudgeMsg;
            _passContext = passContext;
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public T buildObject(final FudgeDeserializationContext context, final org.fudgemsg.FudgeFieldContainer message)
        //JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
        public override T BuildObject(FudgeDeserializer context, IFudgeFieldContainer message)
        {
            try
            {
                return (T)(_passContext ? _fromFudgeMsg.Invoke(null, new object[] { context, message }) : _fromFudgeMsg.Invoke(null, new object[] { message }));
            }
            catch (System.ArgumentException e)
            {
                throw new FudgeRuntimeException("Couldn't call fromFudgeMsg", e);
            }
            catch (MissingMethodException e)
            {
                throw new FudgeRuntimeException("Couldn't call fromFudgeMsg", e);
            }
            catch (TargetInvocationException e)
            {
                throw new FudgeRuntimeException("Couldn't call fromFudgeMsg", e);
            }
        }

    }
}