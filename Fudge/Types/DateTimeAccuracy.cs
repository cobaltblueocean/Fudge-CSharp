using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fudge.Types
{
    public sealed class DateTimeAccuracy
    {
        //The accuracy type code.
        private readonly int _encodedValue;

        #region "Precision"
        /**
        * Millenia precision.
        */
        public static DateTimeAccuracy MILLENIUM = new DateTimeAccuracy(0);
        /**
         * Century precision.
         */
        public static DateTimeAccuracy CENTURY = new DateTimeAccuracy(1);
        /**
         * Year precision.
         */
        public static DateTimeAccuracy YEAR = new DateTimeAccuracy(2);
        /**
         * Month precision. 
         */
        public static DateTimeAccuracy MONTH = new DateTimeAccuracy(3);
        /**
         * Day precision. 
         */
        public static DateTimeAccuracy DAY = new DateTimeAccuracy(4);
        /**
         * Hour precision. 
         */
        public static DateTimeAccuracy HOUR = new DateTimeAccuracy(5);
        /**
         * Minute precision.
         */
        public static DateTimeAccuracy MINUTE = new DateTimeAccuracy(6);
        /**
         * Second precision.
         */
        public static DateTimeAccuracy SECOND = new DateTimeAccuracy(7);
        /**
         * Millisecond precision.
         */
        public static DateTimeAccuracy MILLISECOND = new DateTimeAccuracy(8);
        /**
         * Microsecond precision.
         */
        public static DateTimeAccuracy MICROSECOND = new DateTimeAccuracy(9);
        /**
         * Nanosecond precision.
         */
        public static DateTimeAccuracy NANOSECOND = new DateTimeAccuracy(10);

        #endregion

        /// <summary>
        /// Restricted constructor.
        /// </summary>
        /// <param name="encodedValue"></param>
        private DateTimeAccuracy(int encodedValue)
        {
            _encodedValue = encodedValue;
        }

        /// <summary>
        /// Converts the enum to the Fudge wire value for date and time.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/DateTime+encoding">DateTime encoding</a>
        /// </summary>
        /// <returns>the numeric value</returns>
        public int GetEncodedValue()
        {
            return _encodedValue;
        }

        /// <summary>
        /// Converts the Fudge wire value to the enum for date and time.
        /// See <a href="http://wiki.fudgemsg.org/display/FDG/DateTime+encoding">DateTime encoding</a>
        /// </summary>
        /// <param name="value">numeric value</param>
        /// <returns>the DateTimeAccuracy, null if the value is invalid</returns>
        public static DateTimeAccuracy FromEncodedValue(int value)
        {
            switch (value)
            {
                case 10: return NANOSECOND;
                case 9: return MICROSECOND;
                case 8: return MILLISECOND;
                case 7: return SECOND;
                case 6: return MINUTE;
                case 5: return HOUR;
                case 4: return DAY;
                case 3: return MONTH;
                case 2: return YEAR;
                case 1: return CENTURY;
                case 0: return MILLENIUM;
                default: return null;
            }
        }

        /// <summary>
        /// Tests if this accuracy is a greater precision than another.
        /// For example, SECOND precision is greater than MINUTE precision.
        /// </summary>
        /// <param name="accuracy">the other accuracy, not null</param>
        /// <returns>true if greater, false otherwise</returns>
        public Boolean GreaterThan(DateTimeAccuracy accuracy)
        {
            return GetEncodedValue() > accuracy.GetEncodedValue();
        }

        /// <summary>
        /// Tests is this accuracy is a lower precision than another
        /// For example, MINUTE precision is less than SECOND precision.
        /// </summary>
        /// <param name="accuracy">accuracy  the other accuracy, not null</param>
        /// <returns>true if lower, false otherwise</returns>
        public Boolean LessThan(DateTimeAccuracy accuracy)
        {
            return GetEncodedValue() < accuracy.GetEncodedValue();
        }
    }
}
