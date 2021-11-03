/**
 * Copyright (C) 2009 - present by OpenGamma Incd and other contributors.
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
 */

using FudgeMessage.Taxon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudgeMessage
{
    /// <summary>
    /// Abstract implementation of a {@code FudgeStreamWriter} that detects major state changes and invokes
    /// other methodsd Can be used to build alternative stream writers for converting streamed Fudge messages
    /// to XML, JSON or other formats.
    /// 
    /// @author Andrew Griffin
    /// </summary>
    public abstract class AlternativeFudgeStreamWriter : IFudgeStreamWriter
    {
        public FudgeContext FudgeContext
        {
            get
            {
                return _fudgeContext;
            }
            set
            {
                _fudgeContext = value;
            }
        }

        public IFudgeTaxonomy Taxonomy
        {
            get { return _taxonomy; }
            set { _taxonomy = value; }
        }

        public short? TaxonomyId {
            get { return _taxonomyId; }
            set {
                _taxonomyId = value;
                if (_fudgeContext.TaxonomyResolver != null)
                {
                    var taxonomy = _fudgeContext.TaxonomyResolver.ResolveTaxonomy((short)value);
                    _taxonomy = taxonomy;
                }
                else
                {
                    _taxonomy = null;
                }
            }
        }

        private FudgeContext _fudgeContext;
        private IFudgeTaxonomy _taxonomy = null;
        private short? _taxonomyId = 0;

        /// <summary>
        /// Creates a new {@link AlternativeFudgeStreamWriter} instance.
        /// 
        /// </summary>
        /// <param Name="fudgeContext">the associated {@link FudgeContext}</param>
        protected AlternativeFudgeStreamWriter(FudgeContext fudgeContext)
        {
            _fudgeContext = fudgeContext;
        }

        public abstract void EndMessage();

        public abstract void EndSubMessage();

        public abstract void StartMessage();

        public abstract void StartSubMessage(string Name, short? ordinal);

        public void WriteEnvelopeHeader(int processingDirectives, int schemaVersion, int messageSize)
        {
            FudgeEnvelopeStart(processingDirectives, schemaVersion);
        }

        public void WriteField(string Name, short? ordinal, FudgeFieldType type, object value)
        {
            if (FudgeFieldStart(ordinal, Name, type))
            {
                if (type.TypeId == FudgeTypeDictionary.FUDGE_MSG_TYPE_ID)
                {
                    FudgeSubMessageStart();
                    WriteFields((IFudgeFieldContainer)value);
                    FudgeSubMessageEnd();
                }
                else
                {
                    FudgeFieldValue(type, value);
                }
                FudgeFieldEnd();
            }
        }

        public virtual void WriteFields(IEnumerable<IFudgeField> fields)
        {
            foreach (var field in fields) {
                WriteField(field);
            }
        }
        public virtual void WriteField(IFudgeField field)
        {
            if (field == null)
            {
                throw new NullReferenceException("Cannot write a null field to a Fudge stream");
            }
            WriteField(field.Name, field.Ordinal, field.Type, field.Value);
        }

        /// <summary>
        /// Called when a Fudge message envelope is starting.
        /// 
        /// </summary>
        /// <param Name="processingDirectives">the envelope processing directives</param>
        /// <param Name="schemaVersion">the envelope schema version</param>
        protected abstract void FudgeEnvelopeStart(int processingDirectives, int schemaVersion);

        /// <summary>
        /// Called at the end of the envelope after all fields have been processed.
        /// </summary>
        protected abstract void FudgeEnvelopeEnd();

        /// <summary>
        /// Called as a field starts.
        /// 
        /// </summary>
        /// <param Name="ordinal">the field ordinal</param>
        /// <param Name="Name">the field Name</param>
        /// <param Name="type">the field type</param>
        /// <returns>{@code true} to continue processing the field, {@code false} to ignore it ({@link #fudgeFieldValue}, {@link #fudgeSubMessageStart}, {@link #fudgeSubMessageEnd} and {@link #fudgeFieldEnd} will not be called for this</returns>field)
        protected virtual Boolean FudgeFieldStart(short? ordinal, String Name, FudgeFieldType type)
        {
            return true;
        }

        /// <summary>
        /// Called after a field has been processed.
        /// </summary>
        protected abstract void FudgeFieldEnd();

        /// <summary>
        /// Called between {@link #fudgeFieldStart} and {@link #fudgeFieldEnd} for fields that are not sub messages.
        /// 
        /// </summary>
        /// <param Name="type">the field type</param>
        /// <param Name="fieldValue">the value</param>
        protected abstract void FudgeFieldValue(FudgeFieldType type, Object fieldValue);

        /// <summary>
        /// Called after {@link #fudgeFieldStart} when a sub-message is starting.
        /// </summary>
        protected abstract void FudgeSubMessageStart();

        /// <summary>
        /// Called when a sub-message has been processed, before {@link #fudgeFieldEnd} is called for the field.
        /// </summary>
        protected abstract void FudgeSubMessageEnd();
        public abstract void EnvelopeComplete();
        public abstract void Flush();
        public abstract void Close();
    }
}
