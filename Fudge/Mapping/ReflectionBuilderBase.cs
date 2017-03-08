using System;
using System.Reflection;
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
	/// Base class for <seealso cref="ReflectionObjectBuilder"/> and <seealso cref="ReflectionMessageBuilder"/>.
	/// Contains some helper methods common to reading class methods. The <seealso cref="DotNetAssemblyBuilder"/>
	/// is superior - good old commons-beanutils - these classes have been left in if that
	/// package is not available to remove any dependency on other packages. It may be removed
	/// from future releases.
	/// </summary>
	/// @param <T> class that can be serialized or deserialized by this builder
	/// @author Andrew Griffin </param>
	/* package */	 internal class ReflectionBuilderBase<T>
	 {

	  private readonly IDictionary<string, MethodInfo> _methods;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private static void findMethods(final Class clazz, final java.util.Map<String, Method> methods, final String prefix, final int paramLength, final Class recurseLimit)
	  private static void findMethods(Type clazz, IDictionary<string, MethodInfo> methods, string prefix, int paramLength, Type recurseLimit)
	  {
		if (clazz == typeof(object))
		{
			return;
		}
		if (clazz.BaseType != recurseLimit)
		{
		  findMethods(clazz.BaseType, methods, prefix, paramLength, recurseLimit);
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int prefixLength = prefix.length();
		int prefixLength = prefix.Length;
		foreach (MethodInfo method in clazz.GetMethods())
		{
            if ((!method.IsPublic) || (!method.IsStatic)) // || (!method)
            {
                continue;
            }
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int modifiers = method.getModifiers();
          //int modifiers = method.Modifiers;
          //if (!Modifier.isPublic(modifiers) || Modifier.isStatic(modifiers) || Modifier.isTransient(modifiers))
          //{
          //    continue;
          //}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Class params[] = method.getParameterTypes();
		  var @params = method.GetParameters();  // .ParameterTypes;
		  if (@params.Length != paramLength)
		  {
			  continue;
		  }
		  string name = method.Name;
		  if (name.Length <= prefixLength)
		  {
			  continue;
		  }
		  if (!char.IsUpper(name[prefixLength]))
		  {
			  continue;
		  }
		  if (!name.StartsWith(prefix, StringComparison.Ordinal))
		  {
			  continue;
		  }
		  name = char.ToLower(name[prefixLength]) + name.Substring(prefixLength + 1);
		  methods[name] = method;
		}
	  }

	  /// <summary>
	  /// Creates a new <seealso cref="ReflectionBuilderBase"/> instance, resolving any methods that match the accessor or mutator pattern.
	  /// </summary>
	  /// <param name="clazz"> class to be serialized or deserialize by the builder </param>
	  /// <param name="prefix"> method prefix, e.g. {@code get} or {@code set} </param>
	  /// <param name="paramLength"> number of expected parameters </param>
	  /// <param name="recurseLimit"> class to stop at when processing the superclasses </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: protected ReflectionBuilderBase(final Class clazz, final String prefix, final int paramLength, final Class recurseLimit)
	  protected internal ReflectionBuilderBase(Type clazz, string prefix, int paramLength, Type recurseLimit)
	  {
		_methods = new Dictionary<string, MethodInfo> ();
		findMethods(clazz, _methods, prefix, paramLength, recurseLimit);
	  }

	  /// <summary>
	  /// Returns the map of attribute names to method calls.
	  /// </summary>
	  /// <returns> the <seealso cref="Map"/> </returns>
	  protected internal virtual IDictionary<string, MethodInfo> Methods
	  {
		  get
		  {
			return _methods;
		  }
	  }

	 }
}