﻿/**
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
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FudgeMessage;
using FudgeMessage.Serialization;
using Mercury.Test.Utility;

namespace FudgeMessage.Tests.Unit.Serialization
{
    [Parallelizable(ParallelScope.ContextMask)]
    public class FudgeSerializationContextTest
    {
        [Test]
        public void CheckForCycles_FRN49()
        {
            var context = new FudgeContext();
            var serializer = new FudgeSerializer(context);

            var testObj = new ClassWithCycles();
            testObj.Child = new ClassWithCycles();
            serializer.SerializeToMsg(testObj);        // Doesn't throw because no cycles

            testObj.Child = testObj;
            Assert2.ThrowsException<FudgeRuntimeException>(() => serializer.SerializeToMsg(testObj));
        }

        private class ClassWithCycles
        {
            [FudgeInline]
            public ClassWithCycles Child { get; set; }
        }
    }
}
