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
using System.IO;
using FudgeMessage;
using FudgeMessage.Encodings;
using FudgeMessage.Util;
using Mercury.Test.Utility;

namespace FudgeMessage.Tests.Unit.Encodings
{
    [Parallelizable(ParallelScope.ContextMask)]
    public class FudgeEncodedStreamReaderTest
    {
        [Test]
        public void CheckElementsCorrectForSimpleMessage()
        {
            var context = new FudgeContext();
            var msg = context.NewMessage();
            msg.Add("Test", "Bob");
            var bytes = context.ToByteArray(msg);

            var stream = new MemoryStream(bytes);
            var reader = new FudgeEncodedStreamReader(context, stream);

            Assert2.True(reader.HasNext);
            Assert2.AreEqual(FudgeStreamElement.MessageStart, reader.MoveNext());
            Assert2.True(reader.HasNext);
            Assert2.AreEqual(FudgeStreamElement.SimpleField, reader.MoveNext());
            Assert2.True(reader.HasNext);
            Assert2.AreEqual(FudgeStreamElement.MessageEnd, reader.MoveNext());
            Assert2.False(reader.HasNext);
            Assert2.AreEqual(FudgeStreamElement.NoElement, reader.MoveNext());
            Assert2.False(reader.HasNext);
            Assert2.AreEqual(FudgeStreamElement.NoElement, reader.MoveNext());
        }

        [Test]
        public void CheckEndOfStreamWithoutHasNext()
        {
            // Same as CheckElementsCorrectForSimpleMessage but without using HasNext
            var context = new FudgeContext();
            var msg = context.NewMessage();
            msg.Add("Test", "Bob");
            var bytes = context.ToByteArray(msg);

            var stream = new MemoryStream(bytes);
            var reader = new FudgeEncodedStreamReader(context, stream);

            Assert2.AreEqual(FudgeStreamElement.MessageStart, reader.MoveNext());
            Assert2.AreEqual(FudgeStreamElement.SimpleField, reader.MoveNext());
            Assert2.AreEqual(FudgeStreamElement.MessageEnd, reader.MoveNext());
            Assert2.AreEqual(FudgeStreamElement.NoElement, reader.MoveNext());
            Assert2.AreEqual(FudgeStreamElement.NoElement, reader.MoveNext());
        }

        [Test]
        public void CheckElementsCorrectForSubMessage()
        {
            var context = new FudgeContext();
            var msg = context.NewMessage();
            var subMsg = context.NewMessage();
            msg.Add("sub", subMsg);
            subMsg.Add("Test", "Bob");
            var bytes = context.ToByteArray(msg);

            var stream = new MemoryStream(bytes);
            var reader = new FudgeEncodedStreamReader(context, stream);

            Assert2.True(reader.HasNext);
            Assert2.AreEqual(FudgeStreamElement.MessageStart, reader.MoveNext());
            Assert2.True(reader.HasNext);
            Assert2.AreEqual(FudgeStreamElement.SubmessageFieldStart, reader.MoveNext());
            Assert2.True(reader.HasNext);
            Assert2.AreEqual(FudgeStreamElement.SimpleField, reader.MoveNext());
            Assert2.True(reader.HasNext);
            Assert2.AreEqual(FudgeStreamElement.SubmessageFieldEnd, reader.MoveNext());
            Assert2.True(reader.HasNext);
            Assert2.AreEqual(FudgeStreamElement.MessageEnd, reader.MoveNext());
            Assert2.False(reader.HasNext);
        }

        [Test]
        public void MultipleMessages()
        {
            // Same as CheckElementsCorrectForSimpleMessage but without using HasNext
            var context = new FudgeContext();
            var msg1 = context.NewMessage();
            msg1.Add("Test", "Bob");
            var msg2 = context.NewMessage();
            msg2.Add("Test2", "Shirley");
            var msgs = new FudgeMsg[] {msg1, msg2};
            var stream = new MemoryStream();
            var writer = new FudgeEncodedStreamWriter(context, stream);
            new FudgeStreamPipe(new FudgeMsgStreamReader(context, msgs), writer).Process();

            stream.Position = 0;
            var reader = new FudgeEncodedStreamReader(context, stream);

            Assert2.AreEqual(FudgeStreamElement.MessageStart, reader.MoveNext());
            Assert2.AreEqual(FudgeStreamElement.SimpleField, reader.MoveNext());
            Assert2.AreEqual(FudgeStreamElement.MessageEnd, reader.MoveNext());
            Assert2.AreEqual(FudgeStreamElement.MessageStart, reader.MoveNext());
            Assert2.AreEqual(FudgeStreamElement.SimpleField, reader.MoveNext());
            Assert2.AreEqual(FudgeStreamElement.MessageEnd, reader.MoveNext());
            Assert2.AreEqual(FudgeStreamElement.NoElement, reader.MoveNext());
            Assert2.AreEqual(FudgeStreamElement.NoElement, reader.MoveNext());
        }
    }
}
