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
using System.IO;

namespace Fudge.Wire.Types
{


	using IndicatorType = Fudge.Types.IndicatorType;

	/// <summary>
	/// The type definition for a message-level indicator flag.
	/// </summary>
	internal sealed class IndicatorWireType : FudgeWireType
	{

	  /// <summary>
	  /// Standard Fudge field type: zero length indicator.
	  /// See <seealso cref="FudgeWireType#INDICATOR_TYPE_ID"/>.
	  /// </summary>
	  public static readonly IndicatorWireType Instance = new IndicatorWireType();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private IndicatorWireType() : base(FudgeWireType.INDICATOR_TYPE_ID, typeof(IndicatorType), 0)
	  {
	  }

      public IndicatorType ReadValue(BinaryReader input, int dataSize)
	  {
		return IndicatorType.Instance;
	  }

      public override void WriteValue(BinaryWriter output, object value)
	  {
		// all data written in header, nothing to write here
	  }


      public override int GetVariableSize(object value, Taxon.IFudgeTaxonomy taxonomy)
      {
          return base.FixedSize;
      }

      public override dynamic ReadDynamicValue(BinaryReader input, int dataSize)
      {
          return IndicatorType.Instance;
      }
    }

}