﻿/* <!--
 * Copyright (C) 2009 - 2010 by OpenGamma Inc. and other contributors.
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
using System.IO;
using FudgeMessage.Util;
using System.Diagnostics;
using FudgeMessage.Taxon;
using FudgeMessage.Types;

namespace FudgeMessage.Encodings
{
    /// <summary>
    /// Provides a streaming way of reading binary streams containing messages encoded using Fudge encoding.
    /// </summary>
    /// <remarks>The Fudge Encoding Specification can be found at http://wiki.fudgemsg.org/display/FDG/Encoding+Specification</remarks>
    public class FudgeEncodedStreamReader : FudgeStreamReaderBase
    {
        // Injected Inputs:
        private BinaryReader reader;
        private readonly FudgeContext fudgeContext;
        // Runtime State:
        private readonly Stack<MessageProcessingState> processingStack = new Stack<MessageProcessingState>();
        private IFudgeTaxonomy taxonomy;
        // Set for the envelope
        private int processingDirectives;
        private int schemaVersion;
        private short? taxonomyId;
        private int envelopeSize;
        private bool eof;
        private byte? bufferedByte;

        /// <summary>
        /// Constructs a new <see cref="FudgeEncodedStreamReader"/> with a given <see cref="FudgeMessage.FudgeContext"/>.
        /// </summary>
        /// <param name="fudgeContext"><see cref="FudgeMessage.FudgeContext"/> to use for messages read from the stream.</param>
        public FudgeEncodedStreamReader(FudgeContext fudgeContext)
        {
            if (fudgeContext == null)
            {
                throw new ArgumentNullException("fudgeContext");
            }
            this.fudgeContext = fudgeContext;
        }

        /// <summary>
        /// Constructs a new <see cref="FudgeEncodedStreamReader"/> with a given <see cref="FudgeMessage.FudgeContext"/> reading from a specified <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="fudgeContext"><see cref="FudgeMessage.FudgeContext"/> to use for messages read from the stream.</param>
        /// <param name="reader"><see cref="BinaryReader"/> to read the binary data from.</param>
        public FudgeEncodedStreamReader(FudgeContext fudgeContext, BinaryReader reader)
            : this(fudgeContext)
        {
            Reset(reader);
        }

        /// <summary>
        /// Constructs a new <see cref="FudgeEncodedStreamReader"/> with a given <see cref="FudgeMessage.FudgeContext"/> reading from a specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="fudgeContext"><see cref="FudgeMessage.FudgeContext"/> to use for messages read from the stream.</param>
        /// <param name="stream"><see cref="Stream"/> to read the binary data from.</param>
        public FudgeEncodedStreamReader(FudgeContext fudgeContext, Stream stream)
            : this(fudgeContext)
        {
            Reset(stream);
        }

        /// <summary>
        /// Gets the <see cref="BinaryReader"/> used by this <see cref="FudgeEncodedStreamReader"/>.
        /// </summary>
        protected BinaryReader Reader
        {
            get
            {
                return reader;
            }
        }

        /// <summary>
        /// Reset the state of this parser for a new message.
        /// This method is primarily designed so that instances can be pooled to minimize
        /// object creation in performance sensitive code.
        /// 
        /// </summary>
        /// <param name="dataInput"></param>
        public void Reset(BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader", "Must provide a DataInput to consume data from.");
            }
            this.reader = reader;
            CurrentElement = FudgeStreamElement.NoElement;
            processingStack.Clear();

            processingDirectives = 0;
            schemaVersion = 0;
            taxonomyId = 0;
            envelopeSize = 0;

            FieldType = null;
            FieldOrdinal = null;
            FieldName = null;
            FieldValue = null;
            eof = false;
        }

        /// <summary>
        /// Resets the <see cref="FudgeEncodedStreamReader"/> to use a new input stream.
        /// </summary>
        /// <param name="inputStream"><see cref="Stream"/> providing the binary data.</param>
        public void Reset(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream", "Must provide a Stream to consume data from.");
            }
            Reset(new FudgeBinaryReader(inputStream));
        }

        /// <summary>
        /// Gets the <see cref="FudgeMessage.FudgeContext"/> used by this <see cref="FudgeEncodedStreamReader"/>.
        /// </summary>
        public override FudgeContext FudgeContext
        {
            get
            {
                return fudgeContext;
            }
        }

        /// <inheritdoc/>
        public override FudgeStreamElement MoveNext()
        {
            try
            {
                if (eof)
                {
                    // Already at the end
                    CurrentElement = FudgeStreamElement.NoElement;
                    return CurrentElement;
                }
                else if (processingStack.Count == 0)
                {
                    // Must be an envelope.
                    ConsumeMessageEnvelope();
                }
                else if (IsEndOfSubMessage())
                {
                    CurrentElement = (processingStack.Count == 0) ? FudgeStreamElement.MessageEnd : FudgeStreamElement.SubmessageFieldEnd;
                    FieldName = null;
                    FieldOrdinal = null;
                    FieldType = null;
                }
                else
                {
                    ConsumeFieldData();
                    Debug.Assert(CurrentElement != FudgeStreamElement.NoElement);
                }
            }
            catch (IOException ioe)
            {
                throw new FudgeRuntimeException("Unable to consume data", ioe);
            }
            return CurrentElement;
        }

        /// <summary>
        /// Checks to see if we're at the end of a sub-message (or message), and if so pops the processing stack
        /// </summary>
        /// <returns>True if we were at the end of a sub-message.</returns>
        protected bool IsEndOfSubMessage()
        {
            Debug.Assert(processingStack.Count > 0);

            MessageProcessingState processingState = processingStack.Peek();
            if (processingState.Consumed >= processingState.MessageSize)
            {
                processingStack.Pop();
                if (processingStack.Count > 0)
                {
                    processingStack.Peek().Consumed += processingState.Consumed;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ConsumeFieldData() //throws IOException
        {
            sbyte fieldPrefix = Reader.ReadSByte();
            int typeId = Reader.ReadByte();
            int nRead = 2;
            bool fixedWidth = FudgeFieldPrefixCodec.IsFixedWidth(fieldPrefix);
            bool hasOrdinal = FudgeFieldPrefixCodec.HasOrdinal(fieldPrefix);
            bool hasName = FudgeFieldPrefixCodec.HasName(fieldPrefix);

            short? ordinal = null;
            if (hasOrdinal)
            {
                ordinal = Reader.ReadInt16();
                nRead += 2;
            }

            string name = null;
            if (hasName)
            {
                int nameSize = Reader.ReadByte();
                nRead++;
                name = StringFieldType.ReadString(Reader, nameSize);
                nRead += nameSize;
            }
            else if (ordinal != null)
            {
                if (Taxonomy != null)
                {
                    name = Taxonomy.GetFieldName((short)ordinal.Value);
                }
            }

            FudgeFieldType type = FudgeContext.TypeDictionary.GetByTypeId(typeId);
            if (type == null)
            {
                if (fixedWidth)
                {
                    throw new FudgeRuntimeException("Unknown fixed width type " + typeId + " for field " + ordinal + ":" + name + " cannot be handled.");
                }
                type = FudgeContext.TypeDictionary.GetUnknownType(typeId);
            }

            int varSize = 0;
            if (!fixedWidth)
            {
                int varSizeBytes = FudgeFieldPrefixCodec.GetFieldWidthByteCount(fieldPrefix);
                switch (varSizeBytes)
                {
                    case 0: varSize = 0; break;
                    case 1: varSize = Reader.ReadByte(); nRead += 1; break;
                    case 2: varSize = Reader.ReadInt16(); nRead += 2; break;
                    case 4: varSize = Reader.ReadInt32(); nRead += 4; break;
                    default:
                        throw new FudgeRuntimeException("Illegal number of bytes indicated for variable width encoding: " + varSizeBytes);
                }
            }

            FieldName = name;
            FieldOrdinal = ordinal;
            FieldType = type;
            MessageProcessingState currMsgProcessingState = processingStack.Peek();
            currMsgProcessingState.Consumed += nRead;
            if (typeId == FudgeTypeDictionary.FUDGE_MSG_TYPE_ID)
            {
                CurrentElement = FudgeStreamElement.SubmessageFieldStart;
                FieldValue = null;
                MessageProcessingState subState = new MessageProcessingState();
                subState.MessageSize = varSize;
                subState.Consumed = 0;
                processingStack.Push(subState);
            }
            else
            {
                CurrentElement = FudgeStreamElement.SimpleField;
                FieldValue = ReadFieldValue(Reader, FieldType, varSize);
                if (fixedWidth)
                {
                    currMsgProcessingState.Consumed += type.FixedSize;
                }
                else
                {
                    currMsgProcessingState.Consumed += varSize;
                }
            }
        }

        private static object ReadFieldValue(
            BinaryReader br,
            FudgeFieldType type,
            int varSize) //throws IOException
        {
            Debug.Assert(type != null);
            Debug.Assert(br != null);

            // Special fast-pass for known field types
            switch (type.TypeId)
            {
                case FudgeTypeDictionary.BOOLEAN_TYPE_ID:
                    return br.ReadBoolean();
                case FudgeTypeDictionary.SBYTE_TYPE_ID:
                    return br.ReadSByte();
                case FudgeTypeDictionary.SHORT_TYPE_ID:
                    return br.ReadInt16();
                case FudgeTypeDictionary.INT_TYPE_ID:
                    return br.ReadInt32();
                case FudgeTypeDictionary.LONG_TYPE_ID:
                    return br.ReadInt64();
                case FudgeTypeDictionary.FLOAT_TYPE_ID:
                    return br.ReadSingle();
                case FudgeTypeDictionary.DOUBLE_TYPE_ID:
                    return br.ReadDouble();
            }

            return type.ReadValue(br, varSize);
        }

        private byte? SafelyReadOneByte()
        {
            try
            {
                byte result = Reader.ReadByte();
                return result;
            }
            catch (EndOfStreamException)
            {
                return null;
            }
            catch (ObjectDisposedException)
            {
                // Stream closed
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ConsumeMessageEnvelope() //throws IOException
        {
            Debug.Assert(!eof);

            CurrentElement = FudgeStreamElement.MessageStart;
            if (bufferedByte.HasValue)
            {
                processingDirectives = bufferedByte.Value;
                bufferedByte = null;
            }
            else
            {
                byte? nextByte = SafelyReadOneByte();
                if (nextByte.HasValue)
                    processingDirectives = nextByte.Value;
                else
                {
                    // Hit the end of the stream
                    CurrentElement = FudgeStreamElement.NoElement;
                    eof = true;
                    return;
                }
            }
            schemaVersion = Reader.ReadByte();
            taxonomyId = Reader.ReadInt16();
            envelopeSize = Reader.ReadInt32();
            if (FudgeContext.TaxonomyResolver != null)
            {
                IFudgeTaxonomy taxonomy = FudgeContext.TaxonomyResolver.ResolveTaxonomy(taxonomyId);
                this.taxonomy = taxonomy;
            }
            MessageProcessingState processingState = new MessageProcessingState();
            processingState.Consumed = 8;
            processingState.MessageSize = envelopeSize;
            processingStack.Push(processingState);
        }

        public override void Close()
        {
            reader.Close();
        }

        /// <inheritdoc/>
        public override bool HasNext
        {
            get
            {
                if (eof)
                    return false;
                if (processingStack.Count > 0 || bufferedByte.HasValue)
                    return true;

                // We can't be in a message, so read a byte ahead to see if we have another envelope
                bufferedByte = SafelyReadOneByte();
                if (bufferedByte == null)
                    eof = true;

                return (bufferedByte.HasValue);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>the processingDirectives</returns>
        public override int ProcessingDirectives
        {
            get
            {
                return processingDirectives;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>the schemaVersion</returns>
        public override int SchemaVersion
        {
            get
            {
                return schemaVersion;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>the taxonomy</returns>
        public override short? TaxonomyId
        {
            get
            {
                return taxonomyId;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>the envelopeSize</returns>
        public int EnvelopeSize
        {
            get
            {
                return envelopeSize;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>the taxonomy</returns>
        public override IFudgeTaxonomy Taxonomy
        {
            get
            {
                return taxonomy;
            }
        }

        private class MessageProcessingState
        {
            public int MessageSize;
            public int Consumed;
        }
    }
}
