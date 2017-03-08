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
	/// Utility class for checking for a transient annotation on a Bean property. The Transient annotation from the
	/// Java Persistence Framework should be used where available. Otherwise, there is a Transient annotation defined
	/// within the org.fudgemsg.mapping package.
	/// 
	/// @author Andrew Griffin
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") class TransientUtil
	internal class TransientUtil
	/* package */
	{

	  private static readonly Type s_fudgeTransient = typeof(FudgeTransient);
	  private static readonly Type s_javaxTransient;

	  private TransientUtil()
	  {
	  }

	  static TransientUtil()
	  {
		Type javaxTransient = null;
		try
		{
		  javaxTransient = (Type)Type.GetType("javax.persistence.Transient");
		}
		catch (TypeLoadException)
		{
		  // ignore
		}
		s_javaxTransient = javaxTransient;
	  }

	  /// <summary>
	  /// Detects whether the {@code javax.persistence.Transient} or <seealso cref="FudgeTransient"/> annotation has been used on an element
	  /// </summary>
	  /// <param name="element"> element to check </param>
	  /// <returns> {@code true} if the annotation is present, {@code false} otherwise </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public static boolean hasTransientAnnotation(final AnnotatedElement element)
	  public static bool hasTransientAnnotation(AnnotatedElement element)
	  {
		if (s_javaxTransient != null)
		{
		  if (element.getAnnotation(s_javaxTransient) != null)
		  {
			  return true;
		  }
		}
		if (s_fudgeTransient != null)
		{
		  if (element.getAnnotation(s_fudgeTransient) != null)
		  {
			  return true;
		  }
		}
		return false;
	  }

	}
}