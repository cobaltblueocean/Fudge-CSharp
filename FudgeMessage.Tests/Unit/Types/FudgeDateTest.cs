/* <!--
 * Copyright (C) 2009 - 2010 by OpenGamma Inc. and other contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * -->
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FudgeMessage.Types;
using Mercury.Test.Utility;

namespace FudgeMessage.Tests.Unit.Types
{
    [Parallelizable(ParallelScope.ContextMask)]
    public class FudgeDateTest
    {
        [Test]
        public void VariousConstructors()
        {
            Assert2.AreEqual(20010304, new FudgeDate(20010304).RawValue);
            Assert2.AreEqual(19991012, new FudgeDate(1999, 10, 12).RawValue);
            Assert2.AreEqual(-19991012, new FudgeDate(-1999, 10, 12).RawValue);

            var dateTime = new DateTime(2000, 1, 2);
            Assert2.AreEqual(20000102, new FudgeDate(dateTime).RawValue);
        }

        [Test]
        public void ConstructorRangeChecking()
        {
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeDate(10000000, 1, 1));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeDate(1000, -1, 1));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeDate(1000, 13, 1));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeDate(1000, 1, -1));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeDate(1000, 1, 32));
        }

        [Test]
        public void YearMonthDay()
        {
            var date = new FudgeDate(19701211);
            Assert2.AreEqual(1970, date.Year);
            Assert2.AreEqual(12, date.Month);
            Assert2.AreEqual(11, date.Day);

            var date2 = new FudgeDate(-12340102);
            Assert2.AreEqual(-1234, date2.Year);
            Assert2.AreEqual(1, date2.Month);
            Assert2.AreEqual(2, date2.Day);
        }

        [Test]
        public void IsValid()
        {
            Assert2.True(new FudgeDate(20010305).IsValid);

            Assert2.False(new FudgeDate(20010100).IsValid);
            Assert2.False(new FudgeDate(20010132).IsValid);
            Assert2.False(new FudgeDate(20010010).IsValid);
            Assert2.False(new FudgeDate(20011310).IsValid);
            Assert2.False(new FudgeDate(00000101).IsValid);
            Assert2.False(new FudgeDate(-00000101).IsValid);
            
            Assert2.False(new FudgeDate(20010230).IsValid);

            // Leap years
            Assert2.True(new FudgeDate(20000229).IsValid);
            Assert2.False(new FudgeDate(19990229).IsValid);
            Assert2.False(new FudgeDate(19000229).IsValid);

            // Negatives
            Assert2.True(new FudgeDate(-00010229).IsValid);      // 1BC is the year before 1AD as there was no zero, so it's leap
            Assert2.True(new FudgeDate(-04010229).IsValid);
            Assert2.False(new FudgeDate(-01010229).IsValid);
        }

        [Test]
        public void RollToValidDate()
        {
            Assert2.AreEqual("1999-03-01", new FudgeDate(19990229).RollToValidDate().ToString());
            Assert2.AreEqual("2000-01-01", new FudgeDate(19991232).RollToValidDate().ToString());

            Assert2.AreEqual("0001-01-01", new FudgeDate(00000101).RollToValidDate().ToString());
            Assert2.AreEqual("0001-01-01", new FudgeDate(-00000101).RollToValidDate().ToString());
            Assert2.AreEqual("0001-01-01", new FudgeDate(-00011232).RollToValidDate().ToString());

            Assert2.AreEqual("-0099-01-01", new FudgeDate(-01001232).RollToValidDate().ToString());
        }

        [Test]
        public void ToDateTime()
        {
            var dateTime = new FudgeDate(20030104).ToDateTime();
            Assert2.AreEqual("2003-01-04T00:00:00.0000000", dateTime.ToString("o"));
            Assert2.AreEqual(DateTimeKind.Unspecified, dateTime.Kind);

            // Check rolls to next valid day
            var dateTime2 = new FudgeDate(20030230).ToDateTime();
            Assert2.AreEqual("2003-03-01T00:00:00.0000000", dateTime2.ToString("o"));
        }

        [Test]
        public void VariousToString()
        {
            Assert2.AreEqual("1000-01-01", new FudgeDate(10000101).ToString());
            Assert2.AreEqual("0010-01-01", new FudgeDate(00100101).ToString());
            Assert2.AreEqual("-1000-01-01", new FudgeDate(-10000101).ToString());

            Assert2.AreEqual("1001-01-01", new FudgeDate(10010101).ToString(FudgeDateTimePrecision.Day));
            Assert2.AreEqual("1001-01", new FudgeDate(10010101).ToString(FudgeDateTimePrecision.Month));
            Assert2.AreEqual("1001", new FudgeDate(10010101).ToString(FudgeDateTimePrecision.Year));
            Assert2.AreEqual("1000", new FudgeDate(10010101).ToString(FudgeDateTimePrecision.Century));    // REVIEW 2010-01-10 t0rx -- Is this the right behaviour for centuries?
        }

        [Test]
        public void Comparison()
        {
            Assert2.AreEqual(-1, new FudgeDate(19990102).CompareTo(new FudgeDate(20000102)));
            Assert2.AreEqual(-1, new FudgeDate(19990102).CompareTo(new FudgeDate(19990202)));
            Assert2.AreEqual(-1, new FudgeDate(19990102).CompareTo(new FudgeDate(19990103)));
            Assert2.AreEqual(0, new FudgeDate(19990102).CompareTo(new FudgeDate(19990102)));
            Assert2.AreEqual(1, new FudgeDate(20000102).CompareTo(new FudgeDate(19990102)));
            Assert2.AreEqual(1, new FudgeDate(19990202).CompareTo(new FudgeDate(19990102)));
            Assert2.AreEqual(1, new FudgeDate(19990103).CompareTo(new FudgeDate(19990102)));

            Assert2.AreEqual(-1, new FudgeDate(-00010101).CompareTo(new FudgeDate(00010101)));

            Assert2.AreEqual(-1, new FudgeDate(-19990102).CompareTo(new FudgeDate(-19990103)));
            Assert2.AreEqual(-1, new FudgeDate(-19990102).CompareTo(new FudgeDate(-19990202)));
            Assert2.AreEqual(1, new FudgeDate(-19990102).CompareTo(new FudgeDate(-20000102)));  // -1999 was after -2000
            Assert2.AreEqual(0, new FudgeDate(-19990102).CompareTo(new FudgeDate(-19990102)));
            Assert2.AreEqual(1, new FudgeDate(-19990103).CompareTo(new FudgeDate(-19990102)));
            Assert2.AreEqual(1, new FudgeDate(-19990202).CompareTo(new FudgeDate(-19990102)));
            Assert2.AreEqual(-1, new FudgeDate(-20000102).CompareTo(new FudgeDate(-19990102)));  // -1999 was after -2000
        }

        [Test]
        public void ObjectOverrides()
        {
            Assert2.True(new FudgeDate(12340310).Equals(new FudgeDate(12340310)));
            Assert2.False(new FudgeDate(12340310).Equals(new FudgeDate(19990407)));
            Assert2.False(new FudgeDate(12340310).Equals(null));
            Assert2.False(new FudgeDate(12340310).Equals("Fred"));

            Assert2.AreEqual(new FudgeDate(12340310).GetHashCode(), new FudgeDate(12340310).GetHashCode());
            Assert2.AreNotEqual(new FudgeDate(12340310).GetHashCode(), new FudgeDate(12340311).GetHashCode());

            Assert2.AreEqual("1234-10-01", new FudgeDate(12341001).ToString());
            Assert2.AreEqual("0001-10-01", new FudgeDate(00011001).ToString());
            Assert2.AreEqual("-1234-10-01", new FudgeDate(-12341001).ToString());
            Assert2.AreEqual("-0001-10-01", new FudgeDate(-00011001).ToString());
        }

        [Test]
        public void IConvertible()
        {
            var date = new FudgeDate(19870304);
            Assert2.AreEqual(TypeCode.Object, date.GetTypeCode());
            Assert2.ThrowsException<InvalidCastException>(() => date.ToBoolean(null));
            Assert2.ThrowsException<InvalidCastException>(() => date.ToByte(null));
            Assert2.ThrowsException<InvalidCastException>(() => date.ToChar(null));
            Assert2.AreEqual(new DateTime (1987, 3, 4), date.ToDateTime(null));
            Assert2.ThrowsException<InvalidCastException>(() => date.ToDecimal(null));
            Assert2.ThrowsException<InvalidCastException>(() => date.ToDouble(null));
            Assert2.ThrowsException<InvalidCastException>(() => date.ToInt16(null));
            Assert2.ThrowsException<InvalidCastException>(() => date.ToInt32(null));
            Assert2.ThrowsException<InvalidCastException>(() => date.ToInt64(null));
            Assert2.ThrowsException<InvalidCastException>(() => date.ToSByte(null));
            Assert2.ThrowsException<InvalidCastException>(() => date.ToSingle(null));
            Assert2.AreEqual("1987-03-04", date.ToString());
            Assert2.ThrowsException<InvalidCastException>(() => date.ToUInt16(null));
            Assert2.ThrowsException<InvalidCastException>(() => date.ToUInt32(null));
            Assert2.ThrowsException<InvalidCastException>(() => date.ToUInt64(null));
            Assert2.AreEqual(new DateTime(1987, 3, 4), date.ToType(typeof(DateTime), null));
            Assert2.AreEqual(new DateTimeOffset(1987, 3, 4, 0, 0, 0, TimeSpan.Zero), date.ToType(typeof(DateTimeOffset), null));
            Assert2.AreEqual(new FudgeDateTime(1987, 3, 4, 0, 0, 0, 0, FudgeDateTimePrecision.Day), date.ToType(typeof(FudgeDateTime), null));
            Assert2.AreEqual("1987-03-04", date.ToType(typeof(string), null));
            Assert2.ThrowsException<InvalidCastException>(() => date.ToType(typeof(int), null));
        }
    }
}
