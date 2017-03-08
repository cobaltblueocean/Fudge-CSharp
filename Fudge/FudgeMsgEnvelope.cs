/// <summary>
/// Copyright (C) 2009 - present by OpenGamma Inc. and other contributors.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///     http://www.apache.org/licenses/LICENSE-2.0
///     
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </summary>
using System;
using System.Collections.Generic;
using System.Text;
using Fudge.Taxon;

namespace Fudge
{
    /// <summary>
    /// Wraps a <see cref="FudgeMsg"/> for the purpose of encoding the envelope header.
    /// This is the object which is encoded for a top-level fudge message; sub-messages don't
    /// contain a separate envelope.
    /// </summary>
    public class FudgeMsgEnvelope : FudgeEncodingObject
    {
        private readonly FudgeMsg _message;
        private readonly int _version;
        private readonly int _processingDirectives;

        /// <summary>
        /// Constructs a new envelope containing an emtpy message.
        /// </summary>
        public FudgeMsgEnvelope()
            : this(new FudgeMsg())
        {
        }

        // TODO 2009-12-14 Andrew -- lose the constructor above; we should at least pass the context and use that to construct the inner message

        /// <summary>
        /// Creates a new <c>FudgeMsgEnvelope</c> around an existing <c>FudgeMsg</c>.
        /// </summary>
        /// <param name="message">message contained within the envelope</param>
        public FudgeMsgEnvelope(FudgeMsg message)
            : this(message, 0)
        {
        }

        /// <summary>
        /// Creates a new <c>FudgeMsgEnvelope</c> around an existing <c>FudgeMsg</c> with a specific encoding schema version. The default
        /// schema version is 0.
        /// </summary>
        /// <param name="message">message contained within the envelope</param>
        /// <param name="version">schema version, 0 to 255</param>
        public FudgeMsgEnvelope(FudgeMsg message, int version)
            : this(message, version, 0)
        {
        }

        /// <summary>
        /// Creates an envelope wrapping the given message with a version and processing directive flags.
        /// </summary>
        /// <param name="message">message contained within the envelope</param>
        /// <param name="version">schema version, 0 to 255</param>
        /// <param name="processingDirectives">the processing directive flags, from 0 to 255</param>
        public FudgeMsgEnvelope(FudgeMsg message, int version, int processingDirectives)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message", "Must specify a message to wrap.");
            }
            if (processingDirectives < 0 || processingDirectives > 255)
            {
                throw new ArgumentOutOfRangeException("Processing directives " + processingDirectives + " must fit in one byte");
            }
            if ((version < 0) || (version > 255))
            {
                throw new ArgumentOutOfRangeException("version", "Provided version " + version + " which doesn't fit within one byte.");
            }
            this._message = message;
            this._version = version;
            this._processingDirectives = processingDirectives;

        }

        /// <summary>
        /// Gets the message contained within this envelope.
        /// </summary>
        public FudgeMsg Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Gets the schema version of the envelope.
        /// </summary>
        public int Version
        {
            get { return _version; }
        }

        /// <summary>
        /// Gets the processing directive flags.
        /// </summary>
        public int ProcessingDirectives
        {
            get{
                return _processingDirectives;
            }
        }

        /// <summary>
        /// Calculates the total size of the envelope. This is the encoded size of the message within the envelope plus the 8 byte envelope header.
        /// </summary>
        /// <param name="taxonomy">optional taxonomy to encode the message with</param>
        /// <returns>total size in bytes</returns>
        public override int ComputeSize(IFudgeTaxonomy taxonomy)
        {
            int size = 0;
            // Message envelope header
            size += 8;
            size += _message.GetSize(taxonomy);
            return size;
        }

        /// <summary>
        /// Returns a string suitable for debugging.
        /// </summary>
        /// <returns>a string, not null</returns>
        public String ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("FudgeMsgEnvelope[");
            if (Version != 0)
            {
                buf.Append("version=").Append(Version).Append(',');
            }
            if (ProcessingDirectives != 0)
            {
                buf.Append("processing=").Append(ProcessingDirectives).Append(',');
            }
            buf.Append(Message).Append(']');
            return buf.ToString();
        }
    }
}
