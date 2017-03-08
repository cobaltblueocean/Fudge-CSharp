using System;

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
	/// Factory interface for constructing builders for classes that haven't been explicitly
	/// registered with a <seealso cref="FudgeObjectDictionary"/>. The factory should not attempt to
	/// cache results - the {@code FudgeObjectDictionary} will do that.
	/// 
	/// @author Andrew Griffin
	/// </summary>
	public interface IFudgeBuilderFactory
	{

      /// <summary>
      /// Creates a new <seealso cref="FudgeObjectBuilder"/> for deserializing Fudge messages into the given class.
      /// </summary>
      /// <param name="<T>"> the class the builder should create objects of </param>
      /// <param name="clazz"> the class the builder should create objects of </param>
      /// <returns> the builder or {@code null} if none is available </returns>
      IFudgeObjectBuilder<T> CreateObjectBuilder<T>(Type clazz);

	  /// <summary>
	  /// Creates a new <seealso cref="FudgeMessageBuilder"/> for encoding objects of the given class into Fudge messages.
	  /// </summary>
	  /// <param name="clazz"> the class the builder should create messages from </param>
      /// <param name="<T>"> the class the builder should create messages from </param>
	  /// <returns>  the builder or {@code null} if none is available </returns>
      IFudgeMessageBuilder<T> CreateMessageBuilder<T>(Type clazz);

	  /// <summary>
	  /// Registers a generic builder with the factory that may be returned as a <seealso cref="FudgeObjectBuilder"/> for
	  /// the class, or as a <seealso cref="FudgeMessageBuilder"/> for any sub-classes of the class. After calling this, a
	  /// factory may choose to return an alternative builder, but may not return {@code null} for a class which
	  /// the generic builder has been registered for.
	  /// </summary>
	  /// <param name="clazz"> the generic type (probably an interface) the builder is for </param>
	  /// <param name="builder"> the builder to register </param>
      /// <param name="<T>"> the generic type (probably an interface) the builder is for </param>
	  void AddGenericBuilder<T>(Type clazz, IFudgeBuilder<T> builder);

	}
}