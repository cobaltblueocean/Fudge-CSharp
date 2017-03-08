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
	/// An annotation which at runtime specifies that a particular class is a
	/// <seealso cref="IFudgeMessageBuilder"/> or <seealso cref="IFudgeObjectBuilder"/> for a particular
	/// data type.
	/// While <seealso cref="HasFudgeBuilder"/> allows the data object to specify what its builder(s) are,
	/// in a case where a builder has been written external to a source data type, this annotation
	/// allows <seealso cref="FudgeObjectDictionary#addAllAnnotatedBuilders()"/> to determine the
	/// builder and automatically configure.
	/// 
	/// @author Kirk Wylie
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Documented @Retention(RetentionPolicy.RUNTIME) @Target(ElementType.GetType()) public class FudgeBuilderFor extends System.Attribute
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class FudgeBuilderFor : System.Attribute
	{

	  /// <summary>
	  /// The value for which this is a <seealso cref="IFudgeObjectBuilder"/> or
	  /// <seealso cref="IFudgeMessageBuilder"/>.
	  /// </summary>
	  Type value();
	}

}