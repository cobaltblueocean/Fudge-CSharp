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
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
using Fudge.Taxon;
using Fudge.Wire.Types ;

namespace Fudge
{
    [Serializable]
    public abstract class AbstractFudgeMsg : FudgeMsg, IEnumerable<IFudgeField>
    {
        private FudgeContext _fudgeContext;
        private readonly List<FudgeMsgField> fields = new List<FudgeMsgField>();

        protected AbstractFudgeMsg(FudgeContext fudgeContext)
        {
            if (fudgeContext == null)
            {
                throw new NullReferenceException("Context must be provided.");
            }
            _fudgeContext = fudgeContext;
        }

        /// <summary>
        /// Gets the <see cref="FudgeContext"/> for this message.
        /// </summary>
        public FudgeContext FudgeContext
        {
            get { return _fudgeContext; }
        }

        /// <summary>
        /// Gets the list of fields.
        /// </summary>
        /// <returns></returns>
        protected abstract List<IFudgeField> GetFields();

        protected Boolean fieldNameEquals(String name, IFudgeField field)
        {
            if (name == null)
            {
                return field.Name == null;
            }
            else
            {
                return name.Equals(field.Name);
            }
        }

        protected Boolean fieldOrdinalEquals(int ordinal, IFudgeField field)
        {
            if (ordinal == null)
            {
                return field.Ordinal == null;
            }
            else
            {
                return ordinal.Equals(field.Ordinal);
            }
        }

        protected T getFirstTypedValue<T>(Type clazz, String name, int typeId)
        {
            IFudgeField secondBest = null;
            foreach (IFudgeField field in base.GetAllFields())
            {
                if (fieldNameEquals(name, field))
                {
                    if (field.Type.TypeId == typeId)
                    {
                        return (T)field.Value;  // perfect match
                    }
                    else
                    {
                        if (secondBest == null)
                        {
                            if (FudgeContext.TypeDictionary.canConvertField(clazz, field))
                            {
                                secondBest = field;
                            }
                        }
                    }
                }
            }
            if (secondBest == null)
            {
                throw new NullReferenceException();
            }
            return (T)(Object)FudgeContext.TypeDictionary.GetFieldValue(clazz, secondBest);
        }

        protected T getFirstTypedValue<T>(Type clazz, int ordinal, int typeId)
        {
            IFudgeField secondBest = null;
            foreach (IFudgeField field in base.GetAllFields())
            {
                if (fieldOrdinalEquals(ordinal, field))
                {
                    if (field.Type.TypeId == typeId)
                    {
                        return (T)field.Value;  // perfect match
                    }
                    else
                    {
                        if (secondBest == null)
                        {
                            if (FudgeContext.TypeDictionary.canConvertField(clazz, field))
                            {
                                secondBest = field;
                            }
                        }
                    }
                }
            }
            if (secondBest == null)
            {
                throw new NullReferenceException();
            }
            return (T)(Object)FudgeContext.TypeDictionary.GetFieldValue(clazz, secondBest);
        }

        public void setNamesFromTaxonomy(IFudgeTaxonomy taxonomy)
        {
            if (taxonomy == null)
            {
                return;
            }
            for (int i = 0; i < base.GetAllFields().Count; i++)
            {
                IFudgeField field = base.GetAllFields()[i];
                if (field.Ordinal != null && field.Name == null)
                {
                    String nameFromTaxonomy = taxonomy.GetFieldName((short)field.Ordinal);
                    if (nameFromTaxonomy != null)
                    {
                        field = UnmodifiableFudgeField.of(field.Type, field.Value, field.Ordinal, nameFromTaxonomy);
                        base.GetAllFields()[i] = field;
                    }
                }
                if (field.Value is StandardFudgeMsg)
                {
                    StandardFudgeMsg subMsg = (StandardFudgeMsg)field.Value;
                    subMsg.SetNamesFromTaxonomy(taxonomy);
                }
                else if (field.Value is FudgeMsg)
                {
                    StandardFudgeMsg subMsg = new StandardFudgeMsg(FudgeContext, (FudgeMsg)field.Value);
                    subMsg.SetNamesFromTaxonomy(taxonomy);
                    field = UnmodifiableFudgeField.of(field.Type, subMsg, field.Ordinal, field.Name);
                    base.GetAllFields()[i] = field;
                }
            }
        }


        public new int GetNumFields()
        {
            return GetFields().Count;
        }

        public new Boolean isEmpty()
        {
            return GetNumFields() == 0;
        }

        public new IEnumerable<IFudgeField> GetEnumrator()
        {
            foreach (IFudgeField field in GetFields())
            {
                yield return field;
            }
        }

        public new HashSet<String> GetAllFieldNames()
        {
            HashSet<String> result = new HashSet<String>();
            foreach (IFudgeField field in GetFields())
            {
                if (field.Name != null)
                {
                    result.Add(field.Name);
                }
            }
            return result;
        }

        public new IFudgeField GetByIndex(int index)
        {
            return GetFields()[index];
        }

        public new Boolean HasField(String name)
        {
            return GetByName(name) != null;
        }

        public new List<IFudgeField> GetAllByName(String name)
        {
            List<IFudgeField> fields = new List<IFudgeField>();
            foreach (IFudgeField field in GetFields())
            {
                if (fieldNameEquals(name, field))
                {
                    fields.Add(field);
                }
            }
            return fields;
        }

        public new IFudgeField GetByName(String name)
        {
            foreach (IFudgeField field in GetFields())
            {
                if (fieldNameEquals(name, field))
                {
                    return field;
                }
            }
            return null;
        }

        public new Boolean hasField(int ordinal)
        {
            return GetByOrdinal(ordinal) != null;
        }

        public new List<IFudgeField> GetAllByOrdinal(int ordinal)
        {
            List<IFudgeField> fields = new List<IFudgeField>();
            foreach (IFudgeField field in GetFields())
            {
                if (fieldOrdinalEquals(ordinal, field))
                {
                    fields.Add(field);
                }
            }
            return fields;
        }

        public new IFudgeField GetByOrdinal(int ordinal)
        {
            foreach (IFudgeField field in GetFields())
            {
                if (fieldOrdinalEquals(ordinal, field))
                {
                    return field;
                }
            }
            return null;
        }
        public new Object GetValue(String name)
        {
            IFudgeField field = GetByName(name);
            return (field != null) ? field.Value : null;
        }

        public new Object GetValue(int ordinal)
        {
            IFudgeField field = GetByOrdinal(ordinal);
            return (field != null) ? field.Value : null;
        }


        public new Double GetDouble(String name)
        {
            return (Double)GetFirstTypedValue(name, FudgeWireType.DOUBLE_TYPE_ID);
        }


        public new Double GetDouble(int ordinal)
        {
            return (Double)GetFirstTypedValue(ordinal, FudgeWireType.DOUBLE_TYPE_ID);
        }


        public new float GetFloat(String name)
        {
            return (float)GetFirstTypedValue(name, FudgeWireType.FLOAT_TYPE_ID);
        }


        public new float GetFloat(int ordinal)
        {
            return (float)GetFirstTypedValue(ordinal, FudgeWireType.FLOAT_TYPE_ID);
        }


        public new long GetLong(String name)
        {
            return (long)GetFirstTypedValue(name, FudgeWireType.LONG_TYPE_ID);
        }


        public new long GetLong(int ordinal)
        {
            return (long)GetFirstTypedValue(ordinal, FudgeWireType.LONG_TYPE_ID);
        }


        public new int GetInt(String name)
        {
            return (int)GetFirstTypedValue(name, FudgeWireType.INT_TYPE_ID);
        }


        public new int GetInt(int ordinal)
        {
            return (int)GetFirstTypedValue(ordinal, FudgeWireType.INT_TYPE_ID);
        }


        public new short GetShort(String name)
        {
            return (short)GetFirstTypedValue(name, FudgeWireType.SHORT_TYPE_ID);
        }


        public new short GetShort(int ordinal)
        {
            return (short)GetFirstTypedValue(ordinal, FudgeWireType.SHORT_TYPE_ID);
        }


        public new Byte GetByte(String name)
        {
            return (Byte)GetFirstTypedValue(name, FudgeWireType.BYTE_TYPE_ID);
        }


        public new Byte GetByte(int ordinal)
        {
            return (Byte)GetFirstTypedValue(ordinal, FudgeWireType.BYTE_TYPE_ID);
        }


        public new String GetString(String name)
        {
            return GetFirstTypedValue(name, FudgeWireType.STRING_TYPE_ID).ToString();
        }


        public new String GetString(int ordinal)
        {
            return GetFirstTypedValue(ordinal, FudgeWireType.STRING_TYPE_ID).ToString();
        }


        public new Boolean GetBoolean(String name)
        {
            return (Boolean)GetFirstTypedValue(name, FudgeWireType.BOOLEAN_TYPE_ID);
        }


        public new Boolean GetBoolean(int ordinal)
        {
            return (Boolean)GetFirstTypedValue(ordinal, FudgeWireType.BOOLEAN_TYPE_ID);
        }


        public new FudgeMsg GetMessage(int ordinal)
        {
            return (FudgeMsg)GetFirstTypedValue(ordinal, FudgeWireType.SUB_MESSAGE_TYPE_ID);
        }


        public new FudgeMsg GetMessage(String name)
        {
            return (FudgeMsg)GetFirstTypedValue(name, FudgeWireType.SUB_MESSAGE_TYPE_ID);
        }

        //-------------------------------------------------------------------------

        public new T GetValue<T>(Type clazz, String name)
        {
            FudgeTypeDictionary dictionary = FudgeContext.TypeDictionary;
            foreach (IFudgeField field in GetFields())
            {
                if (fieldNameEquals(name, field) && dictionary.canConvertField(clazz, field))
                {
                    return (T)(Object)dictionary.GetFieldValue(clazz, field);
                }
            }
            return default(T);
        }


        public new T GetValue<T>(Type clazz, int ordinal)
        {
            FudgeTypeDictionary dictionary = FudgeContext.TypeDictionary;
            foreach (IFudgeField field in GetFields())
            {
                if (fieldOrdinalEquals(ordinal, field) && dictionary.canConvertField(clazz, field))
                {
                    return (T)(Object)dictionary.GetFieldValue(clazz, field);
                }
            }
            return default(T);
        }

        /**
         * {@inheritDoc}
         */

        public new T GetFieldValue<T>(Type clazz, IFudgeField field)
        {
            return (T)(Object)FudgeContext.GetFieldValue(clazz, field);
        }

        //-------------------------------------------------------------------------
        /**
         * Checks if this message equals another.
         * <p>
         * The check is performed on the entire list of fields in the message.
         * 
         * @param obj  the object to compare to, null returns false
         * @return true if equal
         */

        public new Boolean equals(Object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (obj is AbstractFudgeMsg)
            {
                AbstractFudgeMsg fm = (AbstractFudgeMsg)obj;
                IEnumerable<IFudgeField> me = (IEnumerable<IFudgeField>)GetEnumerator();
                IEnumerable<IFudgeField> other = (IEnumerable<IFudgeField>)fm.GetEnumerator();
                return me.Equals(other);
            }
            return false;
        }

        /**
         * Gets a suitable hash code.
         * 
         * @return the hash code
         */

        public new int hashCode()
        {
            return GetNumFields();  // poor hash code, but better than nothing
        }

        /**
         * {@inheritDoc}
         */

        public new String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("FudgeMsg[");
            IEnumerator<IFudgeField> iterator = GetEnumerator();
            while (iterator.MoveNext())
            {
                IFudgeField field = iterator.Current;
                if (field.Ordinal != null)
                {
                    sb.Append(field.Ordinal);
                    sb.Append(": ");
                }
                if (field.Name != null)
                {
                    sb.Append(field.Name);
                }
                sb.Append(" => ");
                sb.Append(field.Value);
                sb.Append(", ");
            }
            if (sb.Length > 13)
            {
                sb.Remove(sb.Length - 2, 2);
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
