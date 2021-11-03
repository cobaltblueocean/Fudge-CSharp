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
    /// Immutable <see cref="IFudgeBuilderFactory"/> implementation.
    /// 
    /// @author Andrew Griffin
    /// </summary>
    public class ImmutableFudgeBuilderFactory : FudgeBuilderFactoryAdapter
    {
        public ImmutableFudgeBuilderFactory(IFudgeBuilderFactory dlgt) : base(dlgt)
        {

        }

        /// <summary>
        /// Always throws an exception - this is an immutable factory
        /// 
        /// </summary>
        /// <param name="clazz">the generic type (probably an interface) the builder is for</param>
        /// <param name="builder">the builder to register</param>
        /// <typeparam name="T">the generic type (probably an interface) the builder is for</typeparam>
        public override void AddGenericBuilder<T>(Type type, IFudgeBuilder<T> builder)
        {
            throw new NotSupportedException("AddGenericBuilder called on immutable instance");
        }
    }
}
