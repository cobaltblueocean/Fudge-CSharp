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
using System.Collections.Generic;
using Fudge.Serialization;

namespace Fudge.Mapping
{


	/// <summary>
	/// Builder wrapper for objects that are already Fudge messages. The FudgeFieldContainer class name is added
	/// so that the serialization framework will decode the messages as messages and not as serialized objects.
	/// 
	/// @author Andrew Griffin
	/// </summary>
	/* package */	 internal class FudgeFieldContainerBuilder : IFudgeBuilder<IFudgeFieldContainer>
	 {

	  /// 
	  /* package */	 internal static readonly IFudgeBuilder<IFudgeFieldContainer> INSTANCE = new FudgeFieldContainerBuilder();

	  private FudgeFieldContainerBuilder()
	  {
	  }

	  /// 
	  public override IMutableFudgeFieldContainer buildMessage(FudgeSerializer context, IFudgeFieldContainer fields)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer msg = context.newMessage(fields);
		IMutableFudgeFieldContainer msg = context.newMessage(fields);
		// add the interface name
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		msg.add(null, 0, typeof(IFudgeFieldContainer).FullName);
		return msg;
	  }

	  /// 
	  public override IFudgeFieldContainer buildObject(FudgeDeserializer context, IFudgeFieldContainer message)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer msg = context.getFudgeContext().newMessage(message);
		IMutableFudgeFieldContainer msg = context.FudgeContext.NewMessage(message);
		// remove the class name(s) if added
		const short? ordinal = 0;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Iterator<org.fudgemsg.FudgeField> fields = msg.iterator();
		IEnumerator<IFudgeField> fields = msg.GetEnumerator();
		while (fields.MoveNext())
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.fudgemsg.FudgeField field = fields.Current;
		  IFudgeField field = fields.Current;
		  if (ordinal.Equals(field.Ordinal) && (field.Name == null))
		  {
			fields.remove();
			break;
		  }
		}
		return msg;
	  }

	 }
}