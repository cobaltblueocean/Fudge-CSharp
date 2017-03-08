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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fudge.Wire;

namespace Fudge
{
    public class FudgeMsgWriter
    {
        private IFudgeStreamWriter _streamWriter;
        private short _defaultTaxonomyId = 0;
        private int _defaultMessageVersion = 0;
        private int _defaultMessageProcessingDirectives = 0;

        public FudgeMsgWriter(IFudgeStreamWriter streamWriter)
        {
            if (streamWriter == null)
            {
                throw new NullReferenceException("streamWriter cannot be null");
            }
            _streamWriter = streamWriter;
        }

        public void Flush()
        {
            StreamWriter.Flush();
        }

        public void Close()
        {
            Flush();
            StreamWriter.Close();
        }

        protected IFudgeStreamWriter StreamWriter
        {
            get{
                return _streamWriter;
            }
        }

        public FudgeContext getFudgeContext()
        {
            return StreamWriter.FudgeContext;
        }

        public String toString()
        {
            StringBuilder sb = new StringBuilder("FudgeMessageStreamWriter{");
            if (StreamWriter != null) sb.Append(StreamWriter);
            return sb.Append('}').ToString();
        }

        public int getDefaultTaxonomyId()
        {
            return _defaultTaxonomyId;
        }

        public void setDefaultTaxonomyId(int taxonomyId)
        {
            if ((taxonomyId < short.MinValue) || (taxonomyId > short.MaxValue))
            {
                throw new ArgumentOutOfRangeException("Provided taxonomy ID " + taxonomyId + " out of range.");
            }
            _defaultTaxonomyId = (short)taxonomyId;
        }
        public int getDefaultMessageVersion()
        {
            return _defaultMessageVersion;
        }

        public void setDefaultMessageVersion(int version)
        {
            if ((version < 0) || (version > 255))
            {
                throw new ArgumentException("Provided version " + version + " which doesn't fit within one byte.");
            }
            _defaultMessageVersion = version;
        }

        public int getDefaultMessageProcessingDirectives()
        {
            return _defaultMessageProcessingDirectives;
        }

        public void setDefaultMessageProcessingDirectives(int processingDirectives)
        {
            if ((processingDirectives < 0) || (processingDirectives > 255))
            {
                throw new ArgumentException("Provided processing directives " + processingDirectives + " which doesn't fit within one byte.");
            }
            _defaultMessageProcessingDirectives = processingDirectives;
        }

        public void writeMessage(IFudgeFieldContainer message, int taxonomyId, int version, int processingDirectives)
        {
            writeMessageEnvelope(new FudgeMsgEnvelope((FudgeMsg)message, version, processingDirectives), taxonomyId);
        }

        public void writeMessage(IFudgeFieldContainer message, int taxonomyId)
        {
            writeMessage(message, taxonomyId, getDefaultMessageVersion(), getDefaultMessageProcessingDirectives());
        }

        public void writeMessage(IFudgeFieldContainer message)
        {
            writeMessage(message, getDefaultTaxonomyId());
        }

        public void writeMessageEnvelope(FudgeMsgEnvelope envelope, int taxonomyId)
        {
            if (envelope == null) return;
            IFudgeStreamWriter writer = StreamWriter;
            if (taxonomyId != writer.CurrentTaxonomyId)
            {
                writer.CurrentTaxonomyId = taxonomyId;
            }
            int messageSize = FudgeSize.calculateMessageEnvelopeSize(writer.CurrentTaxonomy, envelope);
            writer.WriteEnvelopeHeader(envelope.ProcessingDirectives, envelope.Version, messageSize);
            writer.WriteFields(envelope.Message);
            writer.EnvelopeComplete();
        }
        public void writeMessageEnvelope(FudgeMsgEnvelope envelope)
        {
            writeMessageEnvelope(envelope, getDefaultTaxonomyId());
        }
    }
}
