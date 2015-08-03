using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.yutuo.Laxer.Entities
{
    class ResultNullValue : ResultValue
    {
        public static ResultNullValue Instance = new ResultNullValue();

        public ResultNullValue()
        {
        }

        public override string StrValue()
        {
            return "";
        }

        public override string ToString()
        {
            return "null";
        }

        public override IComparable ComparaValue()
        {
            return null;
        }
    }
}
