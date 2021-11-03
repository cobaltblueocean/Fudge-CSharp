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
using System.Reflection;

namespace FudgeMessage
{
    /// <summary>
    /// ClassUtility Description
    /// </summary>
    public class ClassUtility
    {
        /// <summary>
        /// Get Class name with attribute info
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <see href="https://ykobosknow.info/csharp/attribute-create.html"/>
        /// <see href="https://ykobosknow.info/csharp/csharp-attribute-access.html"/>
        /// <see href="https://takap-tech.com/entry/2020/10/03/133437"/>
        public static HashSet<String> GetClassAttributeValues(Type targetType)
        {
            HashSet<String> attrbutes = new HashSet<string>();
            attrbutes.Add(targetType.Name);

            targetType.GetCustomAttributes();

            foreach (Attribute attr in targetType.GetCustomAttributes())
            {
                // Only reference to the Class that defined Attribute; exclude this process if attr is null.
                if (attr != null)
                {
                    attrbutes.Add(string.Format("{0}, TypeID={1}", attr.GetType ().Name, attr.TypeId));
                }
            }

            return attrbutes;
        }
    }
}
