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
using System.Collections.Generic;
using System.Linq;
using FudgeMessage;

namespace FudgeMessage.Linq
{
    /// <summary>
    /// Extensions methods for handling Fudge messages with Linq.
    /// </summary>
    public static class FudgeLinqExtensions
    {
        /// <summary>
        /// Map an <c>IEnumerable&lt;FudgeMsg&gt;</c> onto an <see cref="IQueryable{T}"/> so
        /// that all the Linq compiler magic works.
        /// </summary>
        /// <typeparam name="T">Type of the object which has the structure of the message data.</typeparam>
        /// <param name="msgSource">Enumerable (array, list, etc.) to map</param>
        /// <returns></returns>
        /// <remarks>
        /// <para>In the same way as Linq-to-SQL, Linq-to-Fudge relies on there being a proper type that the message
        /// can be mapped onto (i.e. a property with the same name and type as each field that we need to operate
        /// on in the message).  This is only used to make Intellisense and the compiler work - no actual
        /// deserialisation into objects happens whilst processing.
        /// </para>
        /// <para>See the <c>Linq.Examples</c> unit test for some examples.</para>
        /// </remarks>
        public static IOrderedQueryable<T> AsQueryable<T>(this IEnumerable<FudgeMsg> msgSource)
        {
            return msgSource.Cast<IFudgeFieldContainer>().AsQueryable<T>();
        }

        /// <summary>
        /// Map a <c>FudgeMsg[]</c> onto an <see cref="IQueryable{T}"/> so
        /// that all the Linq compiler magic works.
        /// </summary>
        /// <typeparam name="T">Type of the object which has the structure of the message data.</typeparam>
        /// <param name="msgSource">Array to map</param>
        /// <returns></returns>
        /// <remarks>
        /// <para>In the same way as Linq-to-SQL, Linq-to-Fudge relies on there being a proper type that the message
        /// can be mapped onto (i.e. a property with the same name and type as each field that we need to operate
        /// on in the message).  This is only used to make Intellisense and the compiler work - no actual
        /// deserialisation into objects happens whilst processing.
        /// </para>
        /// <para>See the <c>Linq.Examples</c> unit test for some examples.</para>
        /// <para>We need this version in addition to IEnumerable&lt;IFudgeFieldContainer&gt; and IEnumerable&lt;FudgeMsg&gt;
        /// because otherwise arrays would be ambiguous.</para>
        /// </remarks>
        public static IOrderedQueryable<T> AsQueryable<T>(this FudgeMsg[] msgSource)
        {
            IOrderedQueryable<T> result;

            result = new FudgeFieldContainerContext(msgSource.Cast<IFudgeFieldContainer>()) as IOrderedQueryable<T>;

            if (result == null)
                result = (IOrderedQueryable<T>)msgSource.CastAsList<T>().AsQueryable<T>();

            return result;
        }

        /// <summary>
        /// Map an <c>IEnumerable&lt;IFudgeFieldContainer&gt;</c> onto an <see cref="IQueryable{T}"/> so
        /// that all the Linq compiler magic works.
        /// </summary>
        /// <typeparam name="T">Type of the object which has the structure of the message data.</typeparam>
        /// <param name="msgSource">Enumerable (array, list, etc.) to map</param>
        /// <returns></returns>
        /// <remarks>
        /// <para>In the same way as Linq-to-SQL, Linq-to-Fudge relies on there being a proper type that the message
        /// can be mapped onto (i.e. a property with the same name and type as each field that we need to operate
        /// on in the message).  This is only used to make Intellisense and the compiler work - no actual
        /// deserialisation into objects happens whilst processing.
        /// </para>
        /// <para>See the <c>Linq.Examples</c> unit test for some examples.</para>
        /// </remarks>
        public static IOrderedQueryable<T> AsQueryable<T>(this IEnumerable<IFudgeFieldContainer> msgSource)
        {
            IOrderedQueryable<T> result;

            result = new FudgeFieldContainerContext(msgSource.Cast<IFudgeFieldContainer>()) as IOrderedQueryable<T>;

            if (result == null)
                result = (IOrderedQueryable<T>)msgSource.CastAsList<T>().AsQueryable<T>();

            return result;
        }


        private static List<T> CastAsList<T>(this FudgeMsg[] msgSource)
        {
            List<T> result = new List<T>();
            int i = 0;

            foreach (var msg in msgSource)
            {
                result.Add(ClassUtility.NewInstance<T>(typeof(T)));

                var fields = msg.GetAllFields();
                foreach (var field in fields)
                {
                    ClassUtility.SetPropertyValue(result[i], field.Name, field.Value);
                }
                i++;
            }

            return result;
        }

        private static List<T> CastAsList<T>(this IEnumerable<IFudgeFieldContainer> msgSource)
        {
            List<T> result = new List<T>();
            int i = 0;

            foreach (var msg in msgSource)
            {
                result.Add(ClassUtility.NewInstance<T>(typeof(T)));

                var fields = msg.GetAllFields();
                foreach (var field in fields)
                {
                    ClassUtility.SetPropertyValue(result[i], field.Name, field.Value);
                }
                i++;
            }

            return result;
        }
    }
}
