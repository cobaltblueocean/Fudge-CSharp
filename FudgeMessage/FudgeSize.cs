// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Incd and the OpenGamma group of companies
//
// Please see distribution for license.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
//     
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FudgeMessage.Taxon;

namespace FudgeMessage
{
    /// <summary>
    /// Utility for calculating the size of a Fudge message.
    /// <p>
    /// These utilities calculate the size of a stream without constructing a full message.
    /// <p>
    /// This class is a static utility with no shared state.
    /// </summary>
    public class FudgeSize
    {

        /// <summary>
        /// Calculates the size of a field (field header and value payload) in the Fudge stream in bytes.
        /// <p>
        /// The calculation takes account of the value being reduced to fit in a smaller space.
        /// 
        /// </summary>
        /// <typeparam Name="T">the underlying Java type of the field data</typeparam>
        /// <param Name="taxonomy"> the taxonomy in use, null if no taxonomy</param>
        /// <param Name="ordinal"> the field ordinal, null if no ordinal</param>
        /// <param Name="Name"> the field Name, null if no Name</param>
        /// <param Name="type"> the Fudge field type, not null</param>
        /// <param Name="value"> the field value</param>
        /// <returns>the number of bytes</returns>
        public static int CalculateFieldSize<T>(IFudgeTaxonomy taxonomy, short? ordinal, String Name, FudgeFieldType<T> type, T value)
        {
            int size = 0;
            // field prefix
            size += 2;
            Boolean hasOrdinal = ordinal != null;
            Boolean hasName = Name != null;
            if (Name != null && taxonomy != null)
            {
                if (taxonomy.GetFieldOrdinal(Name) != null)
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
                size += UTF8.GetLengthBytes(Name);
            }
            if (type.IsVariableSize)
            {
                int valueSize = type.GetVariableSize(value, taxonomy);
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

        /// <summary>
        /// Calculates the size of a field (field header and value payload) in the Fudge stream in bytes.
        /// <p>
        /// The calculation takes account of the value being reduced to fit in a smaller space.
        /// 
        /// </summary>
        /// <typeparam Name="T">the underlying Java type of the field data</typeparam>
        /// <param Name="taxonomy"> the taxonomy in use, null if no taxonomy</param>
        /// <param Name="field"> the field to calculate a size for, not null</param>
        /// <returns>the number of bytes</returns>
        // @SuppressWarnings("unchecked")
        public static int CalculateFieldSize<T>(IFudgeTaxonomy taxonomy, IFudgeField field)
        {
            return CalculateFieldSize(taxonomy, field.Ordinal.Value, field.Name, (FudgeFieldType<T>)field.Type, (T)field.Value);
        }

        /// <summary>
        /// Calculates the size of a field (field header and value payload) in the Fudge stream in bytes when no taxonomy is used.
        /// <p>
        /// The calculation takes account of the value being reduced to fit in a smaller space.
        /// 
        /// </summary>
        /// <typeparam Name="T">the underlying Java type of the field data</typeparam>
        /// <param Name="field"> the field to calculate a size for, not null</param>
        /// <returns>the number of bytes</returns>
        // @SuppressWarnings("unchecked")
        public static int CalculateFieldSize<T>(IFudgeField field)
        {
            return CalculateFieldSize(null, field.Ordinal.Value, field.Name, (FudgeFieldType<T>)field.Type, (T)field.Value);
        }

        /// <summary>
        /// Calculates the size of a field (field header and value payload) in the Fudge stream in bytes when no taxonomy is used.
        /// <p>
        /// The calculation takes account of the value being reduced to fit in a smaller space.
        /// 
        /// </summary>
        /// <typeparam Name="T">the underlying Java type of the field data</typeparam>
        /// <param Name="ordinal"> the field ordinal, null if no ordinal</param>
        /// <param Name="Name"> the field Name, null if no Name</param>
        /// <param Name="type"> the Fudge field type, not null</param>
        /// <param Name="value"> the field value</param>
        /// <returns>the number of bytes</returns>
        public static int CalculateFieldSize<T>(short ordinal, String Name, FudgeFieldType<T> type, T value)
        {
            return CalculateFieldSize(null, ordinal, Name, type, value);
        }

        /// <summary>
        /// Calculates the size of a field (field header and value payload) in the Fudge stream in bytes when no taxonomy is used.
        /// <p>
        /// The calculation takes account of the value being reduced to fit in a smaller space.
        /// 
        /// </summary>
        /// <typeparam Name="T">the underlying Java type of the field data</typeparam>
        /// <param Name="ordinal"> the field ordinal, null if no ordinal</param>
        /// <param Name="type"> the Fudge field type, not null</param>
        /// <param Name="value"> the field value</param>
        /// <returns>the number of bytes</returns>
        public static int CalculateFieldSize<T>(short ordinal, FudgeFieldType<T> type, T value)
        {
            return CalculateFieldSize(null, ordinal, null, type, value);
        }

        /// <summary>
        /// Calculates the size of a field (field header and value payload) in the Fudge stream in bytes when no taxonomy is used.
        /// <p>
        /// The calculation takes account of the value being reduced to fit in a smaller space.
        /// 
        /// </summary>
        /// <typeparam Name="T">the underlying Java type of the field data</typeparam>
        /// <param Name="Name"> the field Name, null if no Name</param>
        /// <param Name="type"> the Fudge field type, not null</param>
        /// <param Name="value"> the field value</param>
        /// <returns>the number of bytes</returns>
        public static int CalculateFieldSize<T>(String Name, FudgeFieldType<T> type, T value)
        {
            return CalculateFieldSize(null, null, Name, type, value);
        }

        /// <summary>
        /// Calculates the size of a field (field header and value payload) in the Fudge stream in bytes when no taxonomy is used.
        /// <p>
        /// The calculation takes account of the value being reduced to fit in a smaller space.
        /// 
        /// </summary>
        /// <typeparam Name="T">the underlying Java type of the field data</typeparam>
        /// <param Name="type"> the Fudge field type, not null</param>
        /// <param Name="value"> the field value</param>
        /// <returns>the number of bytes</returns>
        public static int CalculateFieldSize<T>(FudgeFieldType<T> type, T value)
        {
            return CalculateFieldSize(null, null, null, type, value);
        }

        //-------------------------------------------------------------------------
        /// <summary>
        /// Calculates the size of a message as the sum of the fields.
        /// 
        /// </summary>
        /// <param Name="taxonomy"> the taxonomy in use, null if no taxonomy</param>
        /// <param Name="fields"> the fields to calculate a size for, not null</param>
        /// <returns>the number of bytes</returns>
        public static int CalculateMessageSize(IFudgeTaxonomy taxonomy, IFudgeFieldContainer fields)
        {
            int bytes = 0;
            foreach (IFudgeField field in fields.GetAllFields())
            {
                Type MyType = field.Value.GetType();

                var method = typeof(FudgeSize).GetMethod("CalculateFieldSize");
                var genericMethod = method.MakeGenericMethod(MyType);

                bytes += (int)genericMethod.Invoke(null, new Object[] { taxonomy, field });
            }
            return bytes;
        }

        /// <summary>
        /// Calculates the size of a message as the sum of the fields when no taxonomy is used.
        /// 
        /// </summary>
        /// <param Name="fields"> the fields to calculate a size for, not null</param>
        /// <returns>the number of bytes</returns>
        public static int CalculateMessageSize(IFudgeFieldContainer fields)
        {
            return CalculateMessageSize(null, fields);
        }

        //-------------------------------------------------------------------------
        /// <summary>
        /// Calculates the size of a message including the envelope header.
        /// 
        /// </summary>
        /// <param Name="taxonomy"> the taxonomy in use, null if no taxonomy</param>
        /// <param Name="fields"> the fields to calculate a size for, not null</param>
        /// <returns>the number of bytes</returns>
        public static int CalculateMessageEnvelopeSize(IFudgeTaxonomy taxonomy, IFudgeFieldContainer fields)
        {
            return 8 + CalculateMessageSize(taxonomy, fields);
        }

        /// <summary>
        /// Calculates the size of a message including the envelope header when no taxonomy is used.
        /// 
        /// </summary>
        /// <param Name="fields"> the fields to calculate a size for, not null</param>
        /// <returns>the number of bytes</returns>
        public static int CalculateMessageEnvelopeSize(IFudgeFieldContainer fields)
        {
            return 8 + CalculateMessageSize(null, fields);
        }

        /// <summary>
        /// Calculates the size of a message including the envelope header.
        /// 
        /// </summary>
        /// <param Name="taxonomy"> the taxonomy in use, null if no taxonomy</param>
        /// <param Name="envelope"> the message envelope to calculate a size for, not null</param>
        /// <returns>the number of bytes</returns>
        public static int CalculateMessageEnvelopeSize(IFudgeTaxonomy taxonomy, FudgeMsgEnvelope envelope)
        {
            return 8 + CalculateMessageSize(taxonomy, envelope.Message);
        }

        /// <summary>
        /// Calculates the size of a message including the envelope header when no taxonomy is used.
        /// 
        /// </summary>
        /// <param Name="envelope"> the message envelope to calculate a size for, not null</param>
        /// <returns>the number of bytes</returns>
        public static int CalculateMessageEnvelopeSize(FudgeMsgEnvelope envelope)
        {
            return 8 + CalculateMessageSize(null, envelope.Message);
        }
    }
}
