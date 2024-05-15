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

namespace FudgeMessage.Extension
{
    /// <summary>
    /// FudgeDictionaryExtension Description
    /// </summary>
    internal static class FudgeDictionaryExtension
    {
        /// <summary>
        /// Add the Key and Value to the Dictionary.  If the Key exists, update the value with new supplied value
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Target Dictionary</param>
        /// <param name="key">Key to add or update</param>
        /// <param name="value">Value to supply</param>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey key, TValue value)
        {
            if (key != null)
            {
                if (originalDictionary.ContainsKey(key))
                    originalDictionary[key] = value;
                else
                    originalDictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Add the Key and Value if the key doesn't exist
        /// </summary>
        /// <typeparam name="TKey">Type of the Key</typeparam>
        /// <typeparam name="TValue">Type of the Value</typeparam>
        /// <param name="originalDictionary">Target Dictionary</param>
        /// <param name="key">Key to add</param>
        /// <param name="value">Value to supply</param>
        /// <returns>The value added</returns>
        public static TValue PutIfAbsent<TKey, TValue>(this IDictionary<TKey, TValue> originalDictionary, TKey key, TValue value)
        {
            if (!originalDictionary.ContainsKey(key))
            {
                originalDictionary.Add(key, value);
            }

            return originalDictionary[key];
        }
    }
}
