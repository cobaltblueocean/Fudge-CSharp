/* <!--
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
 * -->
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Data.Objects;

namespace Fudge.Linq
{
    /// <summary>
    /// <c>FudgeLinqProvider</c> gives an implementation of <see cref="IQueryProvider"/> for
    /// sequences of <see cref="IFudgeFieldContainer"/>s.
    /// </summary>
    /// <remarks>
    /// <para>This is where the real work of Linq happens with <see cref="Expression"/>s that
    /// have been built on sequences of a reference type (by the compiler) being translated
    /// to operate on sequences of <see cref="IFudgeFieldContainer"/> instead.
    /// </para>
    /// <para>
    /// You would not normally construct one of these directly, but instead use the <c>AsQueryable</c>
    /// extension method on a <see cref="IEnumerable"/> of <see cref="IFudgeFieldContainer"/>s (e.g. a <c>List</c>
    /// or array).
    /// </para>
    /// <para>
    /// Note that we only currently support Select and Where clauses - this will be extended in
    /// the future.
    /// </para>
    /// <para>
    /// For a walkthrough of how this works, have a look at Matt Warren's MSDN blog at http://blogs.msdn.com/mattwar/pages/linq-links.aspx
    /// on which IQToolkit is based.
    /// </para>
    /// </remarks>
    public class FudgeLinqProvider : IQueryProvider
    {
        private readonly IEnumerable<IFudgeFieldContainer> source;
        private readonly bool useCache;

        /// <summary>
        /// Constructs a new <c>FudgeLinqProvider</c> from a set of <see cref="IFudgeFieldContainer"/>s (e.g. <see cref="FudgeMsg"/>s),
        /// using a cache to avoid recompilation of expressions we have already seen.
        /// </summary>
        /// <param name="source">Set of messages to operate on.</param>
        public FudgeLinqProvider(IEnumerable<IFudgeFieldContainer> source) : this(source, true)
        {
        }

        /// <summary>
        /// Constructs a new <c>FudgeLinqProvider</c> from a set of <see cref="IFudgeFieldContainer"/>s (e.g. <see cref="FudgeMsg"/>s),
        /// giving explicit control over whether to use a cache to avoid recompilation of expressions we have already seen.
        /// </summary>
        /// <param name="source">Set of messages to operate on.</param>
        /// <param name="useCache">If true then compiled expressions are cached and reused.</param>
        public FudgeLinqProvider(IEnumerable<IFudgeFieldContainer> source, bool useCache)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            this.source = source;
            this.useCache = useCache;
        }

        /// <summary>
        /// Gets the source of messages used by this provider.
        /// </summary>
        public IEnumerable<IFudgeFieldContainer> Source
        {
            get { return source; }
        }

        /// <inheritdoc/>
        public string GetQueryText(Expression expression)
        {
            return expression.ToString();
        }

        /// <inheritdoc/>
        public TResult Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }        


        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            throw new NotImplementedException();
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            throw new NotImplementedException();
        }

        object IQueryProvider.Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }
    }

}
