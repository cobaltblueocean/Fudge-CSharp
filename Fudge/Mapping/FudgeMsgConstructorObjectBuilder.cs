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
using System.Security;
using System.Reflection;

namespace Fudge.Mapping
{



	/// <summary>
	/// Implementation of FudgeObjectBuilder for a class with a public constructor that
	/// accepts a <seealso cref="FudgeFieldContainer"/> or a <seealso cref="FudgeDeserializationContext"/> and <seealso cref="FudgeFieldContainer"/>.
	/// </summary>
	/// @param <T> class supporting a {@code FudgeFieldContainer} constructor that can be deserialized by this builder
	/// @author Andrew Griffin </param>
	/* package */	 internal class FudgeMsgConstructorObjectBuilder<T> : IFudgeObjectBuilder
	 {

	  /// <summary>
	  /// Creates a new <seealso cref="FudgeMsgConstructorObjectBuilder"/> for the class if possible.
	  /// </summary>
	  /// @param <T> class the builder should create objects of </param>
	  /// <param name="clazz"> class the builder should create objects of </param>
	  /// <returns> the {@code FudgeMsgConstructorObjectBuilder} or {@code null} if none is available </returns>
	  /* package *///JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: static <T> FudgeMsgConstructorObjectBuilder<T> create(final Class clazz)
	 internal static FudgeMsgConstructorObjectBuilder<T> create<T>(Type clazz)
	 {
		try
		{
		  return new FudgeMsgConstructorObjectBuilder<T> (clazz, true);
		}
		catch (SecurityException)
		{
		  // ignore
		}
		catch (NotImplementedException )
		{
		  // ignore
		}
		return null;
	 }

	  private readonly Type _assemblyType;
	  private readonly bool _passContext;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private FudgeMsgConstructorObjectBuilder(final Constructor<T> constructor, final boolean passContext)
      private FudgeMsgConstructorObjectBuilder(Type assemblyType, bool passContext)
	  {
          _assemblyType = assemblyType;
		_passContext = passContext;
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: @Override public T buildObject(final FudgeDeserializationContext context, final org.fudgemsg.FudgeFieldContainer message)
	  public override T BuildObject(FudgeDeserializer context, IFudgeFieldContainer message)
	  {
		try
		{
            return _passContext ? (T)Activator.CreateInstance(_assemblyType.GetType(), new Object[] { context, message }) : (T)Activator.CreateInstance(_assemblyType.GetType(), new Object[] { message });
		}
		catch (System.ArgumentException e)
		{
		  throw new FudgeRuntimeException("Couldn't create " + _assemblyType.FullName + " object", e);
		}
        catch (TypeLoadException e)
		{
            throw new FudgeRuntimeException("Couldn't create " + _assemblyType.FullName + " object", e);
		}
        catch (MissingMethodException e)
		{
            throw new FudgeRuntimeException("Couldn't create " + _assemblyType.FullName + " object", e);
		}
        catch (TargetInvocationException e)
		{
            throw new FudgeRuntimeException("Couldn't create " + _assemblyType.FullName + " object", e);
		}
	  }

	 }
}