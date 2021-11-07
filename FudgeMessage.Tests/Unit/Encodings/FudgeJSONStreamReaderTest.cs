/*
 * <!--
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
using System.IO;
using System.Linq;
using NUnit.Framework;
using FudgeMessage;
using FudgeMessage.Encodings;
using FudgeMessage.Types;
using FudgeMessage.Util;
using Mercury.Test.Utility;

namespace FudgeMessage.Tests.Unit.Encodings
{
    public class FudgeJSONStreamReaderTest
    {
        private FudgeContext context = new FudgeContext();
        [Test]
        public void StringField()
        {
            string json = @"{""name"" : ""fred""}";

            var msg = new FudgeJSONStreamReader(context, json).ReadMsg();

            Assert.AreEqual("fred", msg.GetString("name"));
        }

        [Test]
        public void NumberFields()
        {
            string json = @"{""int"" : 1234, ""float"" : 123.45, ""exp"" : -123.45e4}";

            var msg = new FudgeJSONStreamReader(context, json).ReadMsg();

            Assert.AreEqual(1234, msg.GetInt("int"));
            Assert.AreEqual(123.45, msg.GetDouble("float"));
            Assert.AreEqual(-1234500, msg.GetDouble("exp"));
        }

        [Test]
        public void BooleanFields()
        {
            string json = @"{""old"" : true, ""young"" : false}";

            var msg = new FudgeJSONStreamReader(context, json).ReadMsg();

            Assert.AreEqual(true, msg.GetBoolean("old"));
            Assert.AreEqual(false, msg.GetBoolean("young"));
        }

        [Test]
        public void NullFields()
        {
            string json = @"{""old"" : null}";

            var msg = new FudgeJSONStreamReader(context, json).ReadMsg();

            Assert.AreEqual(IndicatorType.Instance, msg.GetByName("old").Value);
        }

        [Test]
        public void SubObjects()
        {
            string json = @"{""inner"" : { ""a"" : 3, ""b"" : 17.3 }}";

            var msg = new FudgeJSONStreamReader(context, json).ReadMsg();

            var inner = msg.GetMessage("inner");
            Assert.NotNull(inner);
            Assert.AreEqual(3, inner.GetInt("a"));
            Assert.AreEqual(17.3, inner.GetDouble("b"));
        }

        [Test]
        public void Arrays()
        {
            string json = @"{""numbers"" : [ 1, 2, 4], ""submsgs"" : [ { ""a"" : -3 }, { ""b"" : 28 } ] }";

            var msg = new FudgeJSONStreamReader(context, json).ReadMsg();

            var numbers = msg.GetAllByName("numbers");
            Assert.AreEqual(3, numbers.Count);                 // REVIEW 2009-12-18 t0rx -- Should JSON arrays collapse to primitive arrays where possible?
            Assert.AreEqual(1, (sbyte)numbers[0].Value);
            Assert.AreEqual(2, (sbyte)numbers[1].Value);
            Assert.AreEqual(4, (sbyte)numbers[2].Value);

            var messages = msg.GetAllByName("submsgs");
            Assert.AreEqual(2, messages.Count);
            Assert2.IsType<FudgeMsg>(messages[1].Value);
            var message2 = (FudgeMsg)messages[1].Value;
            Assert.AreEqual(28, (sbyte)message2.GetInt("b"));
        }

        [Test]
        public void UnicodeEscaping()
        {
            string json = @"{""name"" : ""fr\u0065d""}";

            var msg = new FudgeJSONStreamReader(context, json).ReadMsg();

            Assert.AreEqual("fred", msg.GetString("name"));
        }

        [Test]
        public void BadToken()
        {
            string json = @"{""old"" : ajshgd}";
            Assert.Throws<FudgeParseException>(() => { new FudgeJSONStreamReader(context, json).ReadMsg(); });

            json = @"{abcd : 16}";      // Field names must be quoted
            Assert.Throws<FudgeParseException>(() => { new FudgeJSONStreamReader(context, json).ReadMsg(); });
        }

        [Test]
        public void PrematureEOF()
        {
            string json = @"{""old"" : ";
            Assert.Throws<FudgeParseException>(() => { new FudgeJSONStreamReader(context, json).ReadMsg(); });
        }

        [Test]
        public void MultipleMessages()
        {
            string json = @"{""name"" : ""fred""} {""number"" : 17}";
            var reader = new FudgeJSONStreamReader(context, json);
            var writer = new FudgeMsgStreamWriter();
            new FudgeStreamPipe(reader, writer).Process();

            Assert.AreEqual(2, writer.PeekAllMessages().Count);
            Assert.AreEqual("fred", writer.DequeueMessage().GetString("name"));
            Assert.AreEqual(17, writer.DequeueMessage().GetInt("number"));
        }

        [Test]
        public void LargeMsg()
        {
            var stringWriter = new StringWriter();
            var streamWriter = new FudgeJSONStreamWriter(context, stringWriter);
            FudgeMsg inMsg = StandardFudgeMessages.CreateLargeMessage(context);
            streamWriter.WriteMsg(inMsg);
            
            var msg = new FudgeJSONStreamReader(context, stringWriter.GetStringBuilder().ToString()).ReadMsg();

            FudgeUtils.AssertAllFieldsMatch(inMsg, msg);
        }
    }
}
