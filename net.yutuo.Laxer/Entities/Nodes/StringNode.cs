using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.yutuo.Laxer.Entities.Nodes
{
    class StringNode : ValueNode
    {
        public readonly string Value;

        public StringNode(string value)
        {
            Value = value;
            TrySetCompleted();
        }

        public override ResultValue getValue(Dictionary<String, Object> staticValues, Dictionary<String, Object> rowValus)
        {
            return new ResultStringValue(this.Value);
        }

        public override string ToString()
        {
            return "\"" + this.Value + "\"";
        }
    }
}
