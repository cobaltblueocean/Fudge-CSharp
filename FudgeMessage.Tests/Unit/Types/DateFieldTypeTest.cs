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
using FudgeMessage.Encodings;
using System.IO;
using FudgeMessage;
using FudgeMessage.Util;

namespace FudgeMessage.Tests.Unit.Types
{
    public class DateFieldTypeTest
    {
        private FudgeContext context = new FudgeContext();

        [Test]
        public void RoundTrip()
        {
            var d = new FudgeDate(1999, 12, 10);

            var msg1 = new FudgeMsg(context, new Field("d", d));
            var bytes = msg1.ToByteArray();
            var msg2 = context.Deserialize(bytes).Message;

            Assert.AreEqual("1999-12-10", msg2.GetValue<FudgeDate>("d").ToString());
        }

        [Test]
        public void CheckActualBytes()
        {
            Func<FudgeDate, string> toByteString = d =>
            {
                var stream = new MemoryStream();
                var writer = new FudgeBinaryWriter(stream);
                DateFieldType.Instance.WriteValue(writer, d);
                return stream.ToArray().ToNiceString();
            };

            // Using examples from Confluence at http://www.fudgemsg.org/display/FDG/DateTime+encoding
            Assert.AreEqual("00-0f-b4-3f", toByteString(new FudgeDate(20100131)));
            Assert.AreEqual("00-0f-a1-00", toByteString(new FudgeDate(20000800)));
            Assert.AreEqual("a4-72-80-00", toByteString(new FudgeDate(-3000000, 0, 0)));
        }
    }
}
