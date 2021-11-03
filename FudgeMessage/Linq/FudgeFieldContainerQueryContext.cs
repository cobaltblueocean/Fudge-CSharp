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
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FudgeMessage.Linq
{
    /// <summary>
    /// This is responsible for creating queries and evaluating them.  At the lowest level this basically means we need to be able to create queries and then execute them. 
    /// </summary>
    public class FudgeFieldContainerQueryContext
    {
        private static readonly Dictionary<CacheEntry, CacheEntry> cache = new Dictionary<CacheEntry, CacheEntry>();
        private static readonly ReaderWriterLock cacheLock = new ReaderWriterLock();

        private readonly bool useCache = true;

        internal object Execute(Expression expression, bool isEnumerable, IEnumerable<IFudgeFieldContainer> source)
        {

            if (expression.NodeType != ExpressionType.Call)
                throw new Exception("Unsupported node type: " + expression.NodeType);

            var m = (MethodCallExpression)expression;
            if (m.Method.Name != "Select")
                throw new Exception("Unsupported method: " + m.Method.Name);

            // Pull out the constants from the new expression
            IList<object> values;
            var extractedLambda = ConstantExtractor.Extract(m, out values);

            // Now get compiled version from cache, or create as appropriate
            var cacheKey = new CacheEntry(m);
            var cachedEntry = GetCachedEntry(cacheKey);
            if (cachedEntry == null)
            {
                Type dataType = m.Method.GetGenericArguments()[0];                  // Queryable.Select<TSource,TResult>(...)

                // Perform the translation to using FudgeMsgs rather than the data type
                var translator = new FudgeExpressionTranslator(dataType, source);
                var newSelect = (LambdaExpression)translator.Translate(extractedLambda);

                // We can now create a fully-fledged cache entry
                cachedEntry = new CacheEntry(cacheKey, newSelect.Compile());
                AddCacheEntry(cachedEntry);
            }

            //var queryableElements = source as IQueryable<IFudgeFieldContainer>;

            //// Copy the expression tree that was passed in, changing only the first
            //// argument of the innermost MethodCallExpression.
            //var treeCopier = new ExpressionTreeModifier(queryableElements);
            //var newExpressionTree = treeCopier.CopyAndModify(expression);

            //// This step creates an IQueryable that executes by replacing Queryable methods with Enumerable methods.
            //if (isEnumerable)
            //{
            //    return queryableElements.Provider.CreateQuery(newExpressionTree);
            //}
            //else
            //{
            //    return queryableElements.Provider.Execute(newExpressionTree);
            //}

            return cachedEntry.Invoke(values.ToArray());
        }

        private CacheEntry GetCachedEntry(CacheEntry entry)
        {
            CacheEntry result = null;
            if (useCache)
            {
                cacheLock.AcquireReaderLock(Timeout.Infinite);

                cache.TryGetValue(entry, out result);

                cacheLock.ReleaseReaderLock();
            }
            return result;
        }

        private void AddCacheEntry(CacheEntry entry)
        {
            if (useCache)
            {
                cacheLock.AcquireWriterLock(Timeout.Infinite);

                cache[entry] = entry;

                cacheLock.ReleaseWriterLock();
            }
        }

        internal class ExpressionTreeModifier : ExpressionVisitor
        {
            private IQueryable<IFudgeFieldContainer> queryablePlaces;

            internal ExpressionTreeModifier(IQueryable<IFudgeFieldContainer> places)
            {
                this.queryablePlaces = places;
            }

            internal Expression CopyAndModify(Expression expression)
            {
                return this.Visit(expression);
            }

            protected override Expression VisitConstant(ConstantExpression c)
            {
                // Replace the constant QueryableTerraServerData arg with the queryable Place collection. 
                if (c.Type == typeof(IFudgeFieldContainer))
                    return Expression.Constant(this.queryablePlaces);
                else
                    return c;
            }
        }

        private class CacheEntry
        {
            private readonly int hashCode;
            private readonly Expression expression;
            private readonly Delegate compiledQuery;
            private Func<object[], object> invoker;     // For why it's worth doing this see http://msdn.microsoft.com/en-us/magazine/cc163759.aspx#S3

            public CacheEntry(Expression expression)
            {
                this.expression = expression;
                this.hashCode = ExpressionTreeStructureHasher.ComputeHash(expression);
            }

            public CacheEntry(CacheEntry other, Delegate compiledQuery)
            {
                this.expression = other.expression;
                this.hashCode = other.hashCode;
                this.compiledQuery = compiledQuery;
                this.invoker = FindInvoker(compiledQuery);
            }

            public Expression Expression
            {
                get { return expression; }
            }

            public object Invoke(object[] args)
            {
                return invoker(args);
            }

            public Func<object[], object> FindInvoker(Delegate del)
            {
                // Calling DynamicInvoke is very slow compared to using a typed delegate, so we manufacture one if 
                // we can - see http://msdn.microsoft.com/en-us/magazine/cc163759.aspx#S3

                Debug.Assert(invoker == null);

                Type delegateType = del.GetType();
                if (delegateType.FullName.StartsWith("System.Func`"))       // Make sure it's one we understand
                {
                    Type[] genericArgs = delegateType.GetGenericArguments();
                    MethodInfo method = this.GetType().GetMethod("Invoke" + genericArgs.Length, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (method != null)
                    {
                        // Found a match
                        return (Func<object[], object>)Delegate.CreateDelegate(typeof(Func<object[], object>), this, method.MakeGenericMethod(genericArgs));
                    }
                }

                return DefaultInvoker;
            }

            #region Invokers

            private object DefaultInvoker(object[] args)
            {
                // If we can't use a fast invoker then we have to use the slow DynamicInvoke
                return compiledQuery.DynamicInvoke(args);
            }

            private object Invoke1<RType>(object[] args)
            {
                return ((Func<RType>)compiledQuery)();
            }

            private object Invoke2<Arg0, RType>(object[] args)
            {
                return ((Func<Arg0, RType>)compiledQuery)((Arg0)args[0]);
            }

            private object Invoke3<Arg0, Arg1, RType>(object[] args)
            {
                return ((Func<Arg0, Arg1, RType>)compiledQuery)((Arg0)args[0], (Arg1)args[1]);
            }

            private object Invoke4<Arg0, Arg1, Arg2, RType>(object[] args)
            {
                return ((Func<Arg0, Arg1, Arg2, RType>)compiledQuery)((Arg0)args[0], (Arg1)args[1], (Arg2)args[2]);
            }

            private object Invoke5<Arg0, Arg1, Arg2, Arg3, RType>(object[] args)
            {
                return ((Func<Arg0, Arg1, Arg2, Arg3, RType>)compiledQuery)((Arg0)args[0], (Arg1)args[1], (Arg2)args[2], (Arg3)args[3]);
            }

            #endregion

            #region Object overrides

            public override bool Equals(object obj)
            {
                if (obj == this)
                    return true;
                CacheEntry other = obj as CacheEntry;
                if (other == null)
                    return false;

                return Expression.Lambda<Func<bool>>(Expression.Equal(Expression, other.Expression)).Compile()();
            }

            public override int GetHashCode()
            {
                return hashCode;
            }

            #endregion
        }


        /// <summary>
        /// Extracts constants from the expression tree out so the compiled query can be used with
        /// different constants.
        /// </summary>
        /// <remarks>
        /// It'd be nice to use IQToolkit.QueryCache.QueryParameterizer, but that's not public
        /// </remarks>
        private class ConstantExtractor : ExpressionVisitor
        {
            private readonly List<ParameterExpression> newParameters = new List<ParameterExpression>();
            private readonly List<object> constantValues = new List<object>();

            public ConstantExtractor()
            {
            }

            public static LambdaExpression Extract(Expression exp, out IList<object> values)
            {
                var extractor = new ConstantExtractor();
                var body = extractor.Visit(exp);
                values = extractor.constantValues.ToArray();
                if (values.Count >= 5)
                {
                    // Can't create a lambda with more than 4 parameters.  Go figure.
                    values = new object[] { values };
                    return ParamArrayRewriter.Rewrite(body, extractor.newParameters.ToArray());
                }
                return Expression.Lambda(body, extractor.newParameters.ToArray());
            }

            protected override Expression VisitConstant(ConstantExpression c)
            {
                // Strip the constant out into a parameter
                var param = Expression.Parameter(c.Type, "c" + constantValues.Count);
                newParameters.Add(param);
                constantValues.Add(c.Value);
                if (c.Value is IQueryable)
                {
                    // We actually return c so it can be translated later
                    return c;
                }
                return param;
            }
        }

        /// <summary>
        /// This turns a body that expects a number of parameters, to instead have just one array param and index into that
        /// </summary>
        private class ParamArrayRewriter : ExpressionVisitor
        {
            private static readonly ParameterExpression paramParam = Expression.Parameter(typeof(object[]), "params");
            private readonly ParameterExpression[] parameters;

            public ParamArrayRewriter(ParameterExpression[] parameters)
            {
                this.parameters = parameters;
            }

            public static LambdaExpression Rewrite(Expression body, ParameterExpression[] parameters)
            {
                var rewriter = new ParamArrayRewriter(parameters);
                var newBody = rewriter.Visit(body);
                return Expression.Lambda(newBody, paramParam);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (p == parameters[i])
                    {
                        // Rewrite it
                        return Expression.Convert(Expression.ArrayIndex(paramParam, Expression.Constant(i)), p.Type);
                    }
                }

                // Not found
                return p;
            }
        }

    }
}
