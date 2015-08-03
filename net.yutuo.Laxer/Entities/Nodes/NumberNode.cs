using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.yutuo.Laxer.Entities.Nodes
{
    class NumberNode : ValueNode
    {
        public readonly decimal Value;

        public NumberNode(string value)
        {
            Value = decimal.Parse(value);
            TrySetCompleted();
        }        

        public override ResultValue getValue(Dictionary<String, Object> staticValues, Dictionary<String, Object> rowValus)
        {
            return new ResultNumberValue(this.Value);
        }

        public override string ToString()
        {
            // TODO
            return this.Value.ToString();
        }
    }
}
