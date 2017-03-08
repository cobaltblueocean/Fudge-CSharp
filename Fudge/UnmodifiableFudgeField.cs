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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fudge
{
    ///<summary>
    /// A single field in the Fudge system.
    /// 
    /// Each Fudge message consists of a list of fields.
    /// Each field consists of a Fudge type and value, with an optional name and ordinal.
    /// All four combinations of name and ordinal are possible - from both present to both absent.
    /// 
    /// The type of the value should match the stored Fudge type:
    /// Type.CSharpType.IsAssignableFrom(Value.GetType()) should be true.
    /// 
    /// Applications are recommended to use this interface rather than a concrete class.
    /// 
    /// This interface makes no guarantees about the mutability or thread-safety of implementations.
    ///</summary>
    public class UnmodifiableFudgeField : IFudgeField 
    {
        private FudgeFieldType _type;
        private Object _value;
        private short? _ordinal;
        private String _name;

        /// <summary>
        /// Constructor of the UnmodifiableFudgeField.
        /// </summary>
        /// <param name="type">the Fudge type of the value.</param>
        /// <param name="value">the field value.</param>
        /// <param name="ordinal">the optional field ordinal.</param>
        /// <param name="name">the optional field name.</param>
        public UnmodifiableFudgeField(FudgeFieldType type, Object value, short? ordinal, String name)
        {
            _type = type;
            _value = value;
            _ordinal = ordinal;
            _name = name;
        }

        /// <summary>
        /// Create a new instance of UnmodifiableFudgeField.
        /// </summary>
        /// <param name="type">the Fudge type of the value.</param>
        /// <param name="value">the field value.</param>
        /// <param name="ordinal">the optional field ordinal.</param>
        /// <param name="name">the optional field name.</param>
        /// <returns>new UnmodifiableFudgeField instance.</returns>
        public static UnmodifiableFudgeField of (FudgeFieldType type, Object value, short? ordinal, String name)
        {
            return new UnmodifiableFudgeField(type, value, ordinal, name);
        }

        /// <summary>
        /// Gets the Fudge type of the value.
        /// The type should match the value.
        /// </summary>
        public FudgeFieldType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets the field value.
        /// The type should match the value.
        /// </summary>
        public object Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets the optional field ordinal.
        /// The ordinal is a number that identifies the meaning of the data.
        /// It is typically a reference into the taxonomy.
        /// </summary>
        public short? Ordinal
        {
            get { return _ordinal; }
        }

        /// <summary>
        /// Gets the optional field name.
        /// The name is a string that identifies the meaning of the data.
        /// This is similar to the tag name in XML.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
    }
}
