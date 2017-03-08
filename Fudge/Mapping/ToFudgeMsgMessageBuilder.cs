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

using Fudge.Serialization;

namespace Fudge.Mapping
{



	/// <summary>
	/// <para>Implementation of FudgeMessageBuilder for an object which supports a toFudgeMsg
	/// function of the form (in order of search):</para>
	/// <ol>
	/// <li>void toFudgeMsg (FudgeSerialisationContext, IMutableFudgeFieldContainer)</li>
	/// <li>void toFudgeMsg (FudgeMessageFactory, IMutableFudgeFieldContainer)</li>
	/// <li>FudgeMsg toFudgeMsg (FudgeSerialisationContext)</li>
	/// <li>FudgeMsg toFudgeMsg (FudgeMessageFactory)</li>
	/// <li>void toFudgeMsg (FudgeContext, IMutableFudgeFieldContainer)</li>
	/// <li>FudgeMsg toFudgeMsg (FudgeContext)</li>
	/// </ol>
	/// </summary>
	/// @param <T> class that can be serialized using this builder
	/// @author Andrew Griffin </param>
	/* package */	 internal abstract class ToFudgeMsgMessageBuilder<T> : IFudgeMessageBuilder
	 {

	  /// <summary>
	  /// Attempts to create a new <seealso cref="ToFudgeMsgMessageBuilder"/> for a class.
	  /// </summary>
	  /// @param <T> class to build messages for </param>
	  /// <param name="clazz"> class to build messages for </param>
	  /// <returns> Returns the {@code ToFudgeMsgMessageBuilder} or {@code null} if none is available </returns>
	  /* package *///JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: static <T> ToFudgeMsgMessageBuilder<T> create(final Class clazz)
	 internal static ToFudgeMsgMessageBuilder<T> create(Type clazz)
	 {
		try
		{
		  return new AddFields<T> (clazz.GetMethod("toFudgeMsg"), false);
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

	  private readonly MethodInfo _toFudgeMsg;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private ToFudgeMsgMessageBuilder(final Method toFudgeMsg)
	  private ToFudgeMsgMessageBuilder(MethodInfo toFudgeMsg)
	  {
		_toFudgeMsg = toFudgeMsg;
	  }

	  /// <summary>
	  /// Invoke the {@code toFudgeMsg} method on the object.
	  /// </summary>
	  /// <param name="obj"> object to invoke the method on </param>
	  /// <param name="args"> parameters to pass </param>
	  /// <returns> the value returned by the {@code toFudgeMsg} if any </returns>
	  protected internal virtual object invoke(object obj, params object[] args)
	  {
		try
		{
		  return _toFudgeMsg.Invoke(obj, args);
		}
		catch (System.ArgumentException e)
		{
		  throw new FudgeRuntimeException("Couldn't call 'toFudgeMsg' on '" + obj + "'", e);
		}
        catch (MissingMethodException e)
		{
		  throw new FudgeRuntimeException("Couldn't call 'toFudgeMsg' on '" + obj + "'", e);
		}
		catch (TargetInvocationException e)
		{
		  throw new FudgeRuntimeException("Couldn't call 'toFudgeMsg' on '" + obj + "'", e);
		}
	  }

	  private class CreateMessage<T> : ToFudgeMsgMessageBuilder<T>
	  {

		internal readonly bool _passContext;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private CreateMessage(final Method toFudgeMsg, final boolean passContext)
        internal CreateMessage(MethodInfo toFudgeMsg, bool passContext)
            : base(toFudgeMsg)
		{
		  _passContext = passContext;
		}

		public override IMutableFudgeFieldContainer BuildMessage(FudgeSerializer context, T @object)
		{
            Object[] param;
            if (_passContext)
                param = new Object[] { context.FudgeContext };
            else
                param = new Object[] { context };
            return (IMutableFudgeFieldContainer)invoke(@object, param);
		}

	  }

	  private class AddFields<T> : ToFudgeMsgMessageBuilder<T>
	  {

		internal readonly bool _passContext;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: private AddFields(final Method toFudgeMsg, final boolean passContext)
        internal AddFields(MethodInfo toFudgeMsg, bool passContext)
            : base(toFudgeMsg)
		{
		  _passContext = passContext;
		}

		public override IMutableFudgeFieldContainer BuildMessage(FudgeSerializer context, T @object)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer msg = context.newMessage();
		  IMutableFudgeFieldContainer msg = context.NewMessage();
          //invoke(@object, _passContext ? context.FudgeContext : context, msg);
          Object[] param;
          if (_passContext)
              param = new Object[] { context.FudgeContext };
          else
              param = new Object[] { context, msg };

          invoke(@object, param);
		  return msg;
		}

	  }

	 }
}