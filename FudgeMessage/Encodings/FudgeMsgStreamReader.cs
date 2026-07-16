/*
 * <!--
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
using FudgeMessage;
using FudgeMessage.Taxon;
using FudgeMessage.Types;

namespace FudgeMessage.Encodings
{
    /// <summary>
    /// <c>FudgeMsgStreamReader</c> allows a <see cref="FudgeMsg"/> to be read as if it were a stream source of data.
    /// </summary>
    public class FudgeMsgStreamReader : FudgeStreamReaderBase
    {
        private readonly FudgeContext context;
        private Stack<State> stack = new Stack<State>();
        private State currentState;
        private IFudgeField currentField;
        private IEnumerator<FudgeMsg> messageSource;
        private FudgeMsg nextMessage;
        private int processingDirectives = 0;
        private int schemaVersion = 0;
        private short? taxonomyId = 0;
        private IFudgeTaxonomy _taxonomy;

        /// <summary>
        /// Constructs a new <see cref="FudgeMsgStreamReader"/> using a given <see cref="FudgeMsg"/> for data.
        /// </summary>
        /// <param name="context">Context to control behaviours.</param>
        /// <param name="msg"><see cref="FudgeMsg"/> to provide as a stream.</param>
        public FudgeMsgStreamReader(FudgeContext context, FudgeMsg msg)
            : this(context, new FudgeMsg[] { msg })
        {
        }

        /// <summary>
        /// Constructs a new <see cref="FudgeMsgStreamReader"/> using a set of <see cref="FudgeMsg"/>s for data.
        /// </summary>
        /// <param name="context">Context to control behaviours.</param>
        /// <param name="messages">Set <see cref="FudgeMsg"/>s to provide as a stream.</param>
        public FudgeMsgStreamReader(FudgeContext context, IEnumerable<FudgeMsg> messages)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            this.context = context;

            messageSource = messages.GetEnumerator();
            currentState = null;

            //processingDirectives = messageSource.
            if (context.TaxonomyResolver != null)
            {
                IFudgeTaxonomy taxonomy = context.TaxonomyResolver.ResolveTaxonomy(taxonomyId);
                _taxonomy = taxonomy;
            }

        }

        #region IFudgeStreamReader Members

        /// <inheritdoc/>
        public override bool HasNext
        {
            get
            {
                if (stack.Count > 0 || currentState != null || nextMessage != null)
                    return true;

                // See if there's another message
                if (!messageSource.MoveNext())
                    return false;

                nextMessage = messageSource.Current;
                return true;
            }
        }

        /// <inheritdoc/>
        public override FudgeStreamElement MoveNext()
        {
            if (currentState == null)
            {
                if (!HasNext)       // Will fetch the next if required
                {
                    CurrentElement = FudgeStreamElement.NoElement;
                    FieldType = null;
                    FieldOrdinal = null;
                    FieldName = null;
                    FieldValue = null;
                }
                else
                {
                    currentState = new State(nextMessage);
                    nextMessage = null;
                    CurrentElement = FudgeStreamElement.MessageStart;
                    FieldType = null;
                    FieldOrdinal = null;
                    FieldName = null;
                    FieldValue = null;
                }
            }
            else if (currentState.Fields.Count == 0)
            {
                if (stack.Count == 0)
                {
                    // Finished the message
                    currentState = null;
                    CurrentElement = FudgeStreamElement.MessageEnd;
                    FieldType = null;
                    FieldOrdinal = null;
                    FieldName = null;
                    FieldValue = null;
                }
                else
                {
                    currentState = stack.Pop();
                    CurrentElement = FudgeStreamElement.SubmessageFieldEnd;
                    FieldType = null;
                    FieldOrdinal = null;
                    FieldName = null;
                    FieldValue = null;
                }
            }
            else
            {
                currentField = currentState.Fields.Dequeue();
                // Populate base class field properties for the field
                FieldType = currentField.Type;
                FieldOrdinal = currentField.Ordinal;
                FieldName = currentField.Name;
                FieldValue = currentField.Value;
                if (currentField.Type == FudgeMsgFieldType.Instance)
                {
                    stack.Push(currentState);
                    currentState = new State((FudgeMsg)currentField.Value);
                    CurrentElement = FudgeStreamElement.SubmessageFieldStart;
                }
                else
                {
                    CurrentElement = FudgeStreamElement.SimpleField;
                }
            }

            return CurrentElement;
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        // CurrentElement, FieldType, FieldOrdinal, FieldName and FieldValue are provided by the base class

        public override int ProcessingDirectives
        {
            get { return processingDirectives; }
        }

        public override int SchemaVersion
        {
            get { return schemaVersion; }
        }

        public override short? TaxonomyId
        {
            get { return taxonomyId; }
        }

        public override IFudgeTaxonomy Taxonomy
        {
            get { throw new NotImplementedException(); }
        }

        public override FudgeContext FudgeContext
        {
            get
            {
                return context;
            }
        }

        #endregion

        private class State
        {
            public readonly FudgeMsg Msg;
            public readonly Queue<IFudgeField> Fields;

            public State(FudgeMsg msg)
            {
                Msg = msg;
                Fields = new Queue<IFudgeField>(msg.GetAllFields());
            }
        }
    }
}
