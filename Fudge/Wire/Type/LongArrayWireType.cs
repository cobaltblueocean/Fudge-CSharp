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
	/// The type definition for arrays of 64-bit integers.
	/// </summary>
	internal sealed class LongArrayWireType : FudgeWireType
	{

	  /// <summary>
	  /// Standard Fudge field type: arrays of 64-bit integers.
	  /// See <seealso cref="FudgeWireType#LONG_ARRAY_TYPE_ID"/>.
	  /// </summary>
	  public static readonly LongArrayWireType Instance = new LongArrayWireType();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private LongArrayWireType() : base(FudgeWireType.LONG_ARRAY_TYPE_ID, typeof(long[]))
	  {
	  }

	  //-------------------------------------------------------------------------
	  public int getSize(object value, FudgeTaxonomy taxonomy)
	  {
		long[] data = (long[]) value;
		return data.Length * 8;
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public long[] ReadValue(java.io.BinaryReader input, int dataSize) throws java.io.IOException
	  public long[] ReadValue(BinaryReader input, int dataSize)
	  {
          return (long[])(Object)ReadDynamicValue(input, dataSize);
	  }

	  public static long readLong(sbyte[] buff, int offset)
	  {
		//Fudge spec requires Network Byte Order

		long a = (long) buff[0 + offset] << 56;
		long b = (long)(buff[1 + offset] & 255) << 48;
		long c = (long)(buff[2 + offset] & 255) << 40;
		long d = (long)(buff[3 + offset] & 255) << 32;
		long e = (long)(buff[4 + offset] & 255) << 24;
		long f = (buff[5 + offset] & 255) << 16;
		long g = ((buff[6 + offset] & 255) << 8);
		int h = (buff[7 + offset] & 255) << 0;
		return a + b + c + d + e + f + g + h;
	  }


//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void WriteValue(java.io.BinaryWriter output, Object value) throws java.io.IOException
	  public override void WriteValue(BinaryWriter output, object value)
	  {
		long[] data = (long[]) value;
		foreach (long l in data)
		{
		  output.Write(l);
		}
	  }


      public override int GetVariableSize(object value, FudgeTaxonomy taxonomy)
      {
          return base.FixedSize;
      }

      public override dynamic ReadDynamicValue(BinaryReader input, int dataSize)
      {
          //Reading this in one go is faster, but increases memory requirement by x2.  Should do it in buffered chunks
          byte[] bytes = new byte[dataSize];
          input.Read(bytes, 0, dataSize);

          int nDoubles = dataSize / 8;
          long[] result = new long[nDoubles];
          for (int i = 0; i < nDoubles; i++)
          {
              long l = readLong((sbyte[])(Object)bytes, i * 8);
              result[i] = l;
          }
          return result;
      }
    }

}