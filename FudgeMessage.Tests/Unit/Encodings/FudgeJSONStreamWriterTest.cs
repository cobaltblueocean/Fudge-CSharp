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
 * -->
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FudgeMessage.Encodings;
using System.IO;
using NUnit.Framework;
using FudgeMessage;
using FudgeMessage.Types;
using Mercury.Test.Utility;

namespace FudgeMessage.Tests.Unit.Encodings
{
    [Parallelizable(ParallelScope.ContextMask)]
    public class FudgeJSONStreamWriterTest
    {
        private readonly FudgeContext context = new FudgeContext();

        [Test]
        public void SimpleExample()
        {
            var msg = context.NewMessage(new Field("outer",
                                            new Field("a", 7),
                                            new Field("b", "fred")));
            var stringWriter = new StringWriter();
            var writer = new FudgeJSONStreamWriter(context, stringWriter);
            writer.WriteMsg(msg);
            string s = stringWriter.ToString();

            AssertEqualsNoWhiteSpace("{\"outer\" : {\"a\" : 7, \"b\" : \"fred\"} }", s);
        }

        [Test]
        public void FieldsWithSameNameBecomeArrays()
        {
            var msg = context.NewMessage(new Field("outer",
                                            new Field("a", false),
                                            new Field("b", 17.3),
                                            new Field("a", "test")));

            var stringWriter = new StringWriter();
            var writer = new FudgeJSONStreamWriter(context, stringWriter);
            writer.WriteMsg(msg);
            string s = stringWriter.ToString();

            AssertEqualsNoWhiteSpace("{\"outer\" : {\"a\" : [false, \"test\"], \"b\" : 17.3} }", s);
        }

        [Test]
        public void EscapingAndEncodingStrings()
        {
            var msg = context.NewMessage(new Field("s", "\\\b\f\t\n\r\"\u0001"));

            var stringWriter = new StringWriter();
            var writer = new FudgeJSONStreamWriter(context, stringWriter);
            writer.WriteMsg(msg);
            string s = stringWriter.ToString();

            AssertEqualsNoWhiteSpace(@"{""s"" : ""\\\b\f\t\n\r\""\u0001""}", s);
        }

        [Test]
        public void ArraysOfPrimitiveTypes()
        {
            var msg = context.NewMessage(new Field("bytes", new byte[] {123, 76, 2, 3}),
                                         new Field("shorts", new short[] {10, 11, 12}),
                                         new Field("ints", new int[] {13, 14, 15}),
                                         new Field("longs", new long[] {16, 17, 18}),
                                         new Field("floats", new float[] {1.2f, 3.4f, 5.6f}),
                                         new Field("doubles", new double[] {-1.2, -3.4, -5.6}));

            var stringWriter = new StringWriter();
            var writer = new FudgeJSONStreamWriter(context, stringWriter);
            writer.WriteMsg(msg);
            string s = stringWriter.ToString();

            var testString = "{\"bytes\" : [123, 76, 2, 3]," +
                              "\"shorts\" : [10, 11, 12]," +
                              "\"ints\" : [13, 14, 15]," +
                              "\"longs\" : [16, 17, 18]," +
                              "\"floats\" : [1.2, 3.4, 5.6]," +
                              "\"doubles\" : [-1.2, -3.4, -5.6]}";
            AssertEqualsNoWhiteSpace(testString, s);
        }

        [Test]
        public void ArraysOfObjects()
        {
            var msg = context.NewMessage(new Field("a",
                                            new Field("x", 1),
                                            new Field("y", 2)),
                                         new Field("a",
                                             new Field("z", 3)));

            var stringWriter = new StringWriter();
            var writer = new FudgeJSONStreamWriter(context, stringWriter);
            writer.WriteMsg(msg);
            string s = stringWriter.ToString();

            AssertEqualsNoWhiteSpace("{\"a\" : [ {\"x\" : 1, \"y\" : 2}, {\"z\" : 3} ] }", s);
        }

        [Test]
        public void RoundTrip()
        {
            var msg = context.NewMessage(new Field("outer",
                                            new Field("a", 7),
                                            new Field("b", "fred")));
            var stringWriter = new StringWriter();
            var writer = new FudgeJSONStreamWriter(context, stringWriter);
            writer.WriteMsg(msg);
            string s = stringWriter.ToString();

            var reader = new FudgeJSONStreamReader(context, s);
            var msg2 = reader.ReadMsg();

            FudgeUtils.AssertAllFieldsMatch(msg, msg2);
        }

        [Test]
        public void SubMessageAsValueHandled()
        {
            var subMsg = context.NewMessage(new Field("a", 1));
            var stringWriter = new StringWriter();
            var writer = new FudgeJSONStreamWriter(context, stringWriter);

            writer.StartMessage();
            writer.WriteField("test", null, null, subMsg);
            writer.EndMessage();

            string s = stringWriter.ToString();
            AssertEqualsNoWhiteSpace("{\"test\" : {\"a\" : 1} }", s);
        }

        [Test]
        public void IndicatorWrittenAsNull()
        {
            var msg = context.NewMessage(new Field("test", IndicatorType.Instance));

            var stringWriter = new StringWriter();
            var writer = new FudgeJSONStreamWriter(context, stringWriter);
            writer.WriteMsg(msg);
            string s = stringWriter.ToString();
            AssertEqualsNoWhiteSpace("{\"test\" : null }", s);
        }

        [Test]
        public void ConstuctorRangeChecking()
        {
            var stringWriter = new StringWriter();

            Assert2.ThrowsException<ArgumentNullException>(() => new FudgeJSONStreamWriter(null, stringWriter));
            Assert2.ThrowsException<ArgumentNullException>(() => new FudgeJSONStreamWriter(context, null));
        }

        [Test]
        public void DepthChecks()
        {
            var writer = new FudgeJSONStreamWriter(context, new StringWriter());

            Assert2.ThrowsException<InvalidOperationException>(() => writer.StartSubMessage("test", null));
            Assert2.ThrowsException<InvalidOperationException>(() => writer.WriteField("test", null, null, "test"));
            Assert2.ThrowsException<InvalidOperationException>(() => writer.EndSubMessage());
            Assert2.ThrowsException<InvalidOperationException>(() => writer.EndMessage());

            writer.StartMessage();
            Assert2.ThrowsException<InvalidOperationException>(() => writer.StartMessage());
            Assert2.ThrowsException<InvalidOperationException>(() => writer.EndSubMessage());

            writer.StartSubMessage("test", null);
            Assert2.ThrowsException<InvalidOperationException>(() => writer.EndMessage());
        }

        [Test]
        public void TestEqualsNoWhiteSpaceWorks()
        {
            // Test our test works
            AssertEqualsNoWhiteSpace("\ta b\r\nc", " \rab\n\tc ");
            Assert2.ThrowsException<AssertionException>(() => AssertEqualsNoWhiteSpace("ab", "ac"));
        }

        private void AssertEqualsNoWhiteSpace(string a, string b)
        {
            var whiteSpace = new string[] { " ", "\t", "\r", "\n" };
            foreach (var s in whiteSpace)
            {
                a = a.Replace(s, "");
                b = b.Replace(s, "");
            }

            Assert2.AreEqual(a, b);
        }
    }
}
