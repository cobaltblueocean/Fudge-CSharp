using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fudge.Util
{
    public static class EnumExtension
    {
        public static Enum valueOf(this Enum enumration, String item)
        {
            foreach (var e in Enum.GetValues(enumration.GetType()))
            {
                string eName = Enum.GetName(enumration.GetType(), (int)e);
                if (eName == item)
                    return (Enum)e;
            }
            return null;
        }
    }
}
