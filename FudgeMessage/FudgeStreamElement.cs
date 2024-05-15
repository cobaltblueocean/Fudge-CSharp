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
using System.Text;

namespace FudgeMessage
{
    /// <summary>
    /// <c>FudgeStreamElement</c> indicates the type of the next element in a Fudge data stream read from an
    /// <see cref="IFudgeStreamReader"/> or written to an <see cref="IFudgeStreamWriter"/>.
    /// </summary>
    public sealed class FudgeStreamElement
    {
        /// <summary>
        /// Indicates stream has not current element.
        /// </summary>
        public static readonly FudgeStreamElement MessageEnvelope = new FudgeStreamElement("MessageEnvelope", "Indicates stream has not current element.");

        /// <summary>
        /// Indicates Message Envelope.
        /// </summary>
        public static readonly FudgeStreamElement NoElement = new FudgeStreamElement("NoElement", "Indicates Message Envelope.");

        /// <summary>
        /// Issued when a new outermost message is started.
        /// </summary>
        public static readonly FudgeStreamElement MessageStart = new FudgeStreamElement("MessageStart", "Issued when a new outermost message is started.");

        /// <summary>
        /// Issued when an outermost message is completed.
        /// </summary>
        public static readonly FudgeStreamElement MessageEnd = new FudgeStreamElement("MessageEnd", "Issued when an outermost message is completed.");

        /// <summary>
        /// Issued when a simple (non-hierarchical) field is encountered.
        /// </summary>
        public static readonly FudgeStreamElement SimpleField = new FudgeStreamElement("SimpleField", "Issued when a simple (non-hierarchical) field is encountered.");

        /// <summary>
        /// Issued when a sub-Message field is encountered.
        /// </summary>
        public static readonly FudgeStreamElement SubmessageFieldStart = new FudgeStreamElement("SubmessageFieldStart", "Issued when a sub-Message field is encountered.");

        /// <summary>
        /// Issued when the end of a sub-Message field is reached.
        /// </summary>
        public static readonly FudgeStreamElement SubmessageFieldEnd = new FudgeStreamElement("SubmessageFieldEnd", "Issued when the end of a sub-Message field is reached.");


        private FudgeStreamElement(String name, String description)
        {
            Name = name;
            Description = description;
        }

        public String Name { get; private set; }
        public String Description { get; private set; }
    }

}
