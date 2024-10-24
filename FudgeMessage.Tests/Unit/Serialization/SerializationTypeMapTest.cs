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

namespace FudgeMessage.Tests.Unit.Serialization
{
    [Parallelizable(ParallelScope.ContextMask)]
    public class SerializationTypeMapTest
    {
        [Test]
        public void PicksUpPropertiesFromContext()
        {
            var context = new FudgeContext();

            var map1 = new SerializationTypeMap(context);
            Assert2.True(map1.AllowTypeDiscovery);

            context.SetProperty(ContextProperties.AllowTypeDiscoveryProperty, false);
            var map2 = new SerializationTypeMap(context);
            Assert2.False(map2.AllowTypeDiscovery);
        }

        [Test]
        public void AllowTypeDiscoveryBehaviour()
        {
            var context = new FudgeContext();
            var map = new SerializationTypeMap(context);

            map.AllowTypeDiscovery = false;
            Assert2.Null(map.GetSurrogate(typeof(Reflect.Tick)));
            Assert2.AreEqual(-1, map.GetTypeId(typeof(Reflect.Tick)));

            map.AllowTypeDiscovery = true;
            Assert2.NotNull(map.GetSurrogate(typeof(Reflect.Tick)));
            Assert2.AreNotEqual(-1, map.GetTypeId(typeof(Reflect.Person)));
        }
    }
}
