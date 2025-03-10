﻿/*
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
using System.Xml;
using FudgeMessage.Types;
using System.IO;
using FudgeMessage.Taxon;
using FudgeMessage.Serialization;

namespace FudgeMessage.Encodings
{
    /// <summary>
    /// <c>FudgeXmlStreamWriter</c> allows Fudge messages to be output as XML.
    /// </summary>
    public class FudgeXmlStreamWriter : IFudgeStreamWriter
    {
        /// <summary>Set this property in the <see cref="FudgeContext"/> to give the default behaviour for flushing messages when they are complete.</summary>
        public static readonly FudgeContextProperty AutoFlushOnMessageEndProperty = new FudgeContextProperty("FudgeXmlStreamWriter.AutoFlushOnMessageEnd", typeof(bool));
        private readonly FudgeContext context;
        private readonly XmlWriter writer;
        private readonly string outerElementName;

        /// <summary>
        /// Constructs a new <c>FudgeXmlStreamWriter</c>, outputting to a given <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="context">Context to control behaviours.</param>
        /// <param name="writer"><see cref="XmlWriter"/> to use to output XML.</param>
        /// <param name="outerElementName">The name of the XML element used for outermost messages.</param>
        public FudgeXmlStreamWriter(FudgeContext context, XmlWriter writer, string outerElementName)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (outerElementName == null)
                throw new ArgumentNullException("outerElementName");

            this.context = context;
            this.writer = writer;
            this.outerElementName = outerElementName;
            this.AutoFlushOnMessageEnd = (bool)context.GetProperty(AutoFlushOnMessageEndProperty, true);
        }

        /// <summary>
        /// Constructs a new <c>FudgeXmlStreamWriter</c>, outputting to a given <see cref="Stream"/>.
        /// </summary>
        /// <param name="context">Context to control behaviours.</param>
        /// <param name="stream"><see cref="Stream"/> to use to output XML.</param>
        /// <param name="outerElementName">The name of the XML element used for outermost messages.</param>
        /// <remarks>
        /// if you want greater control over the way the XML is written, use the constructor that takes an <see cref="XmlWriter"/>.
        /// </remarks>
        public FudgeXmlStreamWriter(FudgeContext context, Stream stream, string outerElementName)
            : this(context, XmlWriter.Create(stream, new XmlWriterSettings { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment }), outerElementName)
        {
        }

        /// <summary>
        /// If true, the <c>FudgeXmlStreamWriter</c> will flush the <see cref="XmlWriter"/> whenever a top-level message is completed.
        /// </summary>
        public bool AutoFlushOnMessageEnd { get; set; }

        public FudgeContext FudgeContext
        {
            get { return context; }
        }

        public IFudgeTaxonomy Taxonomy
        {
            get { return context.TaxonomyResolver.ResolveTaxonomy(TaxonomyId); }
        }

        public string EnvelopElementName { get; set; }

        public short? TaxonomyId { get ; set ; }

        #region IFudgeStreamWriter Members

        /// <inheritdoc/>
        public void StartMessage()
        {
            writer.WriteStartElement(outerElementName);
        }

        /// <inheritdoc/>
        public void StartSubMessage(string name, short? ordinal)
        {
            writer.WriteStartElement(name);
        }

        /// <inheritdoc/>
        public void WriteField(string name, short? ordinal, FudgeFieldType type, object value)
        {
            writer.WriteStartElement(name);
            if (type != IndicatorFieldType.Instance)
            {
                writer.WriteValue(value);
            }
            writer.WriteEndElement();
        }

        /// <inheritdoc/>
        public void WriteFields(IEnumerable<IFudgeField> fields)
        {
            foreach (var field in fields)
            {
                if (field.Type == FudgeMsgFieldType.Instance)
                {
                    StartSubMessage(field.Name, field.Ordinal);
                    WriteFields((FudgeMsg)field.Value);
                    EndSubMessage();
                }
                else
                {
                    WriteField(field.Name, field.Ordinal, field.Type, field.Value);
                }
            }
        }

        /// <inheritdoc/>
        public void EndSubMessage()
        {
            writer.WriteEndElement();
        }

        /// <inheritdoc/>
        public void EndMessage()
        {
            writer.WriteEndElement();

            if (AutoFlushOnMessageEnd)
                writer.Flush();
        }

        public void WriteEnvelopeHeader(int processingDirectives, int schemaVersion, int messageSize)
        {
            if (EnvelopElementName != null)
            {
                var intFieldType = new FudgeFieldType<int>(FudgeTypeDictionary.INT_TYPE_ID, false, 0);

                writer.WriteStartElement(EnvelopElementName);
                if ((processingDirectives != 0) && (FudgeElementNames.DEFAULT_ENVELOPE_ATTRIBUTE_PROCESSINGDIRECTIVES != null))
                {
                    writer.WriteAttributeString(FudgeElementNames.DEFAULT_ENVELOPE_ATTRIBUTE_PROCESSINGDIRECTIVES, processingDirectives.ToString());
                }
                if ((schemaVersion != 0) && (FudgeElementNames.DEFAULT_ENVELOPE_ATTRIBUTE_SCHEMAVERSION != null))
                {
                    writer.WriteAttributeString(FudgeElementNames.DEFAULT_ENVELOPE_ATTRIBUTE_SCHEMAVERSION, schemaVersion.ToString());
                }
                if ((TaxonomyId.HasValue) && (TaxonomyId.Value != 0) && (FudgeElementNames.DEFAULT_ENVELOPE_ATTRIBUTE_TAXONOMY != null))
                {
                    writer.WriteAttributeString(FudgeElementNames.DEFAULT_ENVELOPE_ATTRIBUTE_TAXONOMY, TaxonomyId.Value.ToString());
                }

                writer.WriteEndElement();
            }
        }

        public void EnvelopeComplete()
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            writer.Flush();
        }

        public void Close()
        {
            writer.Close();
        }

        #endregion
    }
}
