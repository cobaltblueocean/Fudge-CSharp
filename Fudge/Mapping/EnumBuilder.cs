using System;
using System.Collections.Generic;
using Fudge.Serialization;
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
    /// Builder for enumerations. Note that an enumeration could alternatively be reduced
    /// to a string but that then requires forced knowledge at the receiver.
    /// </summary>
    /// @param <E> enumeration type
    /// @author Andrew Griffin </param>
    /* package */
    internal class EnumBuilder<E> : IFudgeBuilder<Enum>
    {

        private readonly Enum _clazz;

        /// <param name="clazz"> type of the enumeration </param>
        /* package */
        internal EnumBuilder(Enum clazz)
        {
            if (!typeof(E).IsEnum)
            {
                throw new ArgumentException("E must be Enum.");
            }
            _clazz = clazz;
        }

        /// <summary>
        /// Creates a Fudge message representation of an <seealso cref="Enum"/>.
        /// </summary>
        /// <param name="context"> the serialization context </param>
        /// <param name="enumeration"> the enum to serialize </param>
        /// <returns> the Fudge message </returns>
        public override IMutableFudgeFieldContainer buildMessage(FudgeSerializer context, Enum enumeration)
        {
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer msg = context.newMessage();
            IMutableFudgeFieldContainer msg = context.NewMessage();
            // REVIEW: jim 2-Jun-2010 -- changed to getDeclaringClass() to fix problem with enums with methods that appear as anon inner classes.
            msg.Add(null, 0, enumeration.GetType().Name);
            msg.Add(null, 1, Enum.GetUnderlyingType(enumeration.GetType()).Name);
            return msg;
        }

        /// <summary>
        /// Creates an enum from a Fudge message.
        /// </summary>
        /// <param name="context"> the deserialization context </param>
        /// <param name="message"> the Fudge message </param>
        /// <returns> the <seealso cref="Enum"/> </returns>
        public override Enum buildObject(FudgeDeserializer context, IFudgeFieldContainer message)
        {
            //return Enum.valueOf(_clazz, message.getString(1));
            return _clazz.valueOf(message.GetString(1));
        }

    }
}