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
    /// Implementation of a {@link FudgeBuilderFactory} that can delegate to another
    /// instance for unrecognized classes. This pattern is to allow factories to be
    /// chained together.
    /// 
    /// @author Andrew Griffin
    /// </summary>
    public class FudgeBuilderFactoryAdapter : IFudgeBuilderFactory
    {
        private IFudgeBuilderFactory _delegate;

        /// <summary>
        /// Creates a new {@link FudgeBuilderFactoryAdapter}.
        /// 
        /// </summary>
        /// <param name="delegate">instance to pass non-overridden method calls to</param>
        protected FudgeBuilderFactoryAdapter(IFudgeBuilderFactory dlgt)
        {
            if (dlgt == null) throw new NullReferenceException("delegate cannot be null");
            _delegate = dlgt;
        }

        /// <summary>
        /// Returns the delegate instance to pass method calls to.
        /// 
        /// </summary>
        /// <returns>the {@link FudgeBuilderFactory} delegate</returns>
        protected IFudgeBuilderFactory Delegate
        {
            get { return _delegate; }
        }

        public virtual void AddGenericBuilder<T>(Type type, IFudgeBuilder<T> builder)
        {
            Delegate.AddGenericBuilder(type, builder);
        }

        public virtual IFudgeMessageBuilder<T> CreateMessageBuilder<T>(Type type)
        {
            return Delegate.CreateMessageBuilder<T>(type);
        }

        public virtual IFudgeObjectBuilder<T> CreateObjectBuilder<T>(Type type)
        {
            return Delegate.CreateObjectBuilder<T>(type);
        }
    }
}
