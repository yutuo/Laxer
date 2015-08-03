using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.yutuo.Laxer.Entities
{
    public abstract class ResultValue
    {
        public abstract IComparable ComparaValue();

        public abstract string StrValue();

        public static bool HasNullResult(params ResultValue [] values)
        {
            if (values == null)
            {
                return false;
            }

            foreach (ResultValue value in values)
            {
                if (value is ResultNullValue)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasNullResult(List<ResultValue> values)
        {
            if (values == null)
            {
                return false;
            }

            foreach (ResultValue value in values)
            {
                if (value is ResultNullValue)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
