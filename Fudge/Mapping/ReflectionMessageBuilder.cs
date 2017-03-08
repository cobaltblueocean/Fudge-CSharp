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
using System;
using System.Collections.Generic;
using System.Reflection;
using Fudge;
using Fudge.Serialization;

namespace Fudge.Mapping
{



	/// <summary>
	/// <para>Attempt to create an Fudge message containing values from Java-bean style getX
	/// methods. If the nearest superclass supports ToFudgeMsgMessageBuilder then that will
	/// be used to create the initial message that is supplemented by subclass getX
	/// methods.</para>
	/// 
	/// <para>This has been superceded by the DotNetAssemblyBuilder which uses the BeanUtils package
	/// and will probably be removed from future releases.</para>
	/// </summary>
	/// @param <T> class that can be serialized using this builder
	/// @author Andrew Griffin </param>
	/* package */
    internal class ReflectionMessageBuilder<T> : ReflectionBuilderBase<T>, IFudgeMessageBuilder
	 {
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: private final FudgeMessageBuilder<? base T> _baseBuilder;
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
	  private IFudgeMessageBuilder _baseBuilder;

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: private ReflectionMessageBuilder(final Class clazz, final Class upstream, final FudgeMessageBuilder<? base T> baseBuilder)
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
	  private ReflectionMessageBuilder(Type clazz, Type upstream, IFudgeMessageBuilder baseBuilder)// : base(clazz, "get", 0, upstream)
	  {
		_baseBuilder = baseBuilder;
	  }


	  /// <summary>
	  /// Creates a new <seealso cref="ReflectionMessageBuilder"/> for building messages from arbitrary Java objects. Always
	  /// succeeds, although the builder may only be capable of generating empty messages.
	  /// </summary>
	  /// @param <T> class to generate messages for </param>
	  /// <param name="clazz"> class to generate messages for </param>
	  /// <returns> the {@code ReflectionMessageBuilder} </returns>
	  /* package *///JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: static <T> ReflectionMessageBuilder<T> create(final Class clazz)
	 internal static ReflectionMessageBuilder<T> create(Type clazz)
	 {
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: FudgeMessageBuilder<? base T> builder = null;
//JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
		IFudgeMessageBuilder builder = null;
		Type superclazz = clazz;
		while ((superclazz = superclazz.BaseType) != null)
		{
		  builder = ToFudgeMsgMessageBuilder<T>.create(superclazz);
		  if (builder != null)
		  {
			  break;
		  }
		}
		return new ReflectionMessageBuilder<T> (clazz, superclazz, builder);
	 }


	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: @Override public org.fudgemsg.IMutableFudgeFieldContainer buildMessage(final FudgeSerializer context, final T object)
	  public override IMutableFudgeFieldContainer buildMessage(FudgeSerializer context, T @object)
	  {
		//System.out.println ("ReflectionMessageBuilder::buildMessage (" + context + ", " + object + ")");
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer message;
		IMutableFudgeFieldContainer message;
		if (_baseBuilder != null)
		{
		  message = _baseBuilder.BuildMessage(context, @object);
		}
		else
		{
		  message = (IMutableFudgeFieldContainer)context.NewMessage();
		}
		try
		{
		  foreach (KeyValuePair<string, MethodInfo> accessor in Methods)
		  {
			//System.out.println ("\t" + accessor.getValue ());
			context.ObjectToFudgeMsg(message, accessor.Key, null, accessor.Value.invoke(@object));
		  }
		}
		catch (System.ArgumentException e)
		{
		  throw new  FudgeRuntimeException("Couldn't serialise " + @object, e);
		}
        catch (MissingMethodException e)
		{
		  throw new FudgeRuntimeException("Couldn't serialise " + @object, e);
		}
		catch (TargetInvocationException e)
		{
		  throw new FudgeRuntimeException("Couldn't serialise " + @object, e);
		}
		return message;
	  }

	 }
}