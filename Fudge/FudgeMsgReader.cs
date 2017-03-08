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
using Fudge.Types;

namespace Fudge
{
    /// <summary>
    /// A reader for returning whole Fudge messages (FudgeFieldContainer instances) from an underlying FudgeStreamReader instance.
    /// This implementation constructs the whole Fudge message in memory before returning to the caller. This is provided for convenience - greater
    /// runtime efficiency may be possible by working directly with the {@link FudgeStreamReader} to process stream elements as they are decoded.
    /// 
    ///  @author Andrew Griffin
    /// </summary>
    public class FudgeMsgReader 
    {
        private IFudgeStreamReader _streamReader;

        private FudgeMsgEnvelope _currentEnvelope = null;

        public FudgeMsgReader(IFudgeStreamReader streamReader)
        {
            if (streamReader == null)
            {
                throw new NullReferenceException("streamReader cannot be null");
            }
            _streamReader = streamReader;
        }

        public void close()
        {
            if (_streamReader == null) return;
            _streamReader.Close();
        }

        public FudgeContext FudgeContext
        {
            get{
                IFudgeStreamReader reader = StreamReader;
                if (reader == null) return null;
                return reader.FudgeContext;
            }
        }

        protected IFudgeStreamReader StreamReader
        {
            get{
                return _streamReader;
            }
        }

        public Boolean HasNext()
        {
            if (_currentEnvelope != null) return true;
            _currentEnvelope = ReadMessageEnvelope();
            return (_currentEnvelope != null);
        }

        public IFudgeFieldContainer NextMessage()
        {
            FudgeMsgEnvelope msgEnv = NextMessageEnvelope();
            if (msgEnv == null) return null;
            return msgEnv.Message;
        }


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
            ProcessFields((IMutableFudgeFieldContainer)msgEnv.Message);
            return msgEnv;
        }

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
            if (element != FudgeStreamElement.NoElement)
            {
                throw new ArgumentException("First element in encoding stream wasn't a message element.");
            }
            IMutableFudgeFieldContainer msg = FudgeContext.NewMessage();
            FudgeMsgEnvelope envelope = new FudgeMsgEnvelope((FudgeMsg)msg, StreamReader.SchemaVersion, StreamReader.ProcessingDirectives);
            return envelope;
        }

        protected void ProcessFields(IMutableFudgeFieldContainer msg)
        {
            IFudgeStreamReader reader = StreamReader;
            while (reader.HasNext)
            {
                FudgeStreamElement element = reader.MoveNext();
                switch (element)
                {
                    case FudgeStreamElement.SimpleField:
                        msg.Add(reader.FieldName, reader.FieldOrdinal, reader.FieldType, reader.FieldValue);
                        break;
                    case FudgeStreamElement.SubmessageFieldStart:
                        IMutableFudgeFieldContainer subMsg = FudgeContext.NewMessage();
                        msg.Add(reader.FieldName, reader.FieldOrdinal, FudgeMsgFieldType.Instance, subMsg);
                        ProcessFields(subMsg);
                        break;
                    case FudgeStreamElement.SubmessageFieldEnd:
                        return;
                }
            }
        }
    }
}
    
