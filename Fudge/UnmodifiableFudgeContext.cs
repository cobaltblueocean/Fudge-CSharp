/// <summary>
/// Copyright (C) 2009 - present by OpenGamma Inc. and other contributors.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///     http://www.apache.org/licenses/LICENSE-2.0
///     
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fudge.Taxon;
using Fudge.Mapping;

namespace Fudge
{
    /// <summary>
    /// An unmodifiable wrapper for FudgeContext.
    /// 
    /// This is a simple wrapper for a context that blocks the mutable methods.
    /// This does not fully secure the context from editing, being intended simply
    /// to stop common programming errors.
    /// </summary>
    public class UnmodifiableFudgeContext : FudgeContext
    {
        /// <summary>
        /// Creates an immutable version of an existing FudgeContext. Immutable copies of the type and object dictionaries
        /// are taken from the source context.
        /// </summary>
        /// <param name="context"></param>
        public UnmodifiableFudgeContext(FudgeContext context)
        {
            base.TaxonomyResolver = context.TaxonomyResolver;
            base.TypeDictionary =  context.TypeDictionary;
            base.ObjectDictionary =  context.ObjectDictionary;
        }

        /// <summary>
        /// Always throws an exception - this is an immutable context.
        /// </summary>
        public new ITaxonomyResolver TaxonomyResolver
        {
            set { throw new NotSupportedException("TaxonomyResolver cannot set the value on an immutable Fudge context"); }
        }

        /// <summary>
        /// Always throws an exception - this is an immutable context.
        /// </summary>
        public new FudgeTypeDictionary TypeDictionary
        {
            set { throw new NotSupportedException("TypeDictionary cannot set the value on an immutable Fudge context"); }
        }

        /// <summary>
        /// Always throws an exception - this is an immutable context.
        /// </summary>
        public new FudgeObjectDictionary ObjectDictionary
        {
            set { throw new NotSupportedException("ObjectDictionary cannot set the value on an immutable Fudge context"); }
        }
    }
}
