﻿/// <summary>
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
using System.Text;

namespace Fudge.Wire.Types
{


	using FudgeTaxonomy = Fudge.Taxon.IFudgeTaxonomy;

	/// <summary>
	/// The wire type definition for a UTF-8 encoded string.
	/// </summary>
	internal sealed class StringWireType : FudgeWireType
	{
        UTF8Encoding UTF8 = new UTF8Encoding();

	  /// <summary>
	  /// Standard Fudge field type: string.
	  /// See <seealso cref="FudgeWireType#STRING_TYPE_ID"/>.
	  /// </summary>
	  public static readonly StringWireType Instance = new StringWireType();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private StringWireType() : base(FudgeWireType.STRING_TYPE_ID, typeof(string))
	  {
	  }

	  //-------------------------------------------------------------------------
	  public int getSize(object value, FudgeTaxonomy taxonomy)
	  {
		string data = (string) value;
		return UTF8.GetByteCount(data);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public String ReadValue(java.io.BinaryReader input, int dataSize) throws java.io.IOException
	  public string ReadValue(BinaryReader input, int dataSize)
	  {
          return input.ReadString();
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void WriteValue(java.io.BinaryWriter output, Object value) throws java.io.IOException
	  public override void WriteValue(BinaryWriter output, object value)
	  {
		string data = (string) value;
        output.Write(data);
	  }


      public override int GetVariableSize(object value, FudgeTaxonomy taxonomy)
      {
          return base.FixedSize;
      }

      public override dynamic ReadDynamicValue(BinaryReader input, int dataSize)
      {
          return input.ReadString();
      }
    }

}