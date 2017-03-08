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
	/// The type definition for an array of single-precision floating point numbers.
	/// </summary>
	internal sealed class FloatArrayWireType : FudgeWireType
	{

	  /// <summary>
	  /// Standard Fudge field type: arbitrary length 32-bit floating point array.
	  /// See <seealso cref="FudgeWireType#FLOAT_ARRAY_TYPE_ID"/>.
	  /// </summary>
	  public static readonly FloatArrayWireType Instance = new FloatArrayWireType();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FloatArrayWireType() : base(FudgeWireType.FLOAT_ARRAY_TYPE_ID, typeof(float[]))
	  {
	  }

	  //-------------------------------------------------------------------------
	  public int getSize(object value, FudgeTaxonomy taxonomy)
	  {
		float[] data = (float[]) value;
		return data.Length * 4;
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public float[] ReadValue(java.io.BinaryReader input, int dataSize) throws java.io.IOException
	  public float[] ReadValue(BinaryReader input, int dataSize)
	  {
          return (float[])(Object)ReadDynamicValue(input, dataSize);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void WriteValue(java.io.BinaryWriter output, Object value) throws java.io.IOException
	  public override void WriteValue(BinaryWriter output, object value)
	  {
		float[] data = (float[]) value;
		foreach (float f in data)
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
          int nFloats = dataSize / 4;
          float[] result = new float[nFloats];
          for (int i = 0; i < nFloats; i++)
          {
              result[i] = input.ReadSingle();
          }
          return result;
      }
    }

}