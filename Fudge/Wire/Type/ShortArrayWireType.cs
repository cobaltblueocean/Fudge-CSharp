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


	using FudgeTaxonomy = Fudge.Taxon.IFudgeTaxonomy;

	/// <summary>
	/// Type definition for arrays of 16-bit integers.
	/// </summary>
	internal sealed class ShortArrayWireType : FudgeWireType
	{

	  /// <summary>
	  /// Standard Fudge field type: array of 16-bit integers.
	  /// See <seealso cref="FudgeWireType#SHORT_ARRAY_TYPE_ID"/>.
	  /// </summary>
	  public static readonly ShortArrayWireType Instance = new ShortArrayWireType();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private ShortArrayWireType() : base(FudgeWireType.SHORT_ARRAY_TYPE_ID, typeof(short[]))
	  {
	  }

	  //-------------------------------------------------------------------------
	  public int getSize(object value, FudgeTaxonomy taxonomy)
	  {
		short[] data = (short[]) value;
		return data.Length * 2;
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public short[] ReadValue(java.io.BinaryReader input, int dataSize) throws java.io.IOException
	  public short[] ReadValue(BinaryReader input, int dataSize)
	  {
          return (short[])(Object)ReadDynamicValue(input, dataSize);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void WriteValue(java.io.BinaryWriter output, Object value) throws java.io.IOException
	  public override void WriteValue(BinaryWriter output, object value)
	  {
		short[] data = (short[]) value;
		foreach (short f in data)
		{
		  output.Write(f);
		}
	  }


      public override int GetVariableSize(object value, FudgeTaxonomy taxonomy)
      {
          return base.FixedSize;
      }

      public override dynamic ReadDynamicValue(BinaryReader input, int dataSize)
      {
          int nShorts = dataSize / 2;
          short[] result = new short[nShorts];
          for (int i = 0; i < nShorts; i++)
          {
              result[i] = input.ReadInt16();
          }
          return result;
      }
    }

}