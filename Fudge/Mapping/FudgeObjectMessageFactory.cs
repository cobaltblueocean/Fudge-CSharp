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



	/// <summary>
	/// <para>Converts between Java objects and <seealso cref="IFudgeFieldContainer"/> messages using the Fudge serialisation
	/// framework. This class is provided for convenience, direct use of a <seealso cref="FudgeSerializer"/> or <seealso cref="FudgeDeserializer"/>
	/// will be more efficient.</para>
	/// 
	/// <para>This has been deprecated since 0.3, to be removed at the 1.0 release; a couple of utility methods have
	/// gone into the <seealso cref="FudgeContext"/> to support this.</para>
	/// 
	/// @author Kirk Wylie
	/// </summary>
	[Obsolete]
	public class FudgeObjectMessageFactory
	  /// <summary>
	  /// Serialises a Java object to a <seealso cref="FudgeFieldContainer"/> message. Use <seealso cref="FudgeContext#toFudgeMsg"/> instead.
	  /// </summary>
	  /// @param <T> Java type </param>
	  /// <param name="obj"> object to serialise </param>
	  /// <param name="context"> the <seealso cref="FudgeContext"/> to use </param>
	  /// <returns> the serialised message </returns>
	{
		[Obsolete]
		public static IMutableFudgeFieldContainer serializeToMessage<T>(T obj, FudgeContext context)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final FudgeSerializer fsc = new FudgeSerializer(context);
		FudgeSerializer fsc = new FudgeSerializer(context);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer message = fsc.objectToFudgeMsg(obj);
		IMutableFudgeFieldContainer message = fsc.ObjectToFudgeMsg(obj);
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
//ORIGINAL LINE: if (!(obj instanceof java.util.List) && !(obj instanceof java.util.Set) && !(obj instanceof java.util.Map<?, ?>))
		if (!(obj is IList<T>) && !(obj is HashSet<T>) && !(obj is IDictionary<T, dynamic>))
		{
		  FudgeSerializer.AddClassHeader(message, typeof(obj));
		}
		return message;
		}

	  /// <summary>
	  /// Deserializes a <seealso cref="IFudgeFieldContainer"/> message to a Java object, trying to determine the 
	  /// type of the object automatically. Use <seealso cref="FudgeContext#fromFudgeMsg(FudgeFieldContainer)"/> instead.
	  /// </summary>
	  /// <param name="message"> the Fudge message to deserialize </param>
	  /// <param name="context"> the <seealso cref="FudgeContext"/> to use </param>
	  /// <returns> the deserialized object </returns>
	  [Obsolete]
	  public static object deserializeToObject(IFudgeFieldContainer message, FudgeContext context)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final FudgeDeserializationContext fdc = new FudgeDeserializationContext(context);
		FudgeDeserializer fdc = new FudgeDeserializer(context);
		return fdc.fudgeMsgToObject(message);
	  }

	  /// <summary>
	  /// Deserializes a <seealso cref="FudgeFieldContainer"/> message to a Java object of type {@code clazz}. Use <seealso cref="FudgeContext#fromFudgeMsg(Class,FudgeFieldContainer)"/> instead.
	  /// </summary>
	  /// @param <T> Java type </param>
	  /// <param name="clazz"> the target type to deserialise </param>
	  /// <param name="message"> the message to process </param>
	  /// <param name="context"> the underlying <seealso cref="FudgeContext"/> to use </param>
	  /// <returns> the deserialised object </returns>
	  [Obsolete]
	  public static T deserializeToObject<T>(Type clazz, IFudgeFieldContainer message, FudgeContext context)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final FudgeDeserializationContext fdc = new FudgeDeserializationContext(context);
		FudgeDeserializer fdc = new FudgeDeserializer(context);
		return fdc.fudgeMsgToObject<T>(clazz, message);
	  }

	}

}