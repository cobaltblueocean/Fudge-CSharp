// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
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

namespace FudgeMessage.Encodings
{
    /// <summary>
    /// Tunable parameters for the JSON encoding/decoding. Please refer to <a href="http://wiki.fudgemsg.org/display/FDG/JSON+Fudge+Messages">JSON Fudge Messages</a> for details on the representation.
    /// </summary>
    public class FudgeJsonSettings
    {
        /// <summary>
        /// Default name for the processing directives field.
        /// </summary>
        public static readonly String DEFAULT_PROCESSINGDIRECTIVES_FIELD = "fudgeProcessingDirectives";

        /// <summary>
        /// Default name for the schema version field.
        /// </summary>
        public static readonly String DEFAULT_SCHEMAVERSION_FIELD = "fudgeSchemaVersion";

        /// <summary>
        /// Default name for the taxonomy field.
        /// </summary>
        public static readonly String DEFAULT_TAXONOMY_FIELD = "fudgeTaxonomy";

        private String _processingDirectivesField = DEFAULT_PROCESSINGDIRECTIVES_FIELD;
        private String _schemaVersionField = DEFAULT_SCHEMAVERSION_FIELD;
        private String _taxonomyField = DEFAULT_TAXONOMY_FIELD;
        private Boolean _preferFieldNames = true;

        /// <summary>
        /// Creates a new settings object with the default values.
        /// </summary>
        public FudgeJsonSettings()
        {
        }

        /// <summary>
        /// Creates a new settings object copying the current values from another.
        /// </summary>
        /// <param name="copy">object to copy the settings from</param>
        public FudgeJsonSettings(FudgeJsonSettings copy)
        {
            ProcessingDirectivesField = copy.ProcessingDirectivesField;
            SchemaVersionField = copy.SchemaVersionField;
            TaxonomyField = copy.TaxonomyField;
        }

        /// <summary>
        /// Name for the processing directives field.
        /// </summary>
        public String ProcessingDirectivesField
        {
            get { return _processingDirectivesField; }
            set { _processingDirectivesField = value; }
        }

        /// <summary>
        /// Name for the schema version field.
        /// </summary>
        public String SchemaVersionField
        {
            get { return _schemaVersionField; }
            set { _schemaVersionField = value; }
        }

        /// <summary>
        /// Name for the taxonomy field.
        /// </summary>
        public String TaxonomyField
        {
            get { return _taxonomyField; }
            set { TaxonomyField = value; }
        }

        /// <summary>
        /// Setting for the Prefer Field Names.
        /// </summary>
        public Boolean PreferFieldNames
        {
            get { return _preferFieldNames; }
            set { _preferFieldNames = value; }
        }
    }
}
