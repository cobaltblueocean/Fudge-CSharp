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
using FudgeMessage.Serialization;

namespace FudgeMessage.Mapping
{

    public interface IFudgeMessageBuilder
    {
    
    }

    /// <summary>
    /// Defines an object capable of adding data from a given Java object to a fudge message.
    /// 
    /// </summary>
    /// <typeparam name="T">the Java type this builder creates Fudge message from</typeparam>
    /// 
    /// @author Andrew Griffin
    public interface IFudgeMessageBuilder<T>: IFudgeMessageBuilder
    {

        /// <summary>
        /// Creates a message from the given object. Note that a mutable container must be returned, this
        /// is to allow efficient implementation of sub-class builders that only need append data to the
        /// super-class message.
        /// 
        /// </summary>
        /// <param name="context">the {@link FudgeSerializationContext}</param>
        /// <param name="object">the object to serialise</param>
        /// <returns>the Fudge message</returns>
        IMutableFudgeFieldContainer BuildMessage(FudgeSerializationContext context, T value);
    }
}
