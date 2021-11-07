﻿/**
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
using System.IO;
using FudgeMessage;

namespace FudgeMessage.Tests.Unit
{
    public class FudgeMsgFormatterTest
    {
        private static readonly FudgeContext fudgeContext = new FudgeContext();

        /// <summary>
        /// Will output a <see cref="FudgeMsg"/> to <see cref="Console.Out"/> so that you can visually
        /// examine it.
        /// </summary>
        [Test]
        public void OutputToStdoutAllNames()
        {
            Console.Out.WriteLine("FudgeMsgFormatterTest.OutputToStdoutAllNames()");
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllNames(fudgeContext);
            msg.Add("Sub Message", (short)9999, StandardFudgeMessages.CreateMessageAllNames(fudgeContext));
            new FudgeMsgFormatter(Console.Out).Format(msg);
        }

        /// <summary>
        /// Will output a <see cref="FudgeMsg"/> to <see cref="Console.Out"/> so that you can visually
        /// examine it.
        /// </summary>
        public void OutputToStdoutAllOrdinals()
        {
            Console.Out.WriteLine("FudgeMsgFormatterTest.OutputToStdoutAllOrdinals()");
            FudgeMsg msg = StandardFudgeMessages.CreateMessageAllOrdinals(fudgeContext);
            msg.Add("Sub Message", (short)9999, StandardFudgeMessages.CreateMessageAllOrdinals(fudgeContext));
            new FudgeMsgFormatter(Console.Out).Format(msg);
        }
    }
}
