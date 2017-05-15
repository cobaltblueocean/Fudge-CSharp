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
using System.Linq;
using System.Collections.Generic;
using Fudge.Serialization;

namespace Fudge.Mapping
{



	/// <summary>
	/// Builder for Array objects (lists).
	/// </summary>
	/// @param <E> element type of the array
	/// @author Andrew Griffin </param>
    /* package */
    internal class ArrayBuilder<E> : IFudgeBuilder<E>
    {

        private readonly Type _clazz;

        /// <param name="clazz"> type of the array element </param>
        /* package */
        internal ArrayBuilder(Type clazz)
        {
            _clazz = clazz;
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public IMutableFudgeFieldContainer BuildMessage(FudgeSerializer context, object[] array)
        {
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer msg = context.newMessage();
            MutableFudgeMsg msg = context.NewMessage();
            foreach (object entry in array)
            {
                context.AddToMessage(msg, null, 0, entry);
            }
            return msg;
        }

        /// <summary>
        /// {docInherit}
        /// </summary>
        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public E[] buildObject(FudgeDeserializationContext context, org.fudgemsg.FudgeFieldContainer message)
        public E[] BuildObject(FudgeDeserializer context, IFudgeFieldContainer message)
        {
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final java.util.List list = ListBuilder.Instance.buildObject(context, message);
            //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
            IList<E> list = (IList<E>)(Object)ListBuilder.Instance().BuildObject(context, message);
            //return list.ToArray((E[])Array.CreateInstance(_clazz, list.Count));
            return list.ToArray();
        }

        IMutableFudgeFieldContainer IFudgeMessageBuilder<E>.BuildMessage(FudgeSerializer serializer, E obj)
        {
            throw new NotImplementedException();
        }

        E IFudgeObjectBuilder<E>.BuildObject(FudgeDeserializer context, IFudgeFieldContainer message)
        {
            throw new NotImplementedException();
        }
    }
}