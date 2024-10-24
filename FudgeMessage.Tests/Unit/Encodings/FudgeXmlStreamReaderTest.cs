﻿/*
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FudgeMessage.Encodings;
using System.Xml;
using System.IO;
using FudgeMessage;
using FudgeMessage.Types;
using FudgeMessage.Util;
using Mercury.Test.Utility;

namespace FudgeMessage.Tests.Unit.Encodings
{
    [Parallelizable(ParallelScope.ContextMask)]
    public class FudgeXmlStreamReaderTest
    {
        private FudgeContext context = new FudgeContext();

        [Test]
        public void Attributes()
        {
            string xml = "<msg><name type=\"surname\" value=\"Smith\"/></msg>";

            var reader = new FudgeXmlStreamReader(context, xml);
            var msg = reader.ReadMsg();

            Assert2.AreEqual(FudgeMsgFieldType.Instance, msg.GetByName("name").Type);
            var name = msg.GetMessage("name");
            Assert2.AreEqual("surname", name.GetString("type"));
            Assert2.AreEqual("Smith", name.GetString("value"));
        }

        [Test]
        public void AttributesAndText()
        {
            // Value should go into a field with empty name
            // REVIEW 2009-12-17 t0rx -- Is this a good thing to do, or should it go in a field called "value", or just be ignored?
            string xml = "<msg><name type=\"surname\">Smith</name></msg>";

            var reader = new FudgeXmlStreamReader(context, xml);
            var msg = reader.ReadMsg();

            Assert2.AreEqual(FudgeMsgFieldType.Instance, msg.GetByName("name").Type);
            var name = msg.GetMessage("name");
            Assert2.AreEqual("surname", name.GetString("type"));
            Assert2.AreEqual("Smith", name.GetString(""));
        }

        [Test]
        public void AttributesAndSubElements()
        {
            string xml = "<msg><name type=\"surname\"><value>Smith</value></name></msg>";

            var reader = new FudgeXmlStreamReader(context, xml);
            var msg = reader.ReadMsg();

            Assert2.AreEqual(FudgeMsgFieldType.Instance, msg.GetByName("name").Type);
            var name = msg.GetMessage("name");
            Assert2.AreEqual("surname", name.GetString("type"));
            Assert2.AreEqual("Smith", name.GetString("value"));
        }

        [Test]
        public void NestedMessages()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?><msg><name>Fred</name><address><number>17</number><line1>Our House</line1><line2>In the middle of our street</line2><phone>1234</phone><local /></address></msg>";

            var reader = new FudgeXmlStreamReader(context, xml);
            var writer = new FudgeMsgStreamWriter();
            new FudgeStreamPipe(reader, writer).Process();

            var msg = writer.DequeueMessage();

            Assert2.AreEqual("Our House", msg.GetMessage("address").GetString("line1"));

            // Convert back to XML and see if it matches
            var sb = new StringBuilder();
            var xmlWriter = XmlWriter.Create(sb);
            var reader2 = new FudgeMsgStreamReader(context, msg);
            var writer2 = new FudgeXmlStreamWriter(context, xmlWriter, "msg") { AutoFlushOnMessageEnd = true };
            new FudgeStreamPipe(reader2, writer2).Process();

            var xml2 = sb.ToString();
            Assert2.AreEqual(xml, xml2);
        }

        [Test]
        public void MultipleMessages()
        {
            string inputXml = "<msg><name>Fred</name></msg><msg><name>Bob</name></msg>";
            var reader = new FudgeXmlStreamReader(context, inputXml);

            var sb = new StringBuilder();
            var xmlWriter = XmlWriter.Create(sb, new XmlWriterSettings {OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment});
            var writer = new FudgeXmlStreamWriter(context, xmlWriter, "msg") { AutoFlushOnMessageEnd = true };
            var multiwriter = new FudgeStreamMultiwriter(new DebuggingWriter(), writer);
            new FudgeStreamPipe(reader, multiwriter).Process();
            string outputXml = sb.ToString();

            Assert2.AreEqual(inputXml, outputXml);
        }

        [Test]
        public void LargeMsg()
        {
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter);
            var streamWriter = new FudgeXmlStreamWriter(context, xmlWriter, "msg");
            FudgeMsg inMsg = StandardFudgeMessages.CreateLargeMessage(context);
            streamWriter.WriteMsg(inMsg);

            string msgString = stringWriter.GetStringBuilder().ToString();
            var msg = new FudgeXmlStreamReader(context, msgString).ReadMsg();

            FudgeUtils.AssertAllFieldsMatch(inMsg, msg);
        }
    }
}
