using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fudge
{
    public abstract class MutableFudgeMsg : FudgeMsg
    {
        public abstract void Add(IFudgeField field);
        public abstract void Add(String name, Object value);
        public abstract void Add(int ordinal, Object value);
        public abstract void Add(String name, int ordinal, Object value);
        public abstract void Add(String name, int ordinal, FudgeFieldType type, Object value);
        public abstract MutableFudgeMsg AddSubMessage(String name, int ordinal);
        public abstract MutableFudgeMsg EnsureSubMessage(String name, int ordinal);
        public abstract void Remove(String name);
        public abstract void Remove(int ordinal);
        public abstract void Remove(String name, int ordinal);
        public abstract void Clear();

    }
}
