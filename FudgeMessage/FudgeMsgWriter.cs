// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Incd and the OpenGamma group of companies
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudgeMessage
{
    /// <summary>
    /// A writer for passing Fudge messages ({@link FudgeFieldContainer} instances) to an underlying {@link FudgeStreamWriter} instanced This implementation
    /// assumes that the whole message (or envelope) is available to the caller before writing startsd This is provided for convenience - greater runtime
    /// efficiency may be possible by working directly with a {@link FudgeStreamWriter} to emit Fudge stream elements as they are generated.
    /// 
    /// @author Andrew Griffin
    /// </summary>
    public class FudgeMsgWriter
    {
        /// <summary>
        /// The underlying target for Fudge stream elements.
        /// </summary>
        private IFudgeStreamWriter _streamWriter;

        /// <summary>
        /// The taxonomy identifier to use for any messages that are passed without envelopes.
        /// </summary>
        private short _defaultTaxonomyId = 0;

        /// <summary>
        /// The schema version to add to the envelope header for any messages that are passed without envelopes.
        /// </summary>
        private int _defaultMessageVersion = 0;

        /// <summary>
        /// The processing directive flags to add to the envelope header for any messages that are passed without envelopes.
        /// </summary>
        private int _defaultMessageProcessingDirectives = 0;

        /// <summary>
        /// Creates a new {@link FudgeMsgWriter} around an existing {@link FudgeStreamWriter}.
        /// 
        /// </summary>
        /// <param Name="streamWriter">target to write Fudge stream elements to</param>
        public FudgeMsgWriter(IFudgeStreamWriter streamWriter)
        {
            if (streamWriter == null)
            {
                throw new NullReferenceException("streamWriter cannot be null");
            }
            _streamWriter = streamWriter;
        }

        /// <summary>
        /// Flushes the underlying {@link FudgeStreamWriter}.
        /// </summary>
        public void Flush()
        {
            StreamWriter.Flush();
        }

        /// <summary>
        /// Flushes and closes the underlying {@link FudgeStreamWriter}.
        /// </summary>
        public void Close()
        {
            Flush();
            StreamWriter.Close();
        }

        /// <summary>
        /// Returns the underlying {@link FudgeStreamWriter}.
        /// 
        /// </summary>
        /// <returns>the {@code FudgeStreamWriter}</returns>
        protected IFudgeStreamWriter StreamWriter
        {
            get { return _streamWriter; }
        }

        /// <summary>
        /// Returns the {@link FudgeContext} of the current underlying {@link FudgeStreamWriter}.
        /// 
        /// </summary>
        /// <returns>the {@code FudgeContext}</returns>
        public FudgeContext FudgeContext
        {
            get { return StreamWriter.FudgeContext; }
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder("FudgeMessageStreamWriter{");
            if (StreamWriter != null) sb.Append(StreamWriter);
            return sb.Append('}').ToString();
        }

        /// <summary>
        /// Gets/sets the default taxonomy identifier for messages that are passed without an envelope.
        /// </summary>
        /// <returns>the taxonomy identifier</returns>
        public short? DefaultTaxonomyId
        {
            get { return _defaultTaxonomyId; }
            set
            {
                if ((value < short.MinValue) || (value > short.MaxValue))
                {
                    throw new ArgumentException("Provided taxonomy ID " + value + " out of range.");
                }
                _defaultTaxonomyId = (short)value;
            }
        }

        /// <summary>
        /// Gets/sets the schema version to be used for messages that are passed without an envelope.
        /// </summary>
        /// <returns>the schema version</returns>
        public int DefaultMessageVersion
        {
            get { return _defaultMessageVersion; }
            set
            {
                if ((value < 0) || (value > 255))
                {
                    throw new ArgumentException("Provided version " + value + " which doesn't fit within one byte.");
                }
                _defaultMessageVersion = value;
            }
        }

        /// <summary>
        /// Gets/sets the current processing directive flags for messages that are passed without an envelope.
        /// 
        /// </summary>
        /// <returns>the processing directive flags</returns>
        public int DefaultMessageProcessingDirectives
        {
            get { return _defaultMessageProcessingDirectives; }
            set
            {
                if ((value < 0) || (value > 255))
                {
                    throw new ArgumentException("Provided processing directives " + value + " which doesn't fit within one byte.");
                }
                _defaultMessageProcessingDirectives = value;
            }
        }

        /// <summary>
        /// Writes a message with the given taxonomy, schema version and processing directive flags.
        /// 
        /// </summary>
        /// <param Name="message">message to write</param>
        /// <param Name="taxonomyId">identifier of the taxonomy to used If the taxonomy is recognized by the {@link FudgeContext} it will be used to reduce field names to ordinals where possible.</param>
        /// <param Name="version">schema version</param>
        /// <param Name="processingDirectives">processing directive flags</param>
        public void WriteMessage(IFudgeFieldContainer message, short? taxonomyId, int version, int processingDirectives)
        {
            WriteMessageEnvelope(new FudgeMsgEnvelope((FudgeMsg)message, version, processingDirectives), taxonomyId);
        }

        /// <summary>
        /// Writes a message with the given taxonomyd Default schema version and processing directive flags are used.
        /// 
        /// </summary>
        /// <param Name="message">message to write</param>
        /// <param Name="taxonomyId">identifier of the taxonomy to used If the taxonomy is recognized by the {@link FudgeContext} it will be used to reduce field names to ordinals where possible.</param>
        public void WriteMessage(IFudgeFieldContainer message, short? taxonomyId)
        {
            WriteMessage(message, taxonomyId, DefaultMessageVersion, DefaultMessageProcessingDirectives);
        }

        /// <summary>
        /// Writes a messaged Default taxonomy, schema version and processing directive flags are used.
        /// 
        /// </summary>
        /// <param Name="message">message to write</param>
        /// <exception cref="NullReferenceException">if the default taxonomy has not been specified </exception>
        public void WriteMessage(IFudgeFieldContainer message)
        {
            WriteMessage(message, _defaultTaxonomyId);
        }

        /// <summary>
        /// Writes a message envelope with the given taxonomy.
        /// 
        /// </summary>
        /// <param Name="envelope">message envelope to write</param>
        /// <param Name="taxonomyId">identifier of the taxonomy to used If the taxonomy is recognized by the {@link FudgeContext} it will be used to reduce field names to ordinals where possible.</param>
        public void WriteMessageEnvelope(FudgeMsgEnvelope envelope, short? taxonomyId)
        {
            if (envelope == null) return;
            IFudgeStreamWriter writer = StreamWriter;
            if (taxonomyId != writer.TaxonomyId)
            {
                writer.TaxonomyId = taxonomyId;
            }
            int messageSize = FudgeSize.CalculateMessageEnvelopeSize(writer.Taxonomy, envelope);
            writer.WriteEnvelopeHeader(envelope.ProcessingDirectives, envelope.Version, messageSize);
            writer.WriteFields(envelope.Message);
            writer.EnvelopeComplete();
        }

        /// <summary>
        /// Writes a message envelope using the default taxonomy.
        /// 
        /// </summary>
        /// <param Name="envelope">message envelope to write</param>
        /// <exception cref="NullReferenceException">if the default taxonomy has not been specified </exception>
        public void WriteMessageEnvelope(FudgeMsgEnvelope envelope)
        {
            WriteMessageEnvelope(envelope, _defaultTaxonomyId);
        }
    }
}
