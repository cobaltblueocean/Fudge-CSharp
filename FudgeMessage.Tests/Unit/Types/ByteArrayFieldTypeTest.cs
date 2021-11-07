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
using FudgeMessage.Types;

namespace FudgeMessage.Tests.Unit.Types
{
    public class ByteArrayFieldTypeTest
    {
        [Test]
        public void MinimizeToFixedType()
        {
            var baType = ByteArrayFieldType.VariableSizedInstance;
            FudgeFieldType type;
            byte[] data;

            type = baType;
            data = new byte[4];
            Assert.AreEqual(data, baType.Minimize(data, ref type));
            Assert.AreEqual(ByteArrayFieldType.Length4Instance, type);

            type = baType;
            data = new byte[8];
            Assert.AreEqual(data, baType.Minimize(data, ref type));
            Assert.AreEqual(ByteArrayFieldType.Length8Instance, type);

            type = baType;
            data = new byte[512];
            Assert.AreEqual(data, baType.Minimize(data, ref type));
            Assert.AreEqual(ByteArrayFieldType.Length512Instance, type);
        }

        [Test]
        public void MinimizeToIndicatorAndBack()
        {
            var baType = ByteArrayFieldType.VariableSizedInstance;
            FudgeFieldType type = baType;
            byte[] data = new byte[0];

            Assert.AreEqual(IndicatorType.Instance, baType.Minimize(data, ref type));
            Assert.AreEqual(IndicatorFieldType.Instance, type);

            Assert.AreEqual(data, baType.ConvertValueFrom(IndicatorType.Instance));
        }
    }
}
