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
using Fudge.Taxon;

namespace Fudge.Wire
{
    /// <summary>
    /// Utility for calculating the size of a Fudge message.
    /// 
    /// These utilities calculate the size of a stream without constructing a full message.
    /// 
    /// This class is a static utility with no shared state.
    /// </summary>
    public class FudgeSize
    {
        private static UTF8Encoding UTF8 = new UTF8Encoding();

        public static int calculateFieldSize(IFudgeTaxonomy taxonomy, String name, int? ordinal, FudgeFieldType type, Object value)
        {
            int size = 0;
            // field prefix
            size += 2;
            Boolean hasOrdinal = ordinal != null;
            Boolean hasName = name != null;
            if (name != null && taxonomy != null)
            {
                if (taxonomy.GetFieldOrdinal(name) != null)
                {
                    hasOrdinal = true;
                    hasName = false;
                }
            }
            if (hasOrdinal)
            {
                size += 2;
            }
            if (hasName)
            {
                // one for the size prefix
                size++;
                // then for the UTF Encoding

                size += UTF8.GetByteCount(name);
            }
            if (type.IsVariableSize)
            {
                int valueSize = type.FixedSize;
                if (valueSize <= 255)
                {
                    size += valueSize + 1;
                }
                else if (valueSize <= short.MaxValue)
                {
                    size += valueSize + 2;
                }
                else
                {
                    size += valueSize + 4;
                }
            }
            else
            {
                size += type.FixedSize;
            }
            return size;
        }

        public static int calculateFieldSize(IFudgeTaxonomy taxonomy, IFudgeField field)
        {
            return calculateFieldSize(taxonomy, field.Name, field.Ordinal, field.Type, field.Value);
        }

        public static int calculateFieldSize(IFudgeField field)
        {
            return calculateFieldSize(null, field.Name, field.Ordinal, field.Type, field.Value);
        }

        public static int calculateFieldSize(String name, int? ordinal, FudgeFieldType type, Object value)
        {
            return calculateFieldSize(null, name, ordinal, type, value);
        }

        public static int calculateFieldSize(int? ordinal, FudgeFieldType type, Object value)
        {
            return calculateFieldSize(null, null, ordinal, type, value);
        }

        public static int calculateFieldSize(String name, FudgeFieldType type, Object value)
        {
            return calculateFieldSize(null, name, null, type, value);
        }

        public static int calculateFieldSize(FudgeFieldType type, Object value)
        {
            return calculateFieldSize(null, null, null, type, value);
        }

        public static int calculateMessageSize(IFudgeTaxonomy taxonomy, FudgeMsg fields)
        {
            if (fields is FudgeEncoded)
            {
                FudgeEncoded fudgeEncoded = (FudgeEncoded)fields;
                byte[] encoded = (byte[])(Object)fudgeEncoded.GetFudgeEncoded();
                if (encoded != null)
                {
                    return encoded.Length;
                }
            }
            int bytes = 0;
            foreach (IFudgeField field in fields)
            {
                bytes += calculateFieldSize(taxonomy, field);
            }
            return bytes;
        }

        public static int calculateMessageSize(FudgeMsg fields)
        {
            return calculateMessageSize(null, fields);
        }

        public static int calculateMessageEnvelopeSize(IFudgeTaxonomy taxonomy, FudgeMsg fields)
        {
            return 8 + calculateMessageSize(taxonomy, fields);
        }

        public static int calculateMessageEnvelopeSize(FudgeMsg fields)
        {
            return 8 + calculateMessageSize(null, fields);
        }

        public static int calculateMessageEnvelopeSize(IFudgeTaxonomy taxonomy, FudgeMsgEnvelope envelope)
        {
            return 8 + calculateMessageSize(taxonomy, envelope.Message);
        }

        public static int calculateMessageEnvelopeSize(FudgeMsgEnvelope envelope)
        {
            return 8 + calculateMessageSize(null, envelope.Message);
        }
    }
}
