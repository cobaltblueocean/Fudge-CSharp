// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Incd and the OpenGamma group of companies
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
using FudgeMessage.Taxon;
using FudgeMessage.Mapping;

namespace FudgeMessage
{
    /// <summary>
    /// <p>Immutable wrapper for a {@link FudgeContext} that will be used for the global
    /// "default", or for use with {@link ImmutableFudgeMsg}d It cannot be configured
    /// after construction.</p>
    /// 
    /// @author Andrew Griffin
    /// </summary>
    public class ImmutableFudgeContext : FudgeContext
    {

        /// <summary>
        /// Creates an immutable version of an existing {@link FudgeContext}d Immutable copies of the type and object dictionaries
        /// are taken from the source context.
        /// 
        /// </summary>
        /// <param Name="context">the {@code FudgeContext} to base this on</param>
        public ImmutableFudgeContext(FudgeContext context)
        {
        }

        /// <summary>
        /// Always throws an exception - this is an immutable context.
        /// </summary>

        public void setTaxonomyResolver(ITaxonomyResolver taxonomyResolver)
        {
            throw new NotSupportedException("setTaxonomyResolver called on an immutable Fudge context");
        }

        /// <summary>
        /// Always throws an exception - this is an immutable context.
        /// </summary>

        public void setTypeDictionary(FudgeTypeDictionary typeDictionary)
        {
            throw new NotSupportedException("setTypeDictionary called on an immutable Fudge context");
        }

        /// <summary>
        /// Always throws an exception - this is an immutable context.
        /// </summary>

        public void setObjectDictionary(FudgeObjectDictionary objectDictionary)
        {
            throw new NotSupportedException("setObjectDictionary called on an immutable Fudge context");
        }
    }
}
