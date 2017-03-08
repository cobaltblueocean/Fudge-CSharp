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


	/// <summary>
	/// The wire type definition for a double.
	/// </summary>
	internal sealed class DoubleWireType : FudgeWireType
	{

	  /// <summary>
	  /// Standard Fudge field type: double.
	  /// See <seealso cref="FudgeWireType#DOUBLE_TYPE_ID"/>.
	  /// </summary>
	  public static readonly DoubleWireType Instance = new DoubleWireType();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private DoubleWireType() : base(FudgeWireType.DOUBLE_TYPE_ID, typeof(double), 8)
	  {
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public Double ReadValue(java.io.BinaryReader input, int dataSize) throws java.io.IOException
	  public double? ReadValue(BinaryReader input, int dataSize)
	  {
          return (Double?)(Object)ReadDynamicValue(input, dataSize);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void WriteValue(java.io.BinaryWriter output, Object value) throws java.io.IOException
	  public override void WriteValue(BinaryWriter output, object value)
	  {
		output.Write((double) value);
	  }


      public override int GetVariableSize(object value, Taxon.IFudgeTaxonomy taxonomy)
      {
          return base.FixedSize;
      }

      public override dynamic ReadDynamicValue(BinaryReader input, int dataSize)
      {
          return input.ReadDouble();
      }
    }

}