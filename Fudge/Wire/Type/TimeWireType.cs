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


	using FudgeTime = Fudge.Types.FudgeTime;

	/// <summary>
	/// The type definition for a date.
	/// <para>
	/// This is currently backed by a <seealso cref="FudgeTime"/>.
	/// The secondary type mechanism is used to support additional Java representations,
	/// such as <seealso cref="Date"/>, <seealso cref="Calendar"/> and {@code org.threeten.bp} classes.</para>
	/// <para>
	/// For more details, please refer to <a href="http://wiki.fudgemsg.org/display/FDG/DateTime+encoding">DateTime Encoding</a>.
	/// </para>
	/// </summary>
	internal sealed class TimeWireType : FudgeWireType
	{

	  /// <summary>
	  /// Standard Fudge field type: date.
	  /// See <seealso cref="FudgeWireType#TIME_TYPE_ID"/>.
	  /// </summary>
	  public static readonly TimeWireType Instance = new TimeWireType();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private TimeWireType() : base(FudgeWireType.TIME_TYPE_ID, typeof(FudgeTime), 8)
	  {
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public Fudge.Types.FudgeTime ReadValue(java.io.BinaryReader input, int dataSize) throws java.io.IOException
	  public FudgeTime ReadValue(BinaryReader input, int dataSize)
	  {
		return DateTimeWireType.readFudgeTime(input);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void WriteValue(java.io.BinaryWriter output, Object value) throws java.io.IOException
	  public override void WriteValue(BinaryWriter output, object value)
	  {
		FudgeTime data = (FudgeTime) value;
		DateTimeWireType.writeFudgeTime(output, data);
	  }

      public override int GetVariableSize(object value, Taxon.IFudgeTaxonomy taxonomy)
      {
          return base.FixedSize;
      }

      public override dynamic ReadDynamicValue(BinaryReader input, int dataSize)
      {
          return DateTimeWireType.readFudgeTime(input);
      }
    }

}