using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fudge
{
    public class StandardFudgeMsg: FudgeMsg
    {
        public StandardFudgeMsg(FudgeContext context, FudgeMsg msg)
            : base(context, (IFudgeField[])msg.GetAllFields())
        {
        }
    }
}
