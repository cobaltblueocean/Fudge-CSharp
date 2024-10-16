﻿// Copyright (c) 2017 - presented by Kei Nakai
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
    /// An annotation which at runtime specifies that a particular class is a
    /// {@link FudgeMessageBuilder} or {@link FudgeObjectBuilder} for a particular
    /// data type.
    /// While {@link HasFudgeBuilder} allows the data object to specify what its builder(s) are,
    /// in a case where a builder has been written external to a source data type, this annotation
    /// allows {@link FudgeObjectDictionary#addAllAnnotatedBuilders()} to determine the
    /// builder and automatically configure.
    /// 
    /// @author Kirk Wylie
    /// </summary>
    public interface IFudgeBuilderFor<T>
    {
        /// <summary>
        /// The value for which this is a {@link FudgeObjectBuilder} or
        /// {@link FudgeMessageBuilder}.
        /// </summary>
        T Value();

    }
}
