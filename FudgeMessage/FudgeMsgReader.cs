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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudgeMessage
{
    /// <summary>
    /// A reader for returning whole Fudge messages ({@link FudgeFieldContainer} instances) from an underlying {@link FudgeStreamReader} instance.
    /// This implementation constructs the whole Fudge message in memory before returning to the callerd This is provided for convenience - greater
    /// runtime efficiency may be possible by working directly with the {@link FudgeStreamReader} to process stream elements as they are decoded.
    /// 
    /// @author Andrew Griffin
    /// </summary>
    public class FudgeMsgReader : FudgeStreamParser
    {

        /// <summary>
        /// The underlying source of Fudge elements.
        /// </summary>
        private IFudgeStreamReader _streamReader;

        /// <summary>
        /// An envelope buffer for reading in the current messaged {@link #hasNext} will read the envelope header, and create this object
        /// with a {@link MutableFudgeFieldContainer} attached to itd The full call to {@link nextMessage} or {@link #nextMessageEnvelope} will
        /// process the message fields.
        /// </summary>
        private FudgeMsgEnvelope _currentEnvelope = null;

        /// <summary>
        /// Creates a new {@link FudgeMsgReader} around an existing {@link FudgeStreamReader}.
        /// 
        /// </summary>
        /// <param Name="streamReader">the source of Fudge stream elements to read</param>
        public FudgeMsgReader(IFudgeStreamReader streamReader) : base(streamReader.FudgeContext)
        {
            if (streamReader == null)
            {
                throw new NullReferenceException("streamReader cannot be null");
            }
            _streamReader = streamReader;
        }

        /// <summary>
        /// Closes this {@link FudgeMsgReader} and the underlying {@link FudgeStreamReader}.
        /// </summary>
        public void Close()
        {
            if (_streamReader == null) return;
            _streamReader.Close();
        }


        /// <summary>
        /// Returns the underlying {@link FudgeStreamReader} for this message reader.
        /// 
        /// </summary>
        /// <returns>the {@code FudgeStreamReader}</returns>
        protected IFudgeStreamReader StreamReader
        {
            get { return _streamReader; }
        }

        /// <summary>
        /// Returns true if there are more messages to read from the underlying source.
        /// 
        /// </summary>
        /// <returns>{@code true} if {@link #nextMessage()} or {@link #nextMessageEnvelope()} will return data</returns>
        public Boolean HasNext()
        {
            if (_currentEnvelope != null) return true;
            _currentEnvelope = ReadMessageEnvelope();
            return (_currentEnvelope != null);
        }

        /// <summary>
        /// Reads the next message, discarding the envelope.
        /// 
        /// </summary>
        /// <returns>the message read without the envelope</returns>
        public IFudgeFieldContainer nextMessage()
        {
            FudgeMsgEnvelope msgEnv = NextMessageEnvelope();
            if (msgEnv == null) return null;
            return msgEnv.Message;
        }

        /// <summary>
        /// Reads the next message, returning the envelope.
        /// 
        /// </summary>
        /// <returns>the {@link FudgeMsgEnvelope}</returns>
        public FudgeMsgEnvelope NextMessageEnvelope()
        {
            FudgeMsgEnvelope msgEnv;
            if (_currentEnvelope == null)
            {
                msgEnv = ReadMessageEnvelope();
                if (msgEnv == null) return null;
            }
            else
            {
                msgEnv = _currentEnvelope;
                _currentEnvelope = null;
            }
            ProcessFields(_streamReader, msgEnv.Message);
            return msgEnv;
        }

        /// <summary>
        /// Reads the next message envelope from the underlying streamd No fields are read.
        /// 
        /// </summary>
        /// <returns>the {@link FudgeMsgEnvelope} read</returns>
        protected FudgeMsgEnvelope ReadMessageEnvelope()
        {
            if (StreamReader.HasNext == false)
            {
                return null;
            }
            FudgeStreamElement element = StreamReader.MoveNext();
            if (element == null)
            {
                return null;
            }
            if (element != FudgeStreamElement.MessageEnvelope)
            {
                throw new ArgumentException("First element in encoding stream wasn't a message element.");
            }
            var msg = FudgeContext.NewMessage();
            FudgeMsgEnvelope envelope = new FudgeMsgEnvelope(msg, StreamReader.SchemaVersion, StreamReader.ProcessingDirectives);
            return envelope;
        }
    }
}
