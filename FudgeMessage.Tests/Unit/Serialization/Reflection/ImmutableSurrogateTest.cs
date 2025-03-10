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
using FudgeMessage;
using FudgeMessage.Serialization;
using Mercury.Test.Utility;

namespace FudgeMessage.Tests.Unit.Serialization.Reflection
{
    [Parallelizable(ParallelScope.ContextMask)]
    public class ImmutableSurrogateTest
    {
        private readonly FudgeContext context = new FudgeContext();

        [Test]
        public void SimpleCase()
        {
            var serializer = new FudgeSerializer(context);

            var obj1 = new SimpleClass("Test", "Case");
            var msg = serializer.SerializeToMsg(obj1);
            var obj2 = (SimpleClass)serializer.Deserialize(msg);

            Assert2.AreEqual(obj1.Val1, obj2.Val1);
        }

        [Test]
        public void ConstructorOrderRelevant()
        {
            var msg1 = context.NewMessage(new Field(FudgeSerializer.TypeIdFieldOrdinal, typeof(SimpleClass).ToString()),
                                          new Field("Val1", "Test"),
                                          new Field("Val2", "Case"));
            var msg2 = context.NewMessage(new Field(FudgeSerializer.TypeIdFieldOrdinal, typeof(SimpleClass).ToString()),
                                          new Field("Val2", "Case"),
                                          new Field("Val1", "Test"));
            var serializer = new FudgeSerializer(context);

            var obj1 = (SimpleClass)serializer.Deserialize(msg1);
            var obj2 = (SimpleClass)serializer.Deserialize(msg2);
            Assert2.AreEqual(obj1.Val1, obj2.Val1);
            Assert2.AreEqual(obj1.Val2, obj2.Val2);
        }

        [Test]
        public void MissingFieldsAreNull()
        {
            var msg1 = context.NewMessage(new Field(FudgeSerializer.TypeIdFieldOrdinal, typeof(SimpleClass).ToString()),
                                          new Field("Val2", "Case"));
            var serializer = new FudgeSerializer(context);
            var obj1 = (SimpleClass)serializer.Deserialize(msg1);
            Assert2.Null(obj1.Val1);
            Assert2.AreEqual("Case", obj1.Val2);
        }

        [Test]
        public void ChoosesRightConstructor()
        {
            var msg1 = context.NewMessage(new Field(FudgeSerializer.TypeIdFieldOrdinal, typeof(MultiConstructor).ToString()),
                                          new Field("A", 17),
                                          new Field("B", "foo"));
            var serializer = new FudgeSerializer(context);
            var obj1 = (MultiConstructor)serializer.Deserialize(msg1);
            Assert2.AreEqual(17, obj1.A);
            Assert2.AreEqual("foo", obj1.B);
        }

        [Test]
        public void CycleInImmutable_FRN71()
        {
            var outer = new ImmutableCycle1(new ImmutableCycle2());
            outer.Other.Other = outer;
            var serializer = new FudgeSerializer(context);
            var msg = serializer.SerializeToMsg(outer);
            var outer2 = (ImmutableCycle1)serializer.Deserialize(msg);
            Assert2.AreEqual(outer2, outer2.Other.Other);
        }

        private class SimpleClass
        {
            private string val1;
            private string val2;

            public SimpleClass(string val1, string val2)
            {
                this.val1 = val1;
                this.val2 = val2;
            }

            public string Val1
            {
                get { return val1; }
            }

            public string Val2
            {
                get { return val2; }
            }
        }

        private class MultiConstructor
        {
            private readonly int a;
            private readonly string b;

            public MultiConstructor(int a)
                : this(a, null)
            {
            }

            public MultiConstructor(int a, string b)
            {
                this.a = a;
                this.b = b;
            }

            public int A { get { return a; } }

            public string B { get { return b; } }
        }

        private class ImmutableCycle1
        {
            private readonly ImmutableCycle2 other;

            public ImmutableCycle1(ImmutableCycle2 other)
            {
                this.other = other;
            }

            public ImmutableCycle2 Other { get { return other; } }
        }

        private class ImmutableCycle2
        {
            public ImmutableCycle2()
            {
            }

            public ImmutableCycle1 Other { get; set; }
        }
    }
}
