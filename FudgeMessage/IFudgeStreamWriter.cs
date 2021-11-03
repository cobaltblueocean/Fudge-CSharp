/*
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
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FudgeMessage.Taxon;

namespace FudgeMessage
{
    /// <summary>
    /// <c>IFudgeStreamWriter</c> is implemented by classes wishing to write data streams (e.g. Fudge binary encoding, XML, JSON) of Fudge messages.
    /// </summary>
    /// <remarks>
    /// The <see cref="Fudge.Encodings"/> namespace contains a variety of readers and writers for different file formats.
    /// </remarks>
    public interface IFudgeStreamWriter
    {
        /// <summary>
        /// Starts a new top-level message.
        /// </summary>
        void StartMessage();

        /// <summary>
        /// Starts a sub-message within the current message.
        /// </summary>
        /// <param name="name">Name of the field, or <c>null</c> if none.</param>
        /// <param name="ordinal">Ordinal of the field, or <c>null</c> if none.</param>
        void StartSubMessage(string name, short? ordinal);

        /// <summary>
        /// Writes a message envelope header.
        /// </summary>
        /// <param name="processingDirectives">the processing directive flags</param>
        /// <param name="schemaVersion">the schema version value</param>
        /// <param name="messageSize">the Fudge encoded size of the underlying message, including the message envelope</param>
        void WriteEnvelopeHeader(int processingDirectives, int schemaVersion, int messageSize);

        /// <summary>
        /// Writes a simple field to the data stream.
        /// </summary>
        /// <param name="name">Name of the field, or <c>null</c> if none.</param>
        /// <param name="ordinal">Ordinal of the field, or <c>null</c> if none.</param>
        /// <param name="type">Type of the field, as a <see cref="FudgeFieldType"/>, can be <c>null</c> in which case writer will determine type.</param>
        /// <param name="value">Value of the field.</param>
        void WriteField(string name, short? ordinal, FudgeFieldType type, object value);

        /// <summary>
        /// Writes multiple fields to the data stream.
        /// </summary>
        /// <param name="fields">Fields to write.</param>
        void WriteFields(IEnumerable<IFudgeField> fields);

        /// <summary>
        /// Tells the writer that the current sub-message is finished.
        /// </summary>
        void EndSubMessage();

        /// <summary>
        /// Tells the writer that we have finished with the whole top-level message.
        /// </summary>
        void EndMessage();

        /// <summary>
        /// Signal the end of the message contained within an envelope.An implementation may not need to take
        /// any action at this point as the end of the envelope can be detected based on the message size in the
        /// header.
        /// </summary>
        void EnvelopeComplete();


        /// <summary>
        /// Get the bound <see cref="FudgeContext"/> used for type and taxonomy resolution.
        /// </summary>
        FudgeContext FudgeContext { get; }

        /// <summary>
        /// Gets the <see cref="IFudgeTaxonomy"/> (if any) that is currently being used to encode fields. Returns null
        /// if no taxonomy is specified or the taxonomy identifier cannot be resolved by the bound <see cref="FudgeContext"/>.
        /// </summary>
        IFudgeTaxonomy Taxonomy { get; }

        /// <summary>
        /// Gets or sets the current taxonomy identifier.
        /// </summary>
        short? TaxonomyId { get; set; }


        /// <summary>
        /// Flushes any data from the internal buffers to the target stream and attempts to flush the underlying stream if appropriate.
        /// </summary>
        void Flush();

        /// <summary>
        /// Flushes and closes this writer and attempts to close the underlying stream if appropriate.
        /// </summary>
        void Close();

    }
}
