using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.yutuo.Laxer.Entities.Nodes
{
    class BoolNode : ValueNode
    {
        public const string TrueString = "true";
        public const string FalseString = "false";

        public readonly bool Value;

        public BoolNode(string value) {
            Value = bool.Parse(value);
            TrySetCompleted();
        }        

        public override ResultValue getValue(Dictionary<String, Object> staticValues, Dictionary<String, Object> rowValus)
        {
            return new ResultBoolValue(this.Value);
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
