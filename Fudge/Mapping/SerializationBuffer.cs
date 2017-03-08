using System.Diagnostics;
using System.Collections.Generic;

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

namespace Fudge.Mapping
{


	/// <summary>
	/// A basic buffer for the serialization and deserialization contexts that can detect
	/// the cycles they can't deal with. When the method for processing object graphs has
	/// been agreed, this will process back and forward references.
	/// 
	/// @author Andrew Griffin
	/// </summary>
	/* package */	 internal class SerializationBuffer
	 {

	  private readonly Stack<object> _buffer;

	  /// <summary>
	  /// Creates a new <seealso cref="SerializationBuffer"/>.
	  /// </summary>
	  internal SerializationBuffer()
	  {
		_buffer = new Stack<object> ();
	  }

	  /// <summary>
	  /// Registers the start of an object being processed. During serialization can detect a loop
	  /// and raise a <seealso cref="FudgeRuntimeException"/>.
	  /// </summary>
	  /// <param name="object"> the object currently being processed </param>
	  /// <exception cref="FudgeRuntimeException"> if a cyclic reference is detected     </exception>
	  /* package *///JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: void beginObject(final Object object)
	 internal virtual void beginObject(object @object)
	 {
		if (_buffer.Contains(@object))
		{
			throw new System.NotSupportedException("Serialization framework can't support cyclic references");
		}
		_buffer.Push(@object);
	 }

	  /// <summary>
	  /// Registers the end of an object being processed.
	  /// </summary>
	  /// <param name="object"> the object being processed </param>
	  /* package *///JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: void endObject(final Object object)
	 internal virtual void endObject(object @object)
	 {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Object obj = _buffer.pop();
		object obj = _buffer.Pop();
		Debug.Assert(obj == @object);
	 }

	  /// <summary>
	  /// Resets the state of the buffer.
	  /// </summary>
	  /* package */	 internal virtual void reset()
	 {
		_buffer.Clear();
	 }

	 }
}