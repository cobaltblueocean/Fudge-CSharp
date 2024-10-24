﻿/* <!--
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
    public class FudgeTimeTest
    {
        [Test]
        public void BasicConstruction()
        {
            var t = new FudgeTime(13, 4, 5, 123456789, -120, FudgeDateTimePrecision.Nanosecond);
            Assert2.AreEqual(13, t.Hour);
            Assert2.AreEqual(4, t.Minute);
            Assert2.AreEqual(5, t.Second);
            Assert2.AreEqual(123456789, t.Nanoseconds);
            Assert2.AreEqual(-120, t.TimeZoneOffset);
            Assert2.AreEqual(FudgeDateTimePrecision.Nanosecond, t.Precision);
            Assert2.AreEqual("13:04:05.123456789-02:00", t.ToString());

            var t2 = new FudgeTime(1, 2, 3);
            Assert2.Null(t2.TimeZoneOffset);
            Assert2.AreEqual(FudgeDateTimePrecision.Second, t2.Precision);
            Assert2.AreEqual("01:02:03", t2.ToString());

            var t3 = new FudgeTime(1, 2, 3, 60);
            Assert2.AreEqual(60, t3.TimeZoneOffset);
            Assert2.AreEqual(FudgeDateTimePrecision.Second, t3.Precision);
            Assert2.AreEqual("01:02:03+01:00", t3.ToString());

            // Other variants
            Assert2.AreEqual("04:01", new FudgeTime(4, 1).ToString());
            Assert2.AreEqual("23", new FudgeTime(23).ToString());
            Assert2.AreEqual("10:00:05.987654321", new FudgeTime(10, 0, 5, 987654321, FudgeDateTimePrecision.Nanosecond).ToString());
            Assert2.AreEqual("01:01:01.000000123", new FudgeTime(FudgeDateTimePrecision.Nanosecond, 3661, 123).ToString());
            Assert2.AreEqual("01:01:01.000000123+00:30", new FudgeTime(FudgeDateTimePrecision.Nanosecond, 3661, 123, 30).ToString());
        }

        [Test]
        public void ConstructingFromDateTime()
        {
            Assert2.AreEqual("12:14:10.000000000", new FudgeTime(new DateTime(2000, 2, 3, 12, 14, 10, DateTimeKind.Unspecified)).ToString());
            Assert2.AreEqual("12:14:10.000000000+00:00", new FudgeTime(new DateTime(2000, 2, 3, 12, 14, 10, DateTimeKind.Utc)).ToString());

            var local = new FudgeTime(new DateTime(2000, 7, 6, 12, 14, 10, DateTimeKind.Local));
            Assert2.True(local.ToString().StartsWith("12:14:10"));
            Assert2.True(local.TimeZoneOffset.HasValue);
        }

        [Test]
        public void RangeChecking()
        {
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(-1));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(24));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(1, -1));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(1, -1));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(1, 5, -1));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(1, 5, 60));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(1, 5, 13, -1, FudgeDateTimePrecision.Nanosecond));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(1, 5, 13, 1000000000, FudgeDateTimePrecision.Nanosecond));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(1, 5, 23, 13));      // Must be a multiple of 15
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(1, 5, 23, -1920));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(1, 5, 23, 1920));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(1, 5, 23, 123, FudgeDateTimePrecision.Day));

            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(FudgeDateTimePrecision.Nanosecond, -1, 0));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(FudgeDateTimePrecision.Nanosecond, 24 * 60 * 60, 0));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(FudgeDateTimePrecision.Nanosecond, 0, -1));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(FudgeDateTimePrecision.Nanosecond, 0, 1000000000));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(FudgeDateTimePrecision.Nanosecond, 0, 0, 3));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(FudgeDateTimePrecision.Nanosecond, 0, 0, -4500));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(FudgeDateTimePrecision.Nanosecond, 0, 0, 4500));
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(FudgeDateTimePrecision.Month, 0, 0));

            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeTime(DateTime.Now, FudgeDateTimePrecision.Month));
        }

        [Test]
        public void Totals()
        {
            var t = new FudgeTime(1, 2, 3, 123456789, FudgeDateTimePrecision.Nanosecond);
            Assert2.AreEqual(3723, t.TotalSeconds);
            Assert2.AreEqual(3723123456789L, t.TotalNanoseconds);
        }

        [Test]
        public void StringFormats()
        {
            Assert2.AreEqual("10:00:05.987654321", new FudgeTime(10, 0, 5, 987654321, FudgeDateTimePrecision.Nanosecond).ToString());
            Assert2.AreEqual("10:00:05.987654", new FudgeTime(10, 0, 5, 987654321, FudgeDateTimePrecision.Microsecond).ToString());
            Assert2.AreEqual("10:00:05.987", new FudgeTime(10, 0, 5, 987654321, FudgeDateTimePrecision.Millisecond).ToString());
            Assert2.AreEqual("10:00:05", new FudgeTime(10, 0, 5).ToString());
            Assert2.AreEqual("10:00", new FudgeTime(10, 0).ToString());
            Assert2.AreEqual("10", new FudgeTime(10).ToString());

            Assert2.AreEqual("10:00:05-01:15", new FudgeTime(10, 0, 5, -75).ToString());
            Assert2.AreEqual("10:00:05+00:00", new FudgeTime(10, 0, 5, 0).ToString());
            Assert2.AreEqual("10:00:05+04:00", new FudgeTime(10, 0, 5, 240).ToString());
        }

        [Test]
        public void ObjectOverrides()
        {
            Assert2.AreEqual(new FudgeTime(1, 2, 3).GetHashCode(), new FudgeTime(1, 2, 3).GetHashCode());
            Assert2.AreNotEqual(new FudgeTime(1, 2, 3).GetHashCode(), new FudgeTime(1, 2, 4).GetHashCode());
            Assert2.AreNotEqual(new FudgeTime(1, 2, 3, 1234, FudgeDateTimePrecision.Nanosecond).GetHashCode(),
                            new FudgeTime(1, 2, 3, 1235, FudgeDateTimePrecision.Nanosecond).GetHashCode());
            Assert2.AreNotEqual(new FudgeTime(1, 2, 3, 60).GetHashCode(), new FudgeTime(1, 2, 3, 45).GetHashCode());

            Assert2.False(new FudgeTime(1, 2, 3).Equals(null));
            Assert2.False(new FudgeTime(1, 2, 3).Equals("fred"));
            Assert2.True(new FudgeTime(1, 2, 3, 45789, 60, FudgeDateTimePrecision.Nanosecond).Equals(new FudgeTime(1, 2, 3, 45789, 60, FudgeDateTimePrecision.Nanosecond)));
            Assert2.False(new FudgeTime(1, 2, 4, 45789, 60, FudgeDateTimePrecision.Nanosecond).Equals(new FudgeTime(1, 2, 3, 45789, 60, FudgeDateTimePrecision.Nanosecond)));
            Assert2.False(new FudgeTime(1, 2, 3, 45780, 60, FudgeDateTimePrecision.Nanosecond).Equals(new FudgeTime(1, 2, 3, 45789, 60, FudgeDateTimePrecision.Nanosecond)));
            Assert2.False(new FudgeTime(1, 2, 3, 45789, 45, FudgeDateTimePrecision.Nanosecond).Equals(new FudgeTime(1, 2, 3, 45789, 60, FudgeDateTimePrecision.Nanosecond)));
            Assert2.False(new FudgeTime(1, 2, 3, 45789, 60, FudgeDateTimePrecision.Millisecond).Equals(new FudgeTime(1, 2, 3, 45789, 60, FudgeDateTimePrecision.Nanosecond)));
            Assert2.False(new FudgeTime(1, 2, 3, 45789, FudgeDateTimePrecision.Nanosecond).Equals(new FudgeTime(1, 2, 3, 45789, 0, FudgeDateTimePrecision.Nanosecond)));
        }
    }
}
