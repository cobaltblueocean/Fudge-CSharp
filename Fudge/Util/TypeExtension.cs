using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fudge.Util
{
    static class TypeExtension
    {
        public static Type newInstance(this Type t)
        {
            return (Type)Activator.CreateInstance(t);
        }
    }
}
