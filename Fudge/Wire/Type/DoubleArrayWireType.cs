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
	/// The type definition for an array of double-precision floating point numbers.
	/// </summary>
	internal sealed class DoubleArrayWireType : FudgeWireType
	{

	  /// <summary>
	  /// Standard Fudge field type: arbitrary length 64-bit floating point array.
	  /// See <seealso cref="FudgeWireType#DOUBLE_ARRAY_TYPE_ID"/>.
	  /// </summary>
	  public static readonly DoubleArrayWireType Instance = new DoubleArrayWireType();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private DoubleArrayWireType() : base(FudgeWireType.DOUBLE_ARRAY_TYPE_ID, typeof(double[]))
	  {
	  }

	  //-------------------------------------------------------------------------
	  public int getSize(object value, FudgeTaxonomy taxonomy)
	  {
		double[] data = (double[]) value;
		return data.Length * 8;
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public double[] ReadValue(java.io.BinaryReader input, int dataSize) throws java.io.IOException
	  public double[] ReadValue(BinaryReader input, int dataSize)
	  {
          return (double[])(Object)ReadDynamicValue(input, dataSize);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void WriteValue(java.io.BinaryWriter output, Object value) throws java.io.IOException
	  public override void WriteValue(BinaryWriter output, object value)
	  {
		double[] data = (double[]) value;
		foreach (double d in data)
		{
		  output.Write(d);
		}
	  }


      public override int GetVariableSize(object value, FudgeTaxonomy taxonomy)
      {
          return base.FixedSize;
      }

      public override dynamic ReadDynamicValue(BinaryReader input, int dataSize)
      {
          //Reading this in one go is faster, but increases memory requirement by x2. Should do it in buffered chunks
          byte[] bytes = new byte[dataSize];
          input.Read(bytes, 0, dataSize);

          int nDoubles = dataSize / 8;
          double[] result = new double[nDoubles];
          for (int i = 0; i < nDoubles; i++)
          {
              long l = LongArrayWireType.readLong((sbyte[])(Object)bytes, (i * 8));
              result[i] = BitConverter.Int64BitsToDouble(l);
          }
          return result;
      }
    }

}