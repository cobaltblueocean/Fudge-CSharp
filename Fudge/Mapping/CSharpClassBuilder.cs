using System;
using Fudge.Serialization;

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

    using StringFieldType = Fudge.Types.StringFieldType;

    /// <summary>
    /// Builder for Class objects. This is a trivial hack so that getClass can be processed safely
    /// by the Bean based serialisers. The class object is reduced to just the name.
    /// 
    /// @author Andrew Griffin
    /// </summary>
    /* package */
    internal class CSharpClassBuilder : IFudgeBuilder<Type>
    {

        /// 
        /* package */
        internal static readonly IFudgeBuilder<Type> INSTANCE = new CSharpClassBuilder();

        private CSharpClassBuilder()
        {
        }

        /// 
        public IMutableFudgeFieldContainer BuildMessage(FudgeSerializer context, Type @object)
        {
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final org.fudgemsg.IMutableFudgeFieldContainer msg = context.newMessage();
            MutableFudgeMsg msg = context.NewMessage();
            FudgeSerializer.AddClassHeader(msg, @object.GetType());
            //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
            msg.Add("name", null, StringFieldType.Instance, @object.FullName);
            return msg;
        }

        /// 
        public Type BuildObject(FudgeDeserializer context, IFudgeFieldContainer message)
        {
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final String str = message.getString("name");
            string str = message.GetString("name");
            if (str == null)
            {
                throw new System.ArgumentException("Sub-message doesn't contain a Java class name");
            }
            try
            {
                return Type.GetType(str);
            }
            catch (TypeLoadException e)
            {
                throw new FudgeRuntimeException("Cannot deserialise Java class '" + str + "'", e);
            }
        }


        public IMutableFudgeFieldContainer BuildMessage(FudgeSerializer serializer, dynamic @object)
        {
            throw new NotImplementedException();
        }
    }
}