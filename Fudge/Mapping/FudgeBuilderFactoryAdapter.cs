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
	/// Implementation of a <seealso cref="FudgeBuilderFactory"/> that can delegate to another
	/// instance for unrecognized classes. This pattern is to allow factories to be
	/// chained together.
	/// 
	/// @author Andrew Griffin
	/// </summary>
	public class FudgeBuilderFactoryAdapter : IFudgeBuilderFactory
	{

	  private readonly IFudgeBuilderFactory _delegate;

	  /// <summary>
	  /// Creates a new <seealso cref="FudgeBuilderFactoryAdapter"/>.
	  /// </summary>
	  /// <param name="delegate"> instance to pass non-overridden method calls to </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: protected FudgeBuilderFactoryAdapter(final FudgeBuilderFactory delegate)
	  protected internal FudgeBuilderFactoryAdapter(IFudgeBuilderFactory @delegate)
	  {
		if (@delegate == null)
		{
			throw new System.NullReferenceException("delegate cannot be null");
		}
		_delegate = @delegate;
	  }

	  /// <summary>
	  /// Returns the delegate instance to pass method calls to.
	  /// </summary>
	  /// <returns> the <seealso cref="FudgeBuilderFactory"/> delegate </returns>
	  protected internal virtual IFudgeBuilderFactory Delegate
	  {
		  get
		  {
			return _delegate;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual void AddGenericBuilder<T>(Type clazz, IFudgeBuilder<T> builder)
	  {
		Delegate.AddGenericBuilder<T>(clazz, builder);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual IFudgeMessageBuilder<T> CreateMessageBuilder<T>(Type clazz)
	  {
		return Delegate.CreateMessageBuilder<T>(clazz);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual IFudgeObjectBuilder<T> CreateObjectBuilder<T>(Type clazz)
	  {
		return Delegate.CreateObjectBuilder<T>(clazz);
	  }

	}
}