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
	/// <seealso cref="FudgeBuilder"/> or <seealso cref="IFudgeObjectBuilder"/> for a particular
	/// abstract or interface data type.
	/// Where the intention is to invoke <seealso cref="FudgeBuilderFactory#addGenericBuilder(Class, FudgeBuilder)"/> rather
	/// than <seealso cref="FudgeObjectDictionary#addBuilder(Class, FudgeBuilder)"/>, this annotation should be used instead.
	/// 
	/// @author Kirk Wylie
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Documented @Retention(RetentionPolicy.RUNTIME) @Target(ElementType.GetType()) public class GenericFudgeBuilderFor extends System.Attribute
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class GenericFudgeBuilderFor : System.Attribute
	{

	  /// <summary>
	  /// The generic class for which the annotated type is a <seealso cref="FudgeBuilder"/>.
	  /// </summary>
        Type value { get; set; }

	}

}