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
	/// The type definition for an unknown wire type.
	/// <para>
	/// The type system of Fudge is extensible, with a total of 255 types available,
	/// but less than 32 reserved as standard types at present.
	/// Application types should be allocated from 255 downwards to avoid clashes.
	/// </para>
	/// <para>
	/// A field can be processed as a raw byte array even when the type is not recognized.
	/// </para>
	/// </summary>
	internal sealed class UnknownWireType : FudgeWireType
	{

	  /// <summary>
	  /// Creates a new variable width wire type.
	  /// </summary>
	  /// <param name="typeId">  the wire type identifier </param>
	  internal UnknownWireType(int typeId) : base(typeId, typeof(UnknownFudgeFieldValue))
	  {
	  }

	  /// <summary>
	  /// Creates a new fixed width wire type.
	  /// </summary>
	  /// <param name="typeId">  the wire type identifier </param>
	  /// <param name="fixedWidth">  the fixed width </param>
	  internal UnknownWireType(int typeId, int fixedWidth) : base(typeId, typeof(UnknownFudgeFieldValue), fixedWidth)
	  {
	  }

	  //-------------------------------------------------------------------------
	  public int getSize(object value, FudgeTaxonomy taxonomy)
	  {
		UnknownFudgeFieldValue data = (UnknownFudgeFieldValue) value;
		return data.Contents.Length;
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public org.fudgemsg.UnknownFudgeFieldValue ReadValue(java.io.BinaryReader input, int dataSize) throws java.io.IOException
	  public UnknownFudgeFieldValue ReadValue(BinaryReader input, int dataSize)
	  {
          return (UnknownFudgeFieldValue)(Object)ReadDynamicValue(input, dataSize);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void WriteValue(java.io.BinaryWriter output, Object value) throws java.io.IOException
	  public override void WriteValue(BinaryWriter output, object value)
	  {
		UnknownFudgeFieldValue data = (UnknownFudgeFieldValue) value;
		output.Write(data.Contents);
	  }


      public override int GetVariableSize(object value, FudgeTaxonomy taxonomy)
      {
          return base.FixedSize;
      }

      public override dynamic ReadDynamicValue(BinaryReader input, int dataSize)
      {
          byte[] contents = new byte[dataSize];
          input.Read(contents, 0, dataSize);
          return new UnknownFudgeFieldValue(contents, this);
      }
    }

}