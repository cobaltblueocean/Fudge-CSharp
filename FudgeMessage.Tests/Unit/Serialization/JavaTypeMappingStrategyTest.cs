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
using FudgeMessage.Serialization;
using Mercury.Test.Utility;

namespace FudgeMessage.Tests.Unit.Serialization
{
    [Parallelizable(ParallelScope.ContextMask)]
    public class JavaTypeMappingStrategyTest
    {
        [Test]
        public void SimpleExample()
        {
            var mapper = new JavaTypeMappingStrategy("FudgeMessage.Tests.Unit", "org.fudgemsg");
            Assert2.AreEqual("org.fudgemsg.serialization.JavaTypeMappingStrategyTest", mapper.GetName(this.GetType()));
            Assert2.AreEqual(this.GetType(), mapper.GetType("org.fudgemsg.serialization.JavaTypeMappingStrategyTest"));
        }

        [Test]
        public void InnerClasses()
        {
            var mapper = new JavaTypeMappingStrategy("FudgeMessage.Tests.Unit", "org.fudgemsg");
            Assert2.AreEqual("org.fudgemsg.serialization.JavaTypeMappingStrategyTest$Inner", mapper.GetName(typeof(Inner)));
            Assert2.AreEqual(typeof(Inner), mapper.GetType("org.fudgemsg.serialization.JavaTypeMappingStrategyTest$Inner"));
        }

        [Test]
        public void ConstructorRangeChecking()
        {
            Assert2.ThrowsException<ArgumentNullException>(() => new JavaTypeMappingStrategy(null, ""));
            Assert2.ThrowsException<ArgumentNullException>(() => new JavaTypeMappingStrategy("", null));
        }

        private class Inner
        {
        }
    }
}
