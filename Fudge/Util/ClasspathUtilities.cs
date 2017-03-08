using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection; 

namespace Fudge.Util
{
    /// <summary>
    /// ClasspathUtilities is utility class for equivalent to Refrection class in Java.
    /// </summary>
    public class ClasspathUtilities
    {
        public static List<String> getClassNamesWithAnnotation<T>() where T:Attribute
        {
            var retVal = new List<String>();

            foreach (var assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] ts = assem.GetTypes();
                foreach (Type t in ts)
                {
                    if (t.GetCustomAttribute<T>(true) != null)
                    {
                        retVal.Add(t.FullName);
                    }
                }
            }

            return retVal;
        }
    }
}
