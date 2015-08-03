using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.yutuo.Laxer.Entities
{
    class ResultNumberValue : ResultValue
    {
        public readonly decimal Value;

        public ResultNumberValue(decimal value)
        {
            this.Value = value;
        }

        public override string StrValue()
        {
            return this.Value.ToString();
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public override IComparable ComparaValue()
        {
            return this.Value;
        }
    }
}
