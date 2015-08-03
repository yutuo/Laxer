using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.yutuo.Laxer.Entities.Nodes
{
    class DateNode : ValueNode
    {
        public readonly DateTime Value;

        public DateNode(string value)
        {
            Value = DateTime.Parse(value);
            TrySetCompleted();
        }        

        public override ResultValue getValue(Dictionary<String, Object> staticValues, Dictionary<String, Object> rowValus)
        {
            return new ResultDateValue(this.Value);
        }

        public override string ToString()
        {
            return "#" + this.Value.ToString() + "#";

        }
    }
}
