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
    public class FudgeDateTimeTest
    {
        [Test]
        public void VariousConstructors()
        {
            var dt = new DateTime(1978, 4, 3, 1, 17, 34, DateTimeKind.Unspecified);
            Assert2.AreEqual("1978-04-03T01:17:34.000000000", new FudgeDateTime(dt).ToString());
            Assert2.AreEqual("1978-04-03T01:17", new FudgeDateTime(dt, FudgeDateTimePrecision.Minute).ToString());
            Assert2.AreEqual("1978-04", new FudgeDateTime(dt, FudgeDateTimePrecision.Month).ToString());

            Assert2.AreEqual("1978-04-03T01:17:34.123456789", new FudgeDateTime(1978, 4, 3, 1, 17, 34, 123456789, FudgeDateTimePrecision.Nanosecond).ToString());
            Assert2.AreEqual("1978-04-03", new FudgeDateTime(1978, 4, 3, 1, 17, 34, 123456789, FudgeDateTimePrecision.Day).ToString());
            Assert2.AreEqual("1978-04-03T01:17:34.123456789-01:30", new FudgeDateTime(1978, 4, 3, 1, 17, 34, 123456789, -90, FudgeDateTimePrecision.Nanosecond).ToString());
            Assert2.AreEqual("1978-04-03", new FudgeDateTime(1978, 4, 3, 1, 17, 34, 123456789, -90, FudgeDateTimePrecision.Day).ToString());

            Assert2.AreEqual("1978-04-03", new FudgeDateTime(new FudgeDate(19780403), null).ToString());
            Assert2.AreEqual("1978-04-03T01:17:34", new FudgeDateTime(new FudgeDate(19780403), new FudgeTime(1, 17, 34)).ToString());
            Assert2.AreEqual(FudgeDateTimePrecision.Second, new FudgeDateTime(new FudgeDate(19780403), new FudgeTime(1, 17, 34)).Precision);    // Make sure it picks up the precision from the time

            var dto = new DateTimeOffset(1234, 5, 6, 7, 8, 9, new TimeSpan(2, 30, 0));
            Assert2.AreEqual("1234-05-06T07:08:09.000000000+02:30", new FudgeDateTime(dto).ToString());
            Assert2.AreEqual("1234-05-06T07:08+02:30", new FudgeDateTime(dto, FudgeDateTimePrecision.Minute).ToString());
            Assert2.AreEqual("1234-05-06", new FudgeDateTime(dto, FudgeDateTimePrecision.Day).ToString());
        }

        [Test]
        public void Properties()
        {
            var dt = new FudgeDateTime(1234, 5, 6, 7, 8, 9, 123456789, -60, FudgeDateTimePrecision.Nanosecond);
            Assert2.AreEqual(12340506, dt.Date.RawValue);
            Assert2.AreEqual("07:08:09.123456789-01:00", dt.Time.ToString());
            Assert2.AreEqual(FudgeDateTimePrecision.Nanosecond, dt.Precision);
            Assert2.AreEqual(1234, dt.Year);
            Assert2.AreEqual(5, dt.Month);
            Assert2.AreEqual(6, dt.Day);
            Assert2.AreEqual(7, dt.Hour);
            Assert2.AreEqual(8, dt.Minute);
            Assert2.AreEqual(9, dt.Second);
            Assert2.AreEqual(123456789, dt.Nanoseconds);
            Assert2.AreEqual(-60, dt.TimeZoneOffset);
            Assert2.True(dt.IsValidDate);
        }

        [Test]
        public void ConvertingToNativeTypes()
        {
            Assert2.ThrowsException<ArgumentOutOfRangeException>(() => new FudgeDateTime(new FudgeDate(-12300101), null).ToDateTime());

            DateTime dt1 = new FudgeDateTime(1900, 2, 28, 12, 7, 34, 0, FudgeDateTimePrecision.Nanosecond).ToDateTime();
            Assert2.AreEqual("1900-02-28T12:07:34.0000000", dt1.ToString("o"));
            Assert2.AreEqual(DateTimeKind.Unspecified, dt1.Kind);

            DateTime dt2 = new FudgeDateTime(1900, 2, 28, 12, 7, 34, 0, 60, FudgeDateTimePrecision.Nanosecond).ToDateTime();
            Assert2.AreEqual("1900-02-28T11:07:34.0000000Z", dt2.ToString("o"));      // Back an hour
            Assert2.AreEqual(DateTimeKind.Utc, dt2.Kind);

            DateTime dt3 = new FudgeDateTime(2009, 7, 13, 12, 0, 0, 0, FudgeDateTimePrecision.Nanosecond).ToDateTime(DateTimeKind.Local, true);
            Assert2.AreEqual(DateTimeKind.Local, dt3.Kind);
            Assert2.AreEqual(new DateTime(2009, 7, 13, 12, 0, 0, DateTimeKind.Utc).ToLocalTime(), dt3);

            DateTime dt4 = new FudgeDateTime(1900, 2, 28, 12, 7, 34, 0, 60, FudgeDateTimePrecision.Nanosecond).ToDateTime(DateTimeKind.Unspecified, false);
            Assert2.AreEqual("1900-02-28T12:07:34.0000000", dt4.ToString("o"));         // Ignore the offset because we're going to unspecified
            Assert2.AreEqual(DateTimeKind.Unspecified, dt4.Kind);

            DateTimeOffset dto = new FudgeDateTime(1900, 2, 28, 12, 7, 34, 0, 60, FudgeDateTimePrecision.Nanosecond).ToDateTimeOffset();
            Assert2.AreEqual("1900-02-28T12:07:34.0000000+01:00", dto.ToString("o"));
        }

        [Test]
        public void ObjectOverrides()
        {
            // ToString
            var dt = new FudgeDateTime(2003, 11, 13, 12, 14, 34, 987654321, 0, FudgeDateTimePrecision.Nanosecond);
            Assert2.AreEqual("2003-11-13T12:14:34.987654321+00:00", dt.ToString());

            // Equals
            var dt2 = new FudgeDateTime(2003, 11, 13, 12, 14, 34, 987654321, 0, FudgeDateTimePrecision.Nanosecond);
            var dt3 = new FudgeDateTime(2003, 11, 13, 12, 14, 34, 987654321, 0, FudgeDateTimePrecision.Nanosecond);
            Assert2.AreEqual(dt2, dt3);
            Assert2.False(dt2.Equals(null));
            Assert2.False(dt2.Equals("Fred"));
        }
    }
}
