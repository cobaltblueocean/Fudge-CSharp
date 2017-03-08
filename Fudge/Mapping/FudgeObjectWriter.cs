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
using Fudge.Serialization;

namespace Fudge.Mapping
{


	/// <summary>
	/// Serialises Java objects to a target Fudge message stream.
	/// 
	/// @author Andrew Griffin
	/// </summary>
	public class FudgeObjectWriter
	{

	  private readonly FudgeMsgWriter _messageWriter;

	  private FudgeSerializer _serialisationContext;

	  /// <summary>
	  /// Creates a new <seealso cref="FudgeObjectWriter"/> around a <seealso cref="FudgeMsgWriter"/>.
	  /// </summary>
	  /// <param name="messageWriter"> the target for Fudge messages </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public FudgeObjectWriter(final org.fudgemsg.FudgeMsgWriter messageWriter)
	  public FudgeObjectWriter(FudgeMsgWriter messageWriter)
	  {
		if (messageWriter == null)
		{
			throw new System.NullReferenceException("messageWriter cannot be null");
		}
		_messageWriter = messageWriter;
		_serialisationContext = new FudgeSerializer(messageWriter.FudgeContext);
	  }

	  /// <summary>
	  /// Closes the underlying target stream.
	  /// </summary>
	  public virtual void close()
	  {
		if (_messageWriter == null)
		{
			return;
		}
		_messageWriter.Close();
	  }

	  /// <summary>
	  /// Returns the underlying <seealso cref="FudgeContext"/>. This will be the context of the <seealso cref="FudgeMsgWriter"/> being used.
	  /// </summary>
	  /// <returns> the {@code FudgeContext} </returns>
	  public virtual FudgeContext FudgeContext
	  {
		  get
		  {
	//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
	//ORIGINAL LINE: final FudgeSerializer context = getSerialisationContext();
			FudgeSerializer context = SerialisationContext;
			if (context == null)
			{
				return null;
			}
			return context.FudgeContext;
		  }
	  }

	  /// <summary>
	  /// Returns the current <seealso cref="FudgeSerializer"/>. This is associated with the same <seealso cref="FudgeContext"/> as
	  /// the target message stream.
	  /// </summary>
	  /// <returns> the {@code FudgeSerialisationContext} </returns>
	  public virtual FudgeSerializer SerialisationContext
	  {
		  get
		  {
			return _serialisationContext;
		  }
	  }

	  /// <summary>
	  /// Returns the underlying message target.
	  /// </summary>
	  /// <returns> the <seealso cref="FudgeMsgWriter"/> </returns>
	  public virtual FudgeMsgWriter MessageWriter
	  {
		  get
		  {
			return _messageWriter;
		  }
	  }

	  /// <summary>
	  /// Serialises a Java object to a Fudge message and writes it to the target stream.
	  /// </summary>
	  /// @param <T> type of the Java object </param>
	  /// <param name="obj"> the object to write </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public <T> void write(final T obj)
	  public virtual void write<T>(T obj)
	  {
		SerialisationContext.reset();
		IFudgeFieldContainer message;
		if (obj == null)
		{
		  // write an empty message
		  message = SerialisationContext.newMessage();
		}
		else
		{
		  // delegate to a message builder
		  message = SerialisationContext.ObjectToFudgeMsg(obj);
		}
		MessageWriter.writeMessage(message, 0);
	  }

	}
}