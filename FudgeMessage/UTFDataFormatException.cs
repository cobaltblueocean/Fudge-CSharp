// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by the Apache Software Foundation.
//
// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreements.  See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the License.  You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
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

namespace FudgeMessage
{
    /// <summary>
    /// Signals that an incorrectly encoded UTF-8 string has been encountered, most
    /// likely while reading some {@link DataInputStream}.
    /// </summary>
    public class UTFDataFormatException : FormatException
    {
        /// <summary>
        /// Constructs a new {@code UTFDataFormatException} with its stack trace
        /// filled in.
        /// </summary>
        public UTFDataFormatException()
        {
        }
        /// <summary>
        /// Constructs a new {@code UTFDataFormatException} with its stack trace and
        /// detail message filled in.
        /// 
        /// </summary>
        /// <param name="detailMessage"></param>
        ///            the detail message for this exception.
        public UTFDataFormatException(String detailMessage) : base(detailMessage)
        {
           
        }
    }
}
