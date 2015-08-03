using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.yutuo.Laxer.Entities
{
    class ResultStringValue : ResultValue
    {
        public readonly string Value;

        public ResultStringValue(string value)
        {
            this.Value = value;
        }

        public override string StrValue()
        {
            return this.Value;
        }

        public override string ToString()
        {
            return "\"" + this.Value + "\"";
        }

        public override IComparable ComparaValue()
        {
            return this.Value;
        }
    }
}
