// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
//
// Please see distribution for license.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
//     
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fudge
{
    /// <summary>
    /// A factory for Fudge messages.
    /// 
    /// This offers the minimal API necessary to create new Fudge messages.
    /// Use of this interface avoids exposing additional methods and knowledge to other APIs.
    /// </summary>
    public interface IFudgeMessageFactory
    {
        /// <summary>
        /// Creates an initially empty message.
        /// </summary>
        /// <returns>the empty message container, not null</returns>
        IMutableFudgeFieldContainer NewMessage();

        /// <summary>
        /// Creates a new message initially populated with the supplied message.
        /// </summary>
        /// <param name="FromMessage">the source message to copy fields from, not null</param>
        /// <returns>the new message container, not null</returns>
        IMutableFudgeFieldContainer NewMessage(IFudgeFieldContainer FromMessage);
    }
}
