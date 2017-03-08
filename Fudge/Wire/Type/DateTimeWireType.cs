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


	using DateTimeAccuracy = Fudge.Types.DateTimeAccuracy;
	using FudgeDate = Fudge.Types.FudgeDate;
	using FudgeDateTime = Fudge.Types.FudgeDateTime;
	using FudgeTime = Fudge.Types.FudgeTime;

	/// <summary>
	/// The type definition for a date.
	/// <para>
	/// This is currently backed by a <seealso cref="FudgeDateTime"/>.
	/// The secondary type mechanism is used to support additional Java representations,
	/// such as <seealso cref="Date"/>, <seealso cref="Calendar"/> and {@code org.threeten.bp} classes.</para>
	/// <para>
	/// For more details, please refer to <a href="http://wiki.fudgemsg.org/display/FDG/DateTime+encoding">DateTime Encoding</a>.
	/// </para>
	/// </summary>
	internal sealed class DateTimeWireType : FudgeWireType
	{

	  /// <summary>
	  /// Standard Fudge field type: combined date and time.
	  /// See <seealso cref="FudgeWireType#DATETIME_TYPE_ID"/>.
	  /// </summary>
	  public static readonly DateTimeWireType Instance = new DateTimeWireType();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private DateTimeWireType() : base(FudgeWireType.DATETIME_TYPE_ID, typeof(FudgeDateTime), 12)
	  {
	  }

	  /// <summary>
	  /// Reads a Fudge date representation from an input source.
	  /// </summary>
	  /// <param name="input">  the input source </param>
	  /// <returns> the date </returns>
	  /// <exception cref="IOException"> if there is an error from the input source </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: static Fudge.Types.FudgeDate readFudgeDate(final java.io.BinaryReader input) throws java.io.IOException
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
	  internal static FudgeDate readFudgeDate(BinaryReader input)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = input.readInt();
		int n = input.ReadInt32();
		return new FudgeDate(n);
	  }

	  /// <summary>
	  /// Reads a Fudge time representation from an input source.
	  /// </summary>
	  /// <param name="input"> input source </param>
	  /// <returns> the time </returns>
	  /// <exception cref="IOException"> if there is an error from the input source </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: static Fudge.Types.FudgeTime readFudgeTime(final java.io.BinaryReader input) throws java.io.IOException
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
	  internal static FudgeTime readFudgeTime(BinaryReader input)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int hi = input.readInt();
          int hi = input.ReadInt32();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int lo = input.readInt();
          int lo = input.ReadInt32();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int timezoneOffset = (hi >> 24);
		int timezoneOffset = (hi >> 24); // sign extend
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int accuracy = (hi >> 20) & 15;
		int accuracy = (hi >> 20) & 15;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int seconds = hi & 0x1FFFF;
		int seconds = hi & 0x1FFFF;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nanos = lo & 0x3FFFFFFF;
		int nanos = lo & 0x3FFFFFFF;
		//System.out.println ("readFudgeTime: " + hi + ", " + lo + ", " + timezoneOffset + ", " + accuracy + ", " + seconds + ", " + nanos);
		return new FudgeTime(DateTimeAccuracy.FromEncodedValue(accuracy), timezoneOffset, seconds, nanos);
	  }

	  /// <summary>
	  /// Writes a Fudge date representation to an output target.
	  /// </summary>
	  /// <param name="output"> output target </param>
	  /// <param name="value"> Fudge date </param>
	  /// <exception cref="IOException"> if there is an error from the output target </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: static void writeFudgeDate(final java.io.BinaryWriter output, final Fudge.Types.FudgeDate value) throws java.io.IOException
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
	  internal static void writeFudgeDate(BinaryWriter output, FudgeDate value)
	  {
		long n = value.RawValue;
		output.Write(n);
	  }

	  /// <summary>
	  /// Writes a Fudge time representation to an output target.
	  /// </summary>
	  /// <param name="output"> the output target </param>
	  /// <param name="value"> the Fudge time </param>
	  /// <exception cref="IOException"> if there is an error from the output target </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: static void writeFudgeTime(final java.io.BinaryWriter output, final Fudge.Types.FudgeTime value) throws java.io.IOException
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
	  internal static void writeFudgeTime(BinaryWriter output, FudgeTime value)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int hi = (value.getSecondsSinceMidnight() & 0x1FFFF) | (value.getAccuracy().getEncodedValue() << 20) | (value.getEncodedTimezoneOffset() << 24);
		int hi = (value.SecondsSinceMidnight & 0x1FFFF) | (value.Accuracy.GetEncodedValue() << 20) | (value.EncodedTimezoneOffset << 24);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int lo = value.getNanos() & 0x3FFFFFFF;
		int lo = value.Nanoseconds & 0x3FFFFFFF;
		//System.out.println ("writeFudgeTime: " + hi + ", " + lo);
		output.Write(hi);
		output.Write(lo);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public Fudge.Types.FudgeDateTime ReadValue(java.io.BinaryReader input, int dataSize) throws java.io.IOException
	  public FudgeDateTime ReadValue(BinaryReader input, int dataSize)
	  {
          return (FudgeDateTime)(Object)ReadDynamicValue(input, dataSize);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void WriteValue(java.io.BinaryWriter output, Object value) throws java.io.IOException
	  public override void WriteValue(BinaryWriter output, object value)
	  {
		FudgeDateTime data = (FudgeDateTime) value;
		writeFudgeDate(output, data.Date);
		writeFudgeTime(output, data.Time);
	  }


      public override int GetVariableSize(object value, Taxon.IFudgeTaxonomy taxonomy)
      {
          return base.FixedSize;
      }

      public override dynamic ReadDynamicValue(BinaryReader input, int dataSize)
      {
          FudgeDate date = readFudgeDate(input);

          FudgeTime time = readFudgeTime(input);
          return new FudgeDateTime(date, time);

      }
    }

}