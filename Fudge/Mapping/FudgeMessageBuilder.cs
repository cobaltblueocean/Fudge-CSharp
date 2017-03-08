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
using Fudge;
using Fudge.Serialization;

namespace Fudge.Mapping
{

	/// <summary>
	/// Defines an object capable of adding data from a given Java object to a fudge message.
	/// </summary>
	/// @param <T> the Java type this builder creates Fudge message from
	/// 
	/// @author Andrew Griffin </param>
    public interface IFudgeMessageBuilder<T> : IFudgeMessageBuilder
	{

	  /// <summary>
	  /// Creates a message from the given object. Note that a mutable container must be returned, this
	  /// is to allow efficient implementation of sub-class builders that only need append data to the
	  /// super-class message.
	  /// </summary>
        /// <param name="serializer"> the <seealso cref="FudgeSerializer"/> </param>
	  /// <param name="object"> the object to serialise </param>
	  /// <returns> the Fudge message </returns>
      IMutableFudgeFieldContainer BuildMessage(FudgeSerializer serializer, T obj);
    }

    public interface IFudgeMessageBuilder
    { }

}