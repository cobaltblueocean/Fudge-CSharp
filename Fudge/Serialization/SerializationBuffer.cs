using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fudge.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public class SerializationBuffer
    {
        private Stack<Object> _buffer = new Stack<Object>();

        /// <summary>
        /// Buffer used for serialization and deserialization contexts that can detect cycles.
        /// 
        /// Some structures are graphs that contain cycles.
        /// When the method for processing object graphs has been agreed, this will process
        /// back and forward references.
        /// </summary>
        public SerializationBuffer()
        {
        }

        /// <summary>
        /// Registers the start of an object being processed. During serialization can detect a loop
        /// and raise a FudgeRuntimeException.
        /// 
        /// </summary>
        /// <param name="obj">the object currently being processed</param>
        public void BeginObject(Object obj)
        {
            if (_buffer.Contains(obj))
            {
                throw new NotSupportedException("Serialization framework does not support cyclic references: " +
                    obj + " " + (obj != null ? obj.GetType().Name : "") + ", current buffer: " + _buffer);
            }
            _buffer.Push(obj);
        }

        /// <summary>
        /// Registers the end of an object being processed.
        /// </summary>
        /// <param name="obj">the object being processed</param>
        public void EndObject(Object obj)
        {
            Object obj2 = _buffer.Pop();

#if DEBUG
            System.Diagnostics.Debug.Assert(obj2 == obj);
#endif
            System.Diagnostics.Trace.Assert(obj2 == obj);
        }

        /// <summary>
        /// Resets the state of the buffer.
        /// </summary>
        public void Reset()
        {
            _buffer.Clear();
        }
    }
}
