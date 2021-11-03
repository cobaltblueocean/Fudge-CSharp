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

namespace FudgeMessage.Serialization
{
    /// <summary>
    /// FudgeElementNames Description
    /// </summary>
    public static class FudgeElementNames
    {
        /// <summary>
        /// Default element Name for the outer envelope tag.
        /// </summary>
        public static String DEFAULT_ENVELOPE_ELEMENT = "fudgeEnvelope";

        /// <summary>
        /// Default attribute Name for the processing directives on the envelope.
        /// </summary>
        public static String DEFAULT_ENVELOPE_ATTRIBUTE_PROCESSINGDIRECTIVES = "processingDirectives";

        /// <summary>
        /// Default attribute Name for the schema version on the envelope.
        /// </summary>
        public static String DEFAULT_ENVELOPE_ATTRIBUTE_SCHEMAVERSION = "schemaVersion";

        /// <summary>
        /// Default attribute Name for the taxonomy on the envelope.
        /// </summary>
        public static String DEFAULT_ENVELOPE_ATTRIBUTE_TAXONOMY = "taxonomy";

        /// <summary>
        /// Default element Name for an anonymous or unnamed field.
        /// </summary>
        public static String DEFAULT_FIELD_ELEMENT = "fudgeField";

        /// <summary>
        /// Default attribute Name for the Name of a field.
        /// </summary>
        public static String DEFAULT_FIELD_ATTRIBUTE_NAME = "Name";

        /// <summary>
        /// Default attribute Name for the ordinal index of a field.
        /// </summary>
        public static String DEFAULT_FIELD_ATTRIBUTE_ORDINAL = "ordinal";

        /// <summary>
        /// Alternative attribute Name (not written but recognized by default) for the ordinal index of a field.
        /// </summary>
        public static String ALIAS_FIELD_ATTRIBUTE_ORDINAL_INDEX = "index";

        /// <summary>
        /// Alternative attribute Name (not written but recognized by default) for the ordinal index of a field.
        /// </summary>
        public static String ALIAS_FIELD_ATTRIBUTE_ORDINAL_KEY = "key";

        /// <summary>
        /// Default attribute Name for the type of a field.
        /// </summary>
        public static String DEFAULT_FIELD_ATTRIBUTE_TYPE = "type";

        /// <summary>
        /// Default attribute Name for the encoding of a field.
        /// </summary>
        public static String DEFAULT_FIELD_ATTRIBUTE_ENCODING = "encoding";

        /// <summary>
        /// Default value for a {@code true} Boolean field.
        /// </summary>
        public static String DEFAULT_BOOLEAN_TRUE = "true";

        /// <summary>
        /// Alternative value (not written but recognized by default) for a {@code true} Boolean field.
        /// </summary>
        public static String ALIAS_BOOLEAN_TRUE_ON = "on";

        /// <summary>
        /// Alternative value (not written but recognized by default) for a {@code true} Boolean field.
        /// </summary>
        public static String ALIAS_BOOLEAN_TRUE_T = "T";

        /// <summary>
        /// Alternative value (not written but recognized by default) for a {@code true} Boolean field.
        /// </summary>
        public static String ALIAS_BOOLEAN_TRUE_1 = "1";

        /// <summary>
        /// Default value for a {@code false} Boolean field.
        /// </summary>
        public static String DEFAULT_BOOLEAN_FALSE = "false";

        /// <summary>
        /// Alternative value (not written but recognized by default) for a {@code false} Boolean field.
        /// </summary>
        public static String ALIAS_BOOLEAN_FALSE_OFF = "off";

        /// <summary>
        /// Alternative value (not written but recognized by default) for a {@code false} Boolean field.
        /// </summary>
        public static String ALIAS_BOOLEAN_FALSE_F = "F";

        /// <summary>
        /// Alternative value (not written but recognized by default) for a {@code false} Boolean field.
        /// </summary>
        public static String ALIAS_BOOLEAN_FALSE_0 = "0";

        /// <summary>
        /// Default value for base-64 encoded data.
        /// </summary>
        public static String DEFAULT_ENCODING_BASE64 = "base64";

    }
}
