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
using System.Text;

namespace FudgeMessage
{
    /// <summary>
    /// A read-only representation of a field which is contained in a fudge
    /// message, or a stream of fudge encoded data.
    /// </summary>
    public interface IFudgeField
    {
        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        FudgeFieldType Type { get; }

        /// <summary>
        /// Gets the .NET value of the field.
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Gets the ordinal index of the field, or null if none is specified.
        /// </summary>
        short? Ordinal { get; }

        /// <summary>
        /// Gets the descriptive name of the field, or null if none is specified.
        /// </summary>
        string Name { get; }
    }
}
