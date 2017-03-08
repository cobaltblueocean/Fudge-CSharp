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
	/// Deserialises Java objects from an underlying stream of Fudge messages.
	/// 
	/// @author Andrew Griffin
	/// </summary>
	public class FudgeObjectReader
	{

	  private readonly FudgeMsgReader _messageReader;

	  private FudgeDeserializer _deserialisationContext;

	  /// <summary>
	  /// Creates a new <seealso cref="FudgeObjectReader"/> around the underlying <seealso cref="FudgeMsgReader"/> stream.
	  /// </summary>
	  /// <param name="messageReader"> the source of Fudge messages containing serialised objects </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public FudgeObjectReader(final org.fudgemsg.FudgeMsgReader messageReader)
	  public FudgeObjectReader(FudgeMsgReader messageReader)
	  {
		if (messageReader == null)
		{
			throw new System.NullReferenceException("messageReader cannot be null");
		}
		_messageReader = messageReader;
		_deserialisationContext = new FudgeDeserializer(messageReader.FudgeContext);
	  }

	  /// <summary>
	  /// Closes the underlying stream.
	  /// </summary>
	  public virtual void close()
	  {
		if (_messageReader == null)
		{
			return;
		}
		_messageReader.close();
	  }

	  /// <summary>
	  /// Returns the underlying <seealso cref="FudgeContext"/>. This will be the context of the <seealso cref="FudgeMsgReader"/> being used.
	  /// </summary>
	  /// <returns> the {@code FudgeContext} </returns>
	  public virtual FudgeContext FudgeContext
	  {
		  get
		  {
	//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
	//ORIGINAL LINE: final FudgeDeserializationContext context = getDeserialisationContext();
			FudgeDeserializer context = DeserialisationContext;
			if (context == null)
			{
				return null;
			}
			return context.FudgeContext;
		  }
	  }

	  /// <summary>
	  /// Returns the current <seealso cref="FudgeDeserializer"/>. This is associated with the same <seealso cref="FudgeContext"/> as
	  /// the source message stream.
	  /// </summary>
	  /// <returns> the {@code FudgeDeserialisationContext} </returns>
	  public virtual FudgeDeserializer DeserialisationContext
	  {
		  get
		  {
			return _deserialisationContext;
		  }
	  }

	  /// <summary>
	  /// Returns the underlying message source.
	  /// </summary>
	  /// <returns> the <seealso cref="FudgeMsgReader"/> </returns>
	  public virtual FudgeMsgReader MessageReader
	  {
		  get
		  {
			return _messageReader;
		  }
	  }

	  /// <summary>
	  /// Returns {@code true} if the underlying message source has another message and <seealso cref="#read()"/> or <seealso cref="#read(Class)"/> can be
	  /// called.
	  /// </summary>
	  /// <returns> {@code true} if there are more messages to be deserialized, {@code false} otherwise </returns>
	  public virtual bool hasNext()
	  {
		return MessageReader.HasNext();
	  }

	  /// <summary>
	  /// Reads the next message from the underlying source and deserializes it to a Java object.
	  /// </summary>
	  /// <returns> the Java object </returns>
	  public virtual object read()
	  {
		IFudgeFieldContainer message = MessageReader.NextMessage();
		DeserialisationContext.reset();
		return DeserialisationContext.fudgeMsgToObject(message);
	  }

	  /// <summary>
	  /// Reads the next message from the underlying source and deserializes it to the requested Java type.
	  /// </summary>
	  /// @param <T> Java type of the requested object </param>
	  /// <param name="clazz"> Java class of the requested object </param>
	  /// <returns> the Java object </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not allowed in .NET:
//ORIGINAL LINE: public <T> T read(final Class clazz)
	  public virtual T read<T>(Type clazz)
	  {
		IFudgeFieldContainer message = MessageReader.NextMessage();
		DeserialisationContext.reset();
		return DeserialisationContext.fudgeMsgToObject(clazz, message);
	  }

	}
}