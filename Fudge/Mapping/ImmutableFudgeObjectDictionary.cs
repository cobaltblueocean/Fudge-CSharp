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
	/// An immutable version of a <seealso cref="FudgeObjectDictionary"/>.
	/// 
	/// @author Andrew Griffin
	/// </summary>
	public class ImmutableFudgeObjectDictionary : FudgeObjectDictionary
	{

	  /// <summary>
	  /// Creates a new <seealso cref="FudgeObjectDictionary"/> as an immutable clone of an existing one.
	  /// </summary>
	  /// <param name="dictionary"> The {@code FudgeObjectDictionary} to clone </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public ImmutableFudgeObjectDictionary(final FudgeObjectDictionary dictionary)
	  public ImmutableFudgeObjectDictionary(FudgeObjectDictionary dictionary) : base(dictionary)
	  {
	  }

	  /// <summary>
	  /// Always throws an exception - this is an immutable dictionary.
	  /// </summary>
	  /// <param name="defaultBuilderFactory"> the {@code FudgeBuilderFactory} to use </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: @Override public void setDefaultBuilderFactory(final FudgeBuilderFactory defaultBuilderFactory)
	  public override IFudgeBuilderFactory DefaultBuilderFactory
	  {
		  set
		  {
			throw new System.NotSupportedException("setDefaultBuilderFactory called on an immutable dictionary");
		  }
	  }

	  /// <summary>
	  /// Always throws an exception - this is an immutable dictionary.
	  /// </summary>
	  /// @param <T> Java type of the objects created by the builder </param>
	  /// <param name="clazz"> the Java class to register the builder against </param>
	  /// <param name="builder"> the builder to register </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: @Override public <T> void addObjectBuilder(final Class clazz, final FudgeObjectBuilder<? extends T> builder)
	  public override void addObjectBuilder<T, T1>(Type clazz, IFudgeObjectBuilder<T1> builder)// where T1 : T
	  {
		throw new System.NotSupportedException("addObjectBuilder called on an immutable dictionary");
	  }

	  /// <summary>
	  /// Always throws an exception - this is an immutable dictionary.
	  /// </summary>
	  /// @param <T> Java type of the objects processed by the builder </param>
	  /// <param name="clazz"> the Java class to register the builder against </param>
	  /// <param name="builder"> builder to register </param>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public <T> void addMessageBuilder(final Class clazz, final FudgeMessageBuilder<? base T> builder)
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
	  public override void addMessageBuilder<T, T1>(Type clazz, IFudgeMessageBuilder builder)
	  {
		throw new System.NotSupportedException("addMessageBuilder called on an immutable dictionary");
	  }

	  /// <summary>
	  /// Always throws an exception - this is an immutable dictionary.
	  /// </summary>
	  /// @param <T> Java type of the objects processed by the builder </param>
	  /// <param name="clazz"> the Java class to register the builder against </param>
	  /// <param name="builder"> builder to register </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: @Override public <T> void addBuilder(final Class clazz, final FudgeBuilder<T> builder)
	  public override void addBuilder<T>(Type clazz, IFudgeBuilder<T> builder)
	  {
		throw new System.NotSupportedException("addMessageBuilder called on an immutable dictionary");
	  }

	}
}