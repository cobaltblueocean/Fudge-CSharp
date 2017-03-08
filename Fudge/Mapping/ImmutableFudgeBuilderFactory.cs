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
	/// Immutable <seealso cref="FudgeBuilderFactory"/> implementation.
	/// 
	/// @author Andrew Griffin
	/// </summary>
	/* package */	 internal class ImmutableFudgeBuilderFactory : FudgeBuilderFactoryAdapter
	 {

	  /// <summary>
	  /// {@docInherit}
	  /// </summary>
	  /// <param name="delegate"> instance to pass non-overridden method calls to  </param>
	  /* package */	 internal ImmutableFudgeBuilderFactory(IFudgeBuilderFactory @delegate) : base(@delegate)
	 {
	 }

	  /// <summary>
	  /// Always throws an exception - this is an immutable factory
	  /// </summary>
	  /// <param name="clazz"> the generic type (probably an interface) the builder is for </param>
	  /// <param name="builder"> the builder to register </param>
	  /// @param <T> the generic type (probably an interface) the builder is for </param>
	  public virtual void AddGenericBuilder<T>(Type clazz, IFudgeBuilder<T> builder)
	  {
		throw new System.NotSupportedException("AddGenericBuilder called on immutable instance");
	  }

	 }
}