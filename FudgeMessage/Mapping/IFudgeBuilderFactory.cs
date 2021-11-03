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

namespace FudgeMessage.Mapping
{
    /// <summary>
    /// Factory interface for constructing builders for classes that haven't been explicitly
    /// registered with a {@link FudgeObjectDictionary}. The factory should not attempt to
    /// cache results - the {@code FudgeObjectDictionary} will do that.
    /// 
    /// @author Andrew Griffin
    /// </summary>
    public interface IFudgeBuilderFactory
    {

        /// <summary>
        /// Creates a new {@link FudgeObjectBuilder} for deserializing Fudge messages into the given class.
        /// 
        /// </summary>
        /// <typeparam name="T">the class the builder should create objects of</typeparam>
        /// <param name="type">the class the builder should create objects of</param>
        /// <returns>the builder or {@code null} if none is available</returns>
        public IFudgeObjectBuilder<T> CreateObjectBuilder<T>(Type type);

        /// <summary>
        /// Creates a new {@link FudgeMessageBuilder} for encoding objects of the given class into Fudge messages.
        /// 
        /// </summary>
        /// <param name="type">the class the builder should create messages from</param>
        /// <typeparam name="T">the class the builder should create messages from</typeparam>
        /// <returns> the builder or {@code null} if none is available</returns>
        public IFudgeMessageBuilder<T> CreateMessageBuilder<T>(Type type);

        /// <summary>
        /// Registers a generic builder with the factory that may be returned as a {@link FudgeObjectBuilder} for
        /// the class, or as a {@link FudgeMessageBuilder} for any sub-classes of the class. After calling this, a
        /// factory may choose to return an alternative builder, but may not return {@code null} for a class which
        /// the generic builder has been registered for.
        /// 
        /// </summary>
        /// <param name="type">the generic type (probably an interface) the builder is for</param>
        /// <param name="builder">the builder to register</param>
        /// <typeparam name="T">the generic type (probably an interface) the builder is for</typeparam>
        public void AddGenericBuilder<T>(Type type, IFudgeBuilder<T> builder);

    }
}
