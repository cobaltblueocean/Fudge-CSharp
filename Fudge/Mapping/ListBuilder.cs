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
using System.Collections.Generic;
using Fudge.Serialization;

namespace Fudge.Mapping
{


    using IndicatorFieldType = Fudge.Types.IndicatorFieldType;
    using IndicatorType = Fudge.Types.IndicatorType;

    /// <summary>
    /// Builder for List objects.
    /// 
    /// @author Andrew Griffin
    /// </summary>
    /* package */
    internal class ListBuilder : IFudgeBuilder<IList<Object>>
    {

        /// <summary>
        /// Singleton instance of the <seealso cref="ListBuilder"/>.
        /// </summary>
        private static ListBuilder _instance = new ListBuilder();

        private ListBuilder()
        {
        }

        public static ListBuilder Instance()
        {
            return _instance;
        }


        /// <summary>
        /// Creates a Fudge message representation of a <seealso cref="List"/>.
        /// </summary>
        /// <param name="context"> the serialization context </param>
        /// <param name="list"> the list to serialize </param>
        /// <returns> the Fudge message </returns>
        public override IMutableFudgeFieldContainer BuildMessage<T1>(FudgeSerializer context, IList<T1> list)
        {
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer msg = context.newMessage();
            IMutableFudgeFieldContainer msg = context.NewMessage();
            foreach (object entry in list)
            {
                if (entry == null)
                {
                    msg.Add(null, null, IndicatorFieldType.Instance, IndicatorType.Instance);
                }
                else
                {
                    context.ObjectToFudgeMsgWithClassHeaders(msg, null, null, entry);
                }
            }
            return msg;
        }

        /// <summary>
        /// Creates a list from a Fudge message.
        /// </summary>
        /// <param name="context"> the deserialization context </param>
        /// <param name="message"> the Fudge message </param>
        /// <returns> the <seealso cref="List"/> </returns>
        //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
        //ORIGINAL LINE: @Override public java.util.List buildObject(FudgeDeserializationContext context, org.fudgemsg.FudgeFieldContainer message)
        public override IList<Object> BuildObject(FudgeDeserializer context, IFudgeFieldContainer message)
        {
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final java.util.List<Object> list = new java.util.ArrayList<Object> ();
            IList<Object> list = new List<Object>();
            foreach (IFudgeField field in message)
            {
                if ((field.Ordinal != null) && (field.Ordinal != 1))
                {
                    throw new System.ArgumentException("Sub-message doesn't contain a list (bad field " + field + ")");
                }
                object o = context.fieldValueToObject(field);
                list.Add((o is IndicatorType) ? null : o);
            }
            return list;
        }
    }
}