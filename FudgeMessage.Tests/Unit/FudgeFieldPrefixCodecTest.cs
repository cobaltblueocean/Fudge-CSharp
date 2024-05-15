/**
 * Copyright (C) 2009 - 2009 by OpenGamma Inc. and other contributors.
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
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FudgeMessage;
using Mercury.Test.Utility;

namespace FudgeMessage.Tests.Unit
{
    [Parallelizable(ParallelScope.ContextMask)]
    public class FudgeFieldPrefixCodecTest
    {
        [Test]
        public void FieldPrefixComposition()
        {
            Assert2.AreEqual(0x20, FudgeFieldPrefixCodec.ComposeFieldPrefix(false, 10, false, false));
            Assert2.AreEqual(0x40, FudgeFieldPrefixCodec.ComposeFieldPrefix(false, 1024, false, false));
            Assert2.AreEqual(0x60, FudgeFieldPrefixCodec.ComposeFieldPrefix(false, short.MaxValue + 1000, false, false));
            Assert2.AreEqual(0x98, FudgeFieldPrefixCodec.ComposeFieldPrefix(true, 0, true, true));
        }

        [Test]
        public void HasNameChecks()
        {
            Assert2.False(FudgeFieldPrefixCodec.HasName(0x20));
            Assert2.True(FudgeFieldPrefixCodec.HasName(0x98));
        }

        [Test]
        public void fixedWidthChecks()
        {
            Assert2.False(FudgeFieldPrefixCodec.IsFixedWidth(0x20));
            Assert2.True(FudgeFieldPrefixCodec.IsFixedWidth(0x98));
        }

        [Test]
        public void hasOrdinalChecks()
        {
            Assert2.False(FudgeFieldPrefixCodec.HasOrdinal(0x20));
            Assert2.True(FudgeFieldPrefixCodec.HasOrdinal(0x98));
        }

        [Test]
        public void varWidthSizeChecks()
        {
            Assert2.AreEqual(0, FudgeFieldPrefixCodec.GetFieldWidthByteCount(0x98));
            Assert2.AreEqual(1, FudgeFieldPrefixCodec.GetFieldWidthByteCount(0x20));
            Assert2.AreEqual(2, FudgeFieldPrefixCodec.GetFieldWidthByteCount(0x40));
            Assert2.AreEqual(4, FudgeFieldPrefixCodec.GetFieldWidthByteCount(0x60));
        }

    }
}
