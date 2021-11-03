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
    /// An interface which at runtime specifies that a particular class is a
    /// {@link FudgeBuilder} or {@link FudgeObjectBuilder} for a particular
    /// abstract or interface data type.
    /// Where the intention is to invoke {@link FudgeBuilderFactory#addGenericBuilder(Class, FudgeBuilder)} rather
    /// than {@link FudgeObjectDictionary#addBuilder(Class, FudgeBuilder)}, this annotation should be used instead.
    /// 
    /// @author Kirk Wylie
    /// </summary>
    public interface IGenericFudgeBuilderFor
    {
        dynamic Value();
    }
}
