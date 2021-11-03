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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FudgeMessage.Linq
{
    /// <summary>
    /// This is to represent the result of a sorting query that calls the method(s) OrderBy, OrderByDescending, ThenBy or ThenByDescending. 
    /// </summary>
    public class FudgeFieldContainerContext : IOrderedQueryable<IFudgeFieldContainer>
    {
        private Expression _Expression;
        private IQueryProvider _Provider;

        /// <summary>
        /// LINQ Expression
        /// </summary>
        public Expression Expression { get { return _Expression; } private set { _Expression = value; } }

        /// <summary>
        /// LINQ Provider
        /// </summary>
        public IQueryProvider Provider { get { return _Provider; } private set { _Provider = value; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">Source to query using this LINQ</param>
        public FudgeFieldContainerContext(IEnumerable<IFudgeFieldContainer> source)
        {
            Provider = new FudgeLinqProvider(source);
            Expression = Expression.Constant(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="provider">LINQ Provider</param>
        /// <param name="expression">Query expression</param>
        internal FudgeFieldContainerContext(IQueryProvider provider, Expression expression)
        {
            Provider = provider;
            Expression = expression;
        }

        /// <summary>
        /// Element type
        /// </summary>
        public Type ElementType
        {
            get
            {
                return typeof(IFudgeFieldContainer);
            }
        }

        /// <summary>
        /// Get Enumerator
        /// </summary>
        /// <returns>IEnumerator of IFudgeFieldContainer</returns>
        public IEnumerator<IFudgeFieldContainer> GetEnumerator()
        {
            return Provider.Execute<IEnumerable<IFudgeFieldContainer>>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
