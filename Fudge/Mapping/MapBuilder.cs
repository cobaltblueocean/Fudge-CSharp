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
	/// Builder for Map objects.
	/// 
	/// @author Andrew Griffin
	/// </summary>
	/* package */
    internal class MapBuilder : IFudgeBuilder<IDictionary<Type, Object>>
	 {

	  /// <summary>
	  /// Singleton instance of the <seealso cref="MapBuilder"/>.
	  /// </summary>
	  /* package *///JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: static final FudgeBuilder<java.util.Map<?,?>> INSTANCE = new MapBuilder();
        internal static readonly IFudgeBuilder<IDictionary<Type, Object>> INSTANCE = new MapBuilder();

	  private MapBuilder()
	  {
	  }

	  /// <summary>
	  /// Creates a Fudge message representation of a <seealso cref="Map"/>.
	  /// </summary>
	  /// <param name="serializer"> the serialization context </param>
	  /// <param name="map"> the map to serialize </param>
	  /// <returns> the Fudge message </returns>
      public override IMutableFudgeFieldContainer buildMessage<T1>(FudgeSerializer serializer, IDictionary<T1, Object> map)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer msg = context.newMessage();
		IMutableFudgeFieldContainer msg = serializer.NewMessage();
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: for (java.util.Map.Entry<?,?> entry : map.entrySet())
        foreach (KeyValuePair<T1, Object> entry in map)
		{
		  if (entry.Key == null)
		  {
			msg.Add(null, 1, IndicatorFieldType.Instance, IndicatorType.Instance);
		  }
		  else
		  {
			serializer.ObjectToFudgeMsgWithClassHeaders(msg, null, 1, entry.Key);
		  }
		  if (entry.Value == null)
		  {
			msg.Add(null, 2, IndicatorFieldType.Instance, IndicatorType.Instance);
		  }
		  else
		  {
			serializer.ObjectToFudgeMsgWithClassHeaders(msg, null, 2, entry.Value);
		  }
		}
		return msg;
	  }

	  /// <summary>
	  /// Creates a <seealso cref="Map"/> from a Fudge message.
	  /// </summary>
	  /// <param name="context"> the deserialization context </param>
	  /// <param name="message"> the Fudge message </param>
	  /// <returns> the {@code Map}  </returns>
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: @Override public java.util.Map<?,?> buildObject(FudgeDeserializationContext context, org.fudgemsg.FudgeFieldContainer message)
      public override IDictionary<Object, Object> buildObject(FudgeDeserializer context, IFudgeFieldContainer message)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Map<Object, Object> map = new java.util.HashMap<Object, Object> ();
          IDictionary<Object, Object> map = new Dictionary<Object, Object>();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Queue<Object> keys = new java.util.LinkedList<Object> ();
          LinkedList<Object> keys = new LinkedList<Object>();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Queue<Object> values = new java.util.LinkedList<Object> ();
          LinkedList<Object> values = new LinkedList<Object>();
		foreach (IFudgeField field in message)
		{
		  object fieldValue = context.fieldValueToObject(field);
		  if (fieldValue is IndicatorType)
		  {
			  fieldValue = null;
		  }
		  if (field.Ordinal == 1)
		  {
			if (values.Count == 0)
			{
			  // no values ready, so store the key till next time
			  keys.AddLast(fieldValue);
			}
			else
			{
			  // store key along with next value
			  map[fieldValue] = values.RemoveFirst();
			}
		  }
		  else if (field.Ordinal == 2)
		  {
			if (keys.Count == 0)
			{
			  // no keys ready, so store the value till next time
			  values.AddLast(fieldValue);
			}
			else
			{
			  // store value along with next key
			  map[keys.RemoveFirst()] = fieldValue;
			}
		  }
		  else
		  {
			throw new System.ArgumentException("Sub-message doesn't contain a map (bad field " + field + ")");
		  }
		}
		return map;
	  }

	 }
}