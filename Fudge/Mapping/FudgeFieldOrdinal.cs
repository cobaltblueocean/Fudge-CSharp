using System;
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

namespace Fudge.Mapping
{


	/// <summary>
	/// Annotation for mapping a field within a Bean to a specific ordinal in corresponding Fudge messages.
	/// 
	/// @author Andrew Griffin
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class FudgeFieldOrdinal : System.Attribute
	{
        private short _value = default(short);
        private bool _noFieldName = false;
	  /// <summary>
	  /// Specifies the ordinal of the field within a Fudge message. By default, no ordinals will be written
	  /// unless they have been derived from a taxonomy matching the field names.
	  /// </summary>
	  public short Value
      {
          get {return _value;}
          set { _value = value; }
      }

	  /// <summary>
	  /// Indicates that the field name must be omitted from the message.
	  /// </summary>
	  public bool NoFieldName
      {
          get { return _noFieldName; }
          set { _noFieldName = value; }
      }


	}
}