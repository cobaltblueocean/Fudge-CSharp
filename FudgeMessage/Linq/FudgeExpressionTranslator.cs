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
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Diagnostics;

namespace FudgeMessage.Linq
{
    /// <summary>
    /// Used to translate <see cref="Expression"/>s so that calls to get values from members of the
    /// reference type become <c>GetValue</c> calls on the <see cref="IFudgeFieldContainer"/> instead.
    /// </summary>
    internal class FudgeExpressionTranslator : ExpressionVisitor
    {
        private static readonly ParameterExpression msgParam = Expression.Parameter(typeof(IFudgeFieldContainer), "msg");

        #region Pre-cached methods
        private static MethodInfo getValueMethod = typeof(IFudgeFieldContainer).GetMethod("GetValue", new Type[] { typeof(string), typeof(Type) });

        private static MethodInfo selectMethod = (from mi in typeof(Enumerable).GetMethods()
                                                  where mi.Name == "Select" && mi.GetParameters()[1].ParameterType.GetGenericArguments().Length == 2
                                                  select mi).Single();                                  // Find Enumerable.Select(IEnumerable<TSource>,Func<TSource,TResult>) rather than Func<TSource,Int32,Boolean> variant
        private static MethodInfo whereMethod = (from mi in typeof(Enumerable).GetMethods()
                                                 where mi.Name == "Where" && mi.GetParameters()[1].ParameterType.GetGenericArguments().Length == 2
                                                 select mi).Single();                                   // Find Enumerable.Where(IEnumerable<TSource>,Func<TSource,Boolean>) rather than Func<TSource,Int32,Boolean> variant
        private static MethodInfo orderByMethod = (from mi in typeof(Enumerable).GetMethods()
                                                   where mi.Name == "OrderBy" && mi.GetParameters().Length == 2
                                                   select mi).Single();
        private static MethodInfo orderByDescendingMethod = (from mi in typeof(Enumerable).GetMethods()
                                                             where mi.Name == "OrderByDescending" && mi.GetParameters().Length == 2
                                                             select mi).Single();
        #endregion

        private readonly IEnumerable<IFudgeFieldContainer> source;
        private readonly Type dataType;

        public FudgeExpressionTranslator(Type dataType, IEnumerable<IFudgeFieldContainer> source)
        {
            this.dataType = dataType;
            this.source = source;
        }

        public Expression Translate(Expression exp)
        {
            var newExp = this.Visit(exp);
            return newExp;
        }

        public static Expression StripQuotes(Expression exp)
        {
            while (exp.NodeType == ExpressionType.Quote)
                exp = ((UnaryExpression)exp).Operand;
            return exp;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            // This is where we switch the ultimate source used by the expression tree from the Query<type> to our IEnumerable<IFudgeFieldContainer>
            if (c.Type.IsGenericType && c.Type.GetGenericTypeDefinition() == typeof(IOrderedQueryable<>) && c.Type.GetGenericArguments()[0] == dataType)
            {
                return Expression.Constant(source, typeof(IEnumerable<>).MakeGenericType(typeof(IFudgeFieldContainer)));
            }

            return base.VisitConstant(c);
        }

        protected Expression VisitLambda(LambdaExpression lambda)
        {
            // If we have a lambda of the form dataType => something then it now becomes IFudgeFieldContainer => something
            var body = Visit(lambda.Body);
            IList<ParameterExpression> parameters = lambda.Parameters;
            if (parameters.Count == 1 && parameters[0].Type == dataType)
            {
                return Expression.Lambda(body, msgParam);
            }

            if (body == lambda.Body)
                return lambda;
            else
                return Expression.Lambda(body, parameters.ToArray());
        }

        internal ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<Expression> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                Expression p = this.Visit(original[i]);
                if (list != null)
                {
                    list.Add(p);
                }
                else if (p != original[i])
                {
                    list = new List<Expression>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }
                    list.Add(p);
                }
            }
            if (list != null)
                return list.ToReadOnlyCollection();
            return original;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            // Translate calls to Queryable.xxx<IQueryable<dataType>(...) to Enumerable.xxx<IEnumerable<IFudgeFieldContainer>>(...)
            Expression obj = Visit(m.Object);
            var args = VisitExpressionList(m.Arguments);

            var method = m.Method;
            if (method.DeclaringType == typeof(Queryable))
            {
                var newArgs = (from arg in args select StripQuotes(arg)).ToArray();     // Get rid of pesky quotes
                switch (method.Name)
                {
                    case "Select":
                        {
                            Debug.Assert(newArgs.Length == 2);
                            var newMethod = selectMethod.MakeGenericMethod(newArgs[1].Type.GetGenericArguments());     // type args for method are same as for our func
                            return Expression.Call(newMethod, newArgs);
                        }
                    case "Where":
                        {
                            Debug.Assert(newArgs.Length == 2);
                            var newMethod = whereMethod.MakeGenericMethod(new Type[] { typeof(IFudgeFieldContainer) });
                            return Expression.Call(newMethod, newArgs);
                        }
                    case "OrderBy":
                        {
                            Debug.Assert(newArgs.Length == 2);
                            var newMethod = orderByMethod.MakeGenericMethod(newArgs[1].Type.GetGenericArguments());     // type args for method are same as for our func
                            return Expression.Call(newMethod, newArgs);
                        }
                    case "OrderByDescending":
                        {
                            Debug.Assert(newArgs.Length == 2);
                            var newMethod = orderByDescendingMethod.MakeGenericMethod(newArgs[1].Type.GetGenericArguments());     // type args for method are same as for our func
                            return Expression.Call(newMethod, newArgs);
                        }
                    default:
                        break;
                }
            }
            return UpdateMethodCall(m, obj, method, args);
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            // Pick up accesses to dataType.member and translate to IFudgeFieldContainer.GetValue(membername)
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter && m.Expression.Type == dataType)
            {
                // Change the member access to the data type into a call to get the value from the message
                return Expression.Convert(Expression.Call(msgParam, getValueMethod, Expression.Constant(m.Member.Name), Expression.Constant(m.Type)), m.Type);
            }
            else
            {
                return base.VisitMember(m);
            }
        }

        private static MethodCallExpression UpdateMethodCall(MethodCallExpression node, Expression obj, MethodInfo method, IEnumerable<Expression> args)
        {
            if (obj != node.Object || method != node.Method || args != node.Arguments)
            {
                return Expression.Call(obj, method, args);
            }

            return node;
        }
    }

    internal static class ReadOnlyCollectionExtensions
    {
        internal static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
                return DefaultReadOnlyCollection<T>.Empty;
            ReadOnlyCollection<T> col = sequence as ReadOnlyCollection<T>;
            if (col != null)
                return col;
            return new ReadOnlyCollection<T>(sequence.ToArray());
        }
        private static class DefaultReadOnlyCollection<T>
        {
            private static volatile ReadOnlyCollection<T> _defaultCollection;
            internal static ReadOnlyCollection<T> Empty
            {
                get
                {
                    if (_defaultCollection == null)
                        _defaultCollection = new ReadOnlyCollection<T>(new T[] { });
                    return _defaultCollection;
                }
            }
        }
    }

}
