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
	/// The type definition for byte arrays.
	/// </summary>
	internal sealed class ByteArrayWireTypes : FudgeWireType
    {
        #region "Static Members"
        /// <summary>
	  /// Standard Fudge field type: arbitrary length byte array.
	  /// See <seealso cref="FudgeWireType#BYTE_ARRAY_TYPE_ID"/>.
	  /// </summary>
	  public static readonly ByteArrayWireTypes VARIABLE_SIZED_INSTANCE = new ByteArrayWireTypes();
	  /// <summary>
	  /// Standard Fudge field type: byte array of length 4.
	  /// See <seealso cref="FudgeWireType#BYTE_ARRAY_4_TYPE_ID"/>.
	  /// </summary>
	  public static readonly ByteArrayWireTypes LENGTH_4_INSTANCE = new ByteArrayWireTypes(FudgeWireType.BYTE_ARRAY_4_TYPE_ID, 4);
	  /// <summary>
	  /// Standard Fudge field type: byte array of length 8.
	  /// See <seealso cref="FudgeWireType#BYTE_ARRAY_8_TYPE_ID"/>.
	  /// </summary>
	  public static readonly ByteArrayWireTypes LENGTH_8_INSTANCE = new ByteArrayWireTypes(FudgeWireType.BYTE_ARRAY_8_TYPE_ID, 8);
	  /// <summary>
	  /// Standard Fudge field type: byte array of length 16.
	  /// See <seealso cref="FudgeWireType#BYTE_ARRAY_16_TYPE_ID"/>.
	  /// </summary>
	  public static readonly ByteArrayWireTypes LENGTH_16_INSTANCE = new ByteArrayWireTypes(FudgeWireType.BYTE_ARRAY_16_TYPE_ID, 16);
	  /// <summary>
	  /// Standard Fudge field type: byte array of length 20.
	  /// See <seealso cref="FudgeWireType#BYTE_ARRAY_20_TYPE_ID"/>.
	  /// </summary>
	  public static readonly ByteArrayWireTypes LENGTH_20_INSTANCE = new ByteArrayWireTypes(FudgeWireType.BYTE_ARRAY_20_TYPE_ID, 20);
	  /// <summary>
	  /// Standard Fudge field type: byte array of length 32.
	  /// See <seealso cref="FudgeWireType#BYTE_ARRAY_32_TYPE_ID"/>.
	  /// </summary>
	  public static readonly ByteArrayWireTypes LENGTH_32_INSTANCE = new ByteArrayWireTypes(FudgeWireType.BYTE_ARRAY_32_TYPE_ID, 32);
	  /// <summary>
	  /// Standard Fudge field type: byte array of length 64.
	  /// See <seealso cref="FudgeWireType#BYTE_ARRAY_64_TYPE_ID"/>.
	  /// </summary>
	  public static readonly ByteArrayWireTypes LENGTH_64_INSTANCE = new ByteArrayWireTypes(FudgeWireType.BYTE_ARRAY_64_TYPE_ID, 64);
	  /// <summary>
	  /// Standard Fudge field type: byte array of length 128.
	  /// See <seealso cref="FudgeWireType#BYTE_ARRAY_128_TYPE_ID"/>.
	  /// </summary>
	  public static readonly ByteArrayWireTypes LENGTH_128_INSTANCE = new ByteArrayWireTypes(FudgeWireType.BYTE_ARRAY_128_TYPE_ID, 128);
	  /// <summary>
	  /// Standard Fudge field type: byte array of length 256.
	  /// See <seealso cref="FudgeWireType#BYTE_ARRAY_256_TYPE_ID"/>.
	  /// </summary>
	  public static readonly ByteArrayWireTypes LENGTH_256_INSTANCE = new ByteArrayWireTypes(FudgeWireType.BYTE_ARRAY_256_TYPE_ID, 256);
	  /// <summary>
	  /// Standard Fudge field type: byte array of length 512.
	  /// See <seealso cref="FudgeWireType#BYTE_ARRAY_512_TYPE_ID"/>.
	  /// </summary>
	  public static readonly ByteArrayWireTypes LENGTH_512_INSTANCE = new ByteArrayWireTypes(FudgeWireType.BYTE_ARRAY_512_TYPE_ID, 512);

        #endregion

      /// <summary>
	  /// Restricted constructor for variable width.
	  /// </summary>
	  private ByteArrayWireTypes() : base(FudgeWireType.BYTE_ARRAY_TYPE_ID, typeof(sbyte[]))
	  {
	  }

	  /// <summary>
	  /// Restricted constructor for fixed widths.
	  /// </summary>
	  private ByteArrayWireTypes(sbyte typeId, int length) : base(typeId, typeof(sbyte[]), length)
	  {
	  }

	  //-------------------------------------------------------------------------
	  public override int GetVariableSize(object value, FudgeTaxonomy taxonomy)
	  {
		return ((sbyte[]) value).Length;
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public byte[] ReadValue(java.io.BinaryReader input, int dataSize) throws java.io.IOException
      public override dynamic ReadDynamicValue(BinaryReader input, int dataSize)
	  {
		if (!IsVariableSize)
		{
		  dataSize = FixedSize;
		}
        byte[] buf = new byte[dataSize];

		input.Read(buf, 0, dataSize);
        return Array.ConvertAll(buf, b => unchecked((sbyte)b));
	  }

      public sbyte[] ReadValue(BinaryReader input, int dataSize)
      {
          return (sbyte[])(Object)ReadDynamicValue(input, dataSize);
      }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void WriteValue(java.io.BinaryWriter output, Object value) throws java.io.IOException
	  public override void WriteValue(BinaryWriter output, object value)
	  {
		sbyte[] bytes = (sbyte[]) value;
		if (!IsVariableSize)
		{
		  if (bytes.Length != FixedSize)
		  {
			throw new System.ArgumentException("Used fixed size type of size " + FixedSize + " but passed array of size " + bytes.Length);
		  }
		}
        output.Write(Array.ConvertAll(bytes, b => unchecked((byte)b)));
	  }

	}

}