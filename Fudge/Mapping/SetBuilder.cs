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
using System.Collections.Generic;
using Fudge.Serialization;

namespace Fudge.Mapping
{


	using IndicatorFieldType = Fudge.Types.IndicatorFieldType;
	using IndicatorType = Fudge.Types.IndicatorType;

	/// <summary>
	/// Builder for Set objects.
	/// 
	/// @author Andrew Griffin
	/// </summary>
	/* package */	 internal class SetBuilder : IFudgeBuilder<HashSet<Object>>
	 {

	  /// <summary>
	  /// Singleton instance of the <seealso cref="SetBuilder"/>.
	  /// </summary>
	  /* package *///JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: static final FudgeBuilder<java.util.Set> INSTANCE = new SetBuilder();
        internal static readonly IFudgeBuilder<HashSet<Object>> INSTANCE = new SetBuilder();

	  private SetBuilder()
	  {
	  }

	  /// <summary>
	  /// Creates a Fudge message representation of a <seealso cref="Set"/>.
	  /// </summary>
	  /// <param name="context"> the serialization context </param>
	  /// <param name="set"> the set to serialize </param>
	  /// <returns> the Fudge message </returns>
	  public override IMutableFudgeFieldContainer buildMessage<T1>(FudgeSerializer context, HashSet<T1> set)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer msg = context.newMessage();
		IMutableFudgeFieldContainer msg = context.NewMessage();
		foreach (object entry in set)
		{
		  if (entry == null)
		  {
			msg.Add(null, 1, IndicatorFieldType.Instance, IndicatorType.Instance);
		  }
		  else
		  {
			context.ObjectToFudgeMsgWithClassHeaders(msg, null, 1, entry);
		  }
		}
		return msg;
	  }

	  /// <summary>
	  /// Creates a <seealso cref="Set"/> from a Fudge message.
	  /// </summary>
	  /// <param name="context"> the deserialization context </param>
	  /// <param name="message"> the Fudge message </param>
	  /// <returns> the {@code Set}  </returns>
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: @Override public java.util.Set buildObject(FudgeDeserializationContext context, org.fudgemsg.FudgeFieldContainer message)
      public override HashSet<object> buildObject(FudgeDeserializer context, IFudgeFieldContainer message)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Set<Object> set = new java.util.HashSet<Object> ();
		HashSet<object> set = new HashSet<object> ();
		foreach (IFudgeField field in message)
		{
		  object fieldValue = context.fieldValueToObject(field);
		  if (fieldValue is IndicatorType)
		  {
			  fieldValue = null;
		  }
		  if (field.Ordinal == 1)
		  {
			set.Add(fieldValue);
		  }
		  else
		  {
			throw new System.ArgumentException("Sub-message doesn't contain a set (bad field " + field + ")");
		  }
		}
		return set;
	  }

	 }
}