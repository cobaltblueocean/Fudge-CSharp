using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fudge.Serialization;
using Fudge.Types;
using Fudge.Wire.Types;

namespace Fudge
{
    /// <summary>
    /// Abstract Fudge builder.
    /// </summary>
    public abstract class AbstractFudgeBuilder
    {
        /// <summary>
        /// Adds an object to the specified message if non-null.
        /// </summary>
        /// <param name="msg">the msg to populate, not null</param>
        /// <param name="fieldName">the field name, may be null</param>
        /// <param name="value">the value, null ignored</param>
        protected static void addToMessage(MutableFudgeMsg msg, String fieldName, Object value)
        {
            if (value != null)
            {
                if (value is String)
                {
                    msg.Add(fieldName, null, FudgeWireType.STRING, value);
                    //} else if (value is Enumerable ) {
                    //  msg.Add(fieldName, null, FudgeWireType.STRING, ((Enum<?>) value).name());
                }
                else
                {
                    msg.Add(fieldName, null, value);
                }
            }
        }

        protected static void addToMessage<T>(FudgeSerializer serializer, MutableFudgeMsg msg, String fieldName, T value, Type declaredType)
        {
            if (value != null)
            {
                MutableFudgeMsg subMsg = serializer.NewMessage();
                FudgeSerializer.AddClassHeader(subMsg, value.GetType(), declaredType);
                FudgeMsg builtMsg = serializer.ObjectToFudgeMsg(value);
                foreach (IFudgeField field in builtMsg)
                {
                    subMsg.Add(field);
                }
                msg.Add(fieldName, null, subMsg);
            }
        }
    }
}
