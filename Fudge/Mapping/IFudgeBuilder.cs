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

namespace Fudge.Mapping
{

	/// <summary>
	/// Helper interface combining the message and object building operations.
	/// </summary>
	/// @param <T> class that can be serialised or deserialised by this builder
	/// @author Andrew Griffin </param>
    public interface IFudgeBuilder<T> : IFudgeMessageBuilder, IFudgeObjectBuilder
	{

	}
}