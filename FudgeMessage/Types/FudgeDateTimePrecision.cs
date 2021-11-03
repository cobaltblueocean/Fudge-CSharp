using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FudgeMessage.Types
{
    /// <summary>
    /// <c>FudgeDateTimePrecision</c> expresses the resolution of a <see cref="FudgeDateTime"/> or <see cref="FudgeTime"/> object.
    /// </summary>
    /// <remarks>
    /// The values are as defined in the specification at http://www.fudgemsg.org/display/FDG/DateTime+encoding
    /// </remarks>
    public sealed class FudgeDateTimePrecision
    {
        //The accuracy type code.
        private readonly byte _encodedValue;

        #region "Precision"
        /**
        * Millenia precision.
        */
        public static FudgeDateTimePrecision Millennium = new FudgeDateTimePrecision(0);
        /**
         * Century precision.
         */
        public static FudgeDateTimePrecision Century = new FudgeDateTimePrecision(1);
        /**
         * Year precision.
         */
        public static FudgeDateTimePrecision Year = new FudgeDateTimePrecision(2);
        /**
         * Month precision. 
         */
        public static FudgeDateTimePrecision Month = new FudgeDateTimePrecision(3);
        /**
         * Day precision. 
         */
        public static FudgeDateTimePrecision Day = new FudgeDateTimePrecision(4);
        /**
         * Hour precision. 
         */
        public static FudgeDateTimePrecision Hour = new FudgeDateTimePrecision(5);
        /**
         * Minute precision.
         */
        public static FudgeDateTimePrecision Minute = new FudgeDateTimePrecision(6);
        /**
         * Second precision.
         */
        public static FudgeDateTimePrecision Second = new FudgeDateTimePrecision(7);
        /**
         * Millisecond precision.
         */
        public static FudgeDateTimePrecision Millisecond = new FudgeDateTimePrecision(8);
        /**
         * Microsecond precision.
         */
        public static FudgeDateTimePrecision Microsecond = new FudgeDateTimePrecision(9);
        /**
         * Nanosecond precision.
         */
        public static FudgeDateTimePrecision Nanosecond = new FudgeDateTimePrecision(10);

        #endregion

        /// <summary>
        /// Restricted constructor.
        /// </summary>
        /// <param name="encodedValue"></param>
        private FudgeDateTimePrecision(byte encodedValue)
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
        /// <returns>the FudgeDateTimePrecision, null if the value is invalid</returns>
        public static FudgeDateTimePrecision FromEncodedValue(int value)
        {
            switch (value)
            {
                case 10: return Nanosecond;
                case 9: return Microsecond;
                case 8: return Millisecond;
                case 7: return Second;
                case 6: return Minute;
                case 5: return Hour;
                case 4: return Day;
                case 3: return Month;
                case 2: return Year;
                case 1: return Century;
                case 0: return Millennium;
                default: return null;
            }
        }

        /// <summary>
        /// Tests if this accuracy is a greater precision than another.
        /// For example, SECOND precision is greater than MINUTE precision.
        /// </summary>
        /// <param name="accuracy">the other accuracy, not null</param>
        /// <returns>true if greater, false otherwise</returns>
        public Boolean GreaterThan(FudgeDateTimePrecision accuracy)
        {
            return GetEncodedValue() > accuracy.GetEncodedValue();
        }

        /// <summary>
        /// Tests is this accuracy is a lower precision than another
        /// For example, MINUTE precision is less than SECOND precision.
        /// </summary>
        /// <param name="accuracy">accuracy  the other accuracy, not null</param>
        /// <returns>true if lower, false otherwise</returns>
        public Boolean LessThan(FudgeDateTimePrecision accuracy)
        {
            return GetEncodedValue() < accuracy.GetEncodedValue();
        }
    }
}
