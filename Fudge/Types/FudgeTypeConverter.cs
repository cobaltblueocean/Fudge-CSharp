using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Fudge.Types
{
    public class FudgeTypeConverter : TypeConverter
    {
        private int _rangeLo;
        private int _rangeHi;

        public Boolean canConvertPrimary(Type clazz)
        {
            if (typeof(Boolean).IsAssignableFrom(clazz))
                return true;
            if (typeof(sbyte).IsAssignableFrom(clazz))
                return true;
            if (typeof(byte).IsAssignableFrom(clazz))
                return true;
            if (typeof(short).IsAssignableFrom(clazz))
                return true;
            if (typeof(ushort).IsAssignableFrom(clazz))
                return true;
            if (typeof(int).IsAssignableFrom(clazz))
                return true;
            if (typeof(uint).IsAssignableFrom(clazz))
                return true;
            if (typeof(long).IsAssignableFrom(clazz))
                return true;
            if (typeof(ulong).IsAssignableFrom(clazz))
                return true;
            if (typeof(float).IsAssignableFrom(clazz))
                return true;
            if (typeof(double).IsAssignableFrom(clazz))
                return true;
            if (typeof(decimal).IsAssignableFrom(clazz))
                return true;
            if (typeof(String).IsAssignableFrom(clazz))
                return true;
            return false;
        }

        public dynamic primaryToSecondary(Object value)
        {

            if (typeof(Boolean).IsAssignableFrom(value.GetType()))
                return primaryToSecondaryBoolean(value);
            if (typeof(sbyte).IsAssignableFrom(value.GetType()))
                return (sbyte)primaryToSecondaryByte(value);
            if (typeof(byte).IsAssignableFrom(value.GetType()))
                return primaryToSecondaryByte(value);
            if (typeof(short).IsAssignableFrom(value.GetType()))
                return primaryToSecondaryShort(value);
            if (typeof(ushort).IsAssignableFrom(value.GetType()))
                return (ushort)primaryToSecondaryShort(value);
            if (typeof(int).IsAssignableFrom(value.GetType()))
                return primaryToSecondaryInt(value);
            if (typeof(uint).IsAssignableFrom(value.GetType()))
                return (uint)primaryToSecondaryInt(value);
            if (typeof(long).IsAssignableFrom(value.GetType()))
                return primaryToSecondaryLong(value);
            if (typeof(ulong).IsAssignableFrom(value.GetType()))
                return (ulong)primaryToSecondaryLong(value);
            if (typeof(float).IsAssignableFrom(value.GetType()))
                return primaryToSecondaryFloat(value);
            if (typeof(double).IsAssignableFrom(value.GetType()))
                return primaryToSecondaryDouble(value);
            if (typeof(decimal).IsAssignableFrom(value.GetType()))
                return (decimal)Convert.ChangeType(value, typeof(decimal));
            if (typeof(String).IsAssignableFrom(value.GetType()))
                return value.ToString();
            return value;
        }

        private Boolean primaryToSecondaryBoolean(Object value)
        {
            if (value is Boolean)
                return (Boolean)value;
            if (value is Byte)
                return ((Byte)value != 0);
            if (value is short)
                return ((short)value != 0);
            if (value is int)
                return ((int)value != 0);
            if (value is long)
                return ((long)value != 0);
            if (value is float)
                return ((float)value != 0);
            if (value is Double)
                return ((Double)value != 0);
            if (value is String)
                return Boolean.Parse((String)value);
            else
                return (Boolean)Convert.ChangeType(value, typeof(Boolean));
        }

        private Byte primaryToSecondaryByte(Object value)
        {
            if (value is Boolean)
                return (byte)(((Boolean)value) ? 1 : 0);
            if (value is Byte)
                return (Byte)value;
            if (value is short)
                return (byte)rangeCheck((short)value);
            if (value is int)
                return (byte)rangeCheck((int)value);
            if (value is long)
                return (byte)rangeCheck((long)value);
            if (value is float)
                return (byte)rangeCheck((float)value);
            if (value is Double)
                return (byte)rangeCheck((Double)value);
            if (value is String)
                return Byte.Parse((String)value);
            else
                return (Byte)Convert.ChangeType(value, typeof(Byte));
        }

        private short primaryToSecondaryShort(Object value)
        {
            if (value is Boolean)
                return (short)(((Boolean)value) ? 1 : 0);
            if (value is Byte)
                return (short)(Byte)value;
            if (value is short)
                return (short)value;
            if (value is int)
                return (short)rangeCheck((int)value);
            if (value is long)
                return (short)rangeCheck((long)value);
            if (value is float)
                return (short)rangeCheck((float)value);
            if (value is Double)
                return (short)rangeCheck((Double)value);
            if (value is String)
                return short.Parse((String)value);
            else
                return (short)Convert.ChangeType(value, typeof(short));
        }

        private int primaryToSecondaryInt(Object value)
        {
            if (value is Boolean)
                return (int)(((Boolean)value) ? 1 : 0);
            if (value is Byte)
                return (int)(Byte)value;
            if (value is short)
                return (int)(short)value;
            if (value is int)
                return (int)value;
            if (value is long)
                return (int)rangeCheck((long)value);
            if (value is float)
                return (int)rangeCheck((float)value);
            if (value is Double)
                return (int)rangeCheck((Double)value);
            if (value is String)
                return int.Parse((String)value);
            else
                return (int)Convert.ChangeType(value, typeof(int));
        }

        private long primaryToSecondaryLong(Object value)
        {
            if (value is Boolean)
                return (long)(((Boolean)value) ? 1 : 0);
            if (value is float)
                return (long)rangeCheck(long.MinValue, long.MaxValue, (float)value);
            if (value is Double)
                return (long)rangeCheck(long.MinValue, long.MaxValue, (Double)value);
            if (IsNumber(value))
                return (long)Convert.ChangeType(value, typeof(long));
            if (value is String)
                return long.Parse((String)value);
            else
                return (long)Convert.ChangeType(value, typeof(long));
        }

        private float primaryToSecondaryFloat(Object value)
        {
            if (value is Boolean)
                return (float)(((Boolean)value) ? 1 : 0);
            if (IsNumber(value))
                return (float)Convert.ChangeType(value, typeof(float));
            if (value is String)
                return float.Parse((String)value);
            else
                return (float)Convert.ChangeType(value, typeof(float));
        }

        private Double primaryToSecondaryDouble(Object value)
        {
            if (value is Boolean)
                return (double)(((Boolean)value) ? 1 : 0);
            if (IsNumber(value))
                return (Double)Convert.ChangeType(value, typeof(Double));
            if (value is String)
                return Double.Parse((String)value);
            else
                return (Double)Convert.ChangeType(value, typeof(Double));
        }

        protected int rangeCheck(int value)
        {
            if ((value >= int.MinValue) && (value <= int.MaxValue))
                return value;
            throw new ArgumentException("value " + value + " out of range for " + value.GetType().FullName);
        }

        protected long rangeCheck(long value)
        {
            if ((value >= long.MinValue) && (value <= long.MaxValue))
                return value;
            throw new ArgumentException("value " + value + " out of range for " + value.GetType().FullName);
        }

        protected double rangeCheck(double value)
        {
            return rangeCheck(Double.MinValue, Double.MaxValue, value);
        }

        protected double rangeCheck(double lo, double hi, double value)
        {
            if ((value >= lo) && (value <= hi))
                return value;
            throw new ArgumentException("value " + value + " out of range for " + value.GetType().FullName);
        }

        protected static bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }
    }
}
