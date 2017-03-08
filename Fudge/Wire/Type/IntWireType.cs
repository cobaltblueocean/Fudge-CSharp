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
	/// The wire type definition for a int.
	/// </summary>
	internal sealed class IntWireType : FudgeWireType
	{

	  /// <summary>
	  /// Standard Fudge field type: int.
	  /// See <seealso cref="FudgeWireType#INT_TYPE_ID"/>.
	  /// </summary>
	  public static readonly IntWireType Instance = new IntWireType();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private IntWireType() : base(FudgeWireType.INT_TYPE_ID, typeof(int), 4)
	  {
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public Integer ReadValue(java.io.BinaryReader input, int dataSize) throws java.io.IOException
	  public int? ReadValue(BinaryReader input, int dataSize)
	  {
		return input.ReadInt32();
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void WriteValue(java.io.BinaryWriter output, Object value) throws java.io.IOException
	  public override void WriteValue(BinaryWriter output, object value)
	  {
		output.Write((int) value);
	  }


      public override int GetVariableSize(object value, Taxon.IFudgeTaxonomy taxonomy)
      {
          return base.FixedSize;
      }

      public override dynamic ReadDynamicValue(BinaryReader input, int dataSize)
      {
          return input.ReadInt32();
      }
    }

}