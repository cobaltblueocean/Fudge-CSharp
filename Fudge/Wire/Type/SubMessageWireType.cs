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
	/// The type definition for a sub-message in a hierarchical message format.
	/// </summary>
	internal class SubMessageWireType : FudgeWireType
	{

	  /// <summary>
	  /// Standard Fudge field type: embedded sub-message.
	  /// See <seealso cref="FudgeWireType#SUB_MESSAGE_TYPE_ID"/>.
	  /// </summary>
	  public static readonly SubMessageWireType Instance = new SubMessageWireType();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private SubMessageWireType() : base(FudgeWireType.SUB_MESSAGE_TYPE_ID, typeof(FudgeMsg))
	  {
	  }

	  //-------------------------------------------------------------------------
	  public int getSize(object value, FudgeTaxonomy taxonomy)
	  {
		FudgeMsg data = (FudgeMsg) value;
		if (value is FudgeEncoded)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.wire.FudgeEncoded fudgeEncoded = (org.fudgemsg.wire.FudgeEncoded) value;
		  FudgeEncoded fudgeEncoded = (FudgeEncoded) value;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final byte[] encoded = fudgeEncoded.getFudgeEncoded();
		  sbyte[] encoded = fudgeEncoded.GetFudgeEncoded();
		  if (encoded != null)
		  {
			return encoded.Length;
		  }
		}
		return FudgeSize.calculateMessageSize(taxonomy, data);
	  }

	  public FudgeMsg ReadValue(BinaryReader input, int dataSize)
	  {
		throw new System.NotSupportedException("Sub-messages can only be decoded from FudgeStreamReader");
	  }

	  public override void WriteValue(BinaryWriter output, object value)
	  {
		throw new System.NotSupportedException("Sub-messages can only be written using FudgeStreamWriter");
	  }


      public override int GetVariableSize(object value, FudgeTaxonomy taxonomy)
      {
          throw new System.NotSupportedException("Sub-messages's size can only be get from FudgeStreamReader");
      }

      public override dynamic ReadDynamicValue(BinaryReader input, int dataSize)
      {
          throw new System.NotSupportedException("Sub-messages can only be decoded from FudgeStreamReader");
      }
    }

}