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
using Fudge.Serialization;

namespace Fudge.Mapping
{


	/// <summary>
	/// Builder for primitive Java objects. Typically this is required if a primitive was written out
	/// through serialization (i.e. has a class name) but is being deserialized to an Object target.
	/// 
	/// @author Andrew Griffin
	/// </summary>
	/* package */	internal class PrimitiveBuilder
	{

	  private const string VALUE_KEY = "value";

	  /// <summary>
	  /// Handles java.lang.Boolean
	  /// </summary>
	  /* package */	internal class BuildBoolean : IFudgeBuilder<bool?>
	{

		/// <summary>
		/// Singleton instance.
		/// </summary>
		/* package */	internal static readonly IFudgeBuilder<bool?> INSTANCE = new BuildBoolean();

		internal BuildBoolean()
		{
		}

		/// <summary>
		/// Creates a message containing the object value as a Fudge primitive field.
		/// </summary>
		/// <param name="context"> the serialization context </param>
		/// <param name="object"> the value </param>
		/// <returns> the message </returns>
		public override IMutableFudgeFieldContainer buildMessage(FudgeSerializer context, bool? @object)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer message = context.newMessage();
		  IMutableFudgeFieldContainer message = context.newMessage();
		  message.add(VALUE_KEY, ((bool) @object) ? 1 : 0);
		  return message;
		}

		/// <summary>
		/// Returns the Java object representation of the value within a message.
		/// </summary>
		/// <param name="context"> the deserialization context </param>
		/// <param name="message"> the message </param>
		/// <returns> the object </returns>
		public override bool? buildObject(FudgeDeserializer context, IFudgeFieldContainer message)
		{
		  return message.getValue(typeof(int?), VALUE_KEY) != 0;
		}

	}

	  /// <summary>
	  /// Handles java.lang.Byte
	  /// </summary>
	  /* package */	internal class BuildByte : IFudgeBuilder<sbyte?>
	{

		/// <summary>
		/// Singleton instance.
		/// </summary>
		/* package */	internal static readonly IFudgeBuilder<sbyte?> INSTANCE = new BuildByte();

		internal BuildByte()
		{
		}

		/// <summary>
		/// Creates a message containing the object value as a Fudge primitive field.
		/// </summary>
		/// <param name="context"> the serialization context </param>
		/// <param name="object"> the value </param>
		/// <returns> the message </returns>
		public override IMutableFudgeFieldContainer buildMessage(FudgeSerializer context, sbyte? @object)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer message = context.newMessage();
		  IMutableFudgeFieldContainer message = context.newMessage();
		  message.add(VALUE_KEY, @object);
		  return message;
		}

		/// <summary>
		/// Returns the Java object representation of the value within a message.
		/// </summary>
		/// <param name="context"> the deserialization context </param>
		/// <param name="message"> the message </param>
		/// <returns> the object </returns>
		public override sbyte? buildObject(FudgeDeserializer context, IFudgeFieldContainer message)
		{
		  return message.getValue(typeof(sbyte?), VALUE_KEY);
		}

	}

	  /// <summary>
	  /// Handles java.lang.Double
	  /// </summary>
	  /* package */	internal class BuildDouble : IFudgeBuilder<double?>
	{

		/// <summary>
		/// Singleton instance.
		/// </summary>
		/* package */	internal static readonly IFudgeBuilder<double?> INSTANCE = new BuildDouble();

		internal BuildDouble()
		{
		}

		/// <summary>
		/// Creates a message containing the object value as a Fudge primitive field.
		/// </summary>
		/// <param name="context"> the serialization context </param>
		/// <param name="object"> the value </param>
		/// <returns> the message </returns>
		public override IMutableFudgeFieldContainer buildMessage(FudgeSerializer context, double? @object)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer message = context.newMessage();
		  IMutableFudgeFieldContainer message = context.newMessage();
		  message.add(VALUE_KEY, @object);
		  // TODO what about NaN and infinite ?
		  return message;
		}

		/// <summary>
		/// Returns the Java object representation of the value within a message.
		/// </summary>
		/// <param name="context"> the deserialization context </param>
		/// <param name="message"> the message </param>
		/// <returns> the object </returns>
		public override double? buildObject(FudgeDeserializer context, IFudgeFieldContainer message)
		{
		  return message.getValue(typeof(double?), VALUE_KEY);
		}

	}

	  /// <summary>
	  /// Handles java.lang.Character. A character isn't available as a Fudge primitive, so is written as
	  /// a string of length 1.
	  /// </summary>
	  /* package */	internal class BuildCharacter : IFudgeBuilder<char?>
	{

		/// <summary>
		/// Singleton instance.
		/// </summary>
		/* package */	internal static readonly IFudgeBuilder<char?> INSTANCE = new BuildCharacter();

		internal BuildCharacter()
		{
		}

		/// <summary>
		/// Creates a message containing the object value as a Fudge primitive field.
		/// </summary>
		/// <param name="context"> the serialization context </param>
		/// <param name="object"> the value </param>
		/// <returns> the message </returns>
		public override IMutableFudgeFieldContainer buildMessage(FudgeSerializer context, char? @object)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer message = context.newMessage();
		  IMutableFudgeFieldContainer message = context.newMessage();
		  message.add(VALUE_KEY, @object.ToString());
		  return message;
		}

		/// <summary>
		/// Returns the Java object representation of the value within a message.
		/// </summary>
		/// <param name="context"> the deserialization context </param>
		/// <param name="message"> the message </param>
		/// <returns> the object </returns>
		public override char? buildObject(FudgeDeserializer context, IFudgeFieldContainer message)
		{
		  return message.getValue(typeof(string), VALUE_KEY).charAt(0);
		}

	}

	  /// <summary>
	  /// Handles java.lang.Float
	  /// </summary>
	  /* package */	internal class BuildFloat : IFudgeBuilder<float?>
	{

		/// <summary>
		/// Singleton instance.
		/// </summary>
		/* package */	internal static readonly IFudgeBuilder<float?> INSTANCE = new BuildFloat();

		internal BuildFloat()
		{
		}

		/// <summary>
		/// Creates a message containing the object value as a Fudge primitive field.
		/// </summary>
		/// <param name="context"> the serialization context </param>
		/// <param name="object"> the value </param>
		/// <returns> the message </returns>
		public override IMutableFudgeFieldContainer buildMessage(FudgeSerializer context, float? @object)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer message = context.newMessage();
		  IMutableFudgeFieldContainer message = context.newMessage();
		  message.add(VALUE_KEY, @object);
		  // TODO what about NaN and infinite ?
		  return message;
		}

		/// <summary>
		/// Returns the Java object representation of the value within a message.
		/// </summary>
		/// <param name="context"> the deserialization context </param>
		/// <param name="message"> the message </param>
		/// <returns> the object </returns>
		public override float? buildObject(FudgeDeserializer context, IFudgeFieldContainer message)
		{
		  return message.getValue(typeof(float?), VALUE_KEY);
		}

	}

	  /// <summary>
	  /// Handles java.lang.Integer
	  /// </summary>
	  /* package */	internal class BuildInteger : IFudgeBuilder<int?>
	{

		/// <summary>
		/// Singleton instance.
		/// </summary>
		/* package */	internal static readonly IFudgeBuilder<int?> INSTANCE = new BuildInteger();

		internal BuildInteger()
		{
		}

		/// <summary>
		/// Creates a message containing the object value as a Fudge primitive field.
		/// </summary>
		/// <param name="context"> the serialization context </param>
		/// <param name="object"> the value </param>
		/// <returns> the message </returns>
		public override IMutableFudgeFieldContainer buildMessage(FudgeSerializer context, int? @object)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer message = context.newMessage();
		  IMutableFudgeFieldContainer message = context.newMessage();
		  message.add(VALUE_KEY, @object);
		  return message;
		}

		/// <summary>
		/// Returns the Java object representation of the value within a message.
		/// </summary>
		/// <param name="context"> the deserialization context </param>
		/// <param name="message"> the message </param>
		/// <returns> the object </returns>
		public override int? buildObject(FudgeDeserializer context, IFudgeFieldContainer message)
		{
		  return message.getValue(typeof(int?), VALUE_KEY);
		}

	}

	  /// <summary>
	  /// Handles java.lang.Long
	  /// </summary>
	  /* package */	internal class BuildLong : IFudgeBuilder<long?>
	{

		/// <summary>
		/// Singleton instance.
		/// </summary>
		/* package */	internal static readonly IFudgeBuilder<long?> INSTANCE = new BuildLong();

		internal BuildLong()
		{
		}

		/// <summary>
		/// Creates a message containing the object value as a Fudge primitive field.
		/// </summary>
		/// <param name="context"> the serialization context </param>
		/// <param name="object"> the value </param>
		/// <returns> the message </returns>
		public override IMutableFudgeFieldContainer buildMessage(FudgeSerializer context, long? @object)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer message = context.newMessage();
		  IMutableFudgeFieldContainer message = context.newMessage();
		  message.add(VALUE_KEY, @object);
		  return message;
		}

		/// <summary>
		/// Returns the Java object representation of the value within a message.
		/// </summary>
		/// <param name="context"> the deserialization context </param>
		/// <param name="message"> the message </param>
		/// <returns> the object </returns>
		public override long? buildObject(FudgeDeserializer context, IFudgeFieldContainer message)
		{
		  return message.getValue(typeof(long?), VALUE_KEY);
		}

	}

	  /// <summary>
	  /// Handles java.lang.Short
	  /// </summary>
	  /* package */	internal class BuildShort : IFudgeBuilder<short?>
	{

		/// <summary>
		/// Singleton instance.
		/// </summary>
		/* package */	internal static readonly IFudgeBuilder<short?> INSTANCE = new BuildShort();

		internal BuildShort()
		{
		}

		/// <summary>
		/// Creates a message containing the object value as a Fudge primitive field.
		/// </summary>
		/// <param name="context"> the serialization context </param>
		/// <param name="object"> the value </param>
		/// <returns> the message </returns>
		public override IMutableFudgeFieldContainer buildMessage(FudgeSerializer context, short? @object)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer message = context.newMessage();
		  IMutableFudgeFieldContainer message = context.newMessage();
		  message.add(VALUE_KEY, @object);
		  return message;
		}

		/// <summary>
		/// Returns the Java object representation of the value within a message.
		/// </summary>
		/// <param name="context"> the deserialization context </param>
		/// <param name="message"> the message </param>
		/// <returns> the object </returns>
		public override short? buildObject(FudgeDeserializer context, IFudgeFieldContainer message)
		{
		  return message.getValue(typeof(short?), VALUE_KEY);
		}

	}

	  /// <summary>
	  /// Handles java.lang.String
	  /// </summary>
	  /* package */	internal class BuildString : IFudgeBuilder<string>
	{

		/// <summary>
		/// Singleton instance.
		/// </summary>
		/* package */	internal static readonly IFudgeBuilder<string> INSTANCE = new BuildString();

		internal BuildString()
		{
		}

		/// <summary>
		/// Creates a message containing the object value as a Fudge primitive field.
		/// </summary>
		/// <param name="context"> the serialization context </param>
		/// <param name="object"> the value </param>
		/// <returns> the message </returns>
		public override IMutableFudgeFieldContainer buildMessage(FudgeSerializer context, string @object)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer message = context.newMessage();
		  IMutableFudgeFieldContainer message = context.newMessage();
		  message.add(VALUE_KEY, @object);
		  return message;
		}

		/// <summary>
		/// Returns the Java object representation of the value within a message.
		/// </summary>
		/// <param name="context"> the deserialization context </param>
		/// <param name="message"> the message </param>
		/// <returns> the object </returns>
		public override string buildObject(FudgeDeserializer context, IFudgeFieldContainer message)
		{
		  return message.getValue(typeof(string), VALUE_KEY);
		}

	}

	}

}