using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.yutuo.Laxer.Entities.Nodes
{
    class BracketNode : Node
    {
        public Node Value { get; set; }

        public BracketNode()
        {
        }

        public override ResultValue getValue(Dictionary<String, Object> staticValues, Dictionary<String, Object> rowValus)
        {
            return Value.getValue(staticValues, rowValus);
        }

        public override void TrySetCompleted()
        {
            return;
        }

        public void SetCompleted()
        {
            IsComplete = true;

            if (Value is FunctionNode)
            {
                ((FunctionNode)Value).SetCompleted();
            }
            else
            {
                if (this.Parent != null)
                {
                    this.Parent.TrySetCompleted();
                }
            }
        }

        public override void AddNode(Node node)
        {
            if (IsComplete)
            {
                if (!(node is OperateNode))
                {
                    throw new LaxerParseException();
                }
                else
                {
                    OperateNode operateNode = (OperateNode)node;
                    operateNode.Left = this;
                    operateNode.Parent = this.Parent;
                    this.Parent = operateNode;
                }
            }
            else if (Value == null)
            {
                node.Parent = this;
                this.Value = node;
            }
            else
            {
                this.Value.AddNode(node);
                while (!Object.ReferenceEquals(this, this.Value.Parent))
                {
                    this.Value = this.Value.Parent;
                }
            }
        }

        public override string ToString()
        {
            if (this.Value is FunctionNode)
            {
                return this.Value.ToString();
            }
            else
            {
                return "(" + this.Value.ToString() + ")";
            }
        }

        public override List<Node> GetUnCompletedNodes()
        {
            if (IsComplete)
            {
                return new List<Node>();
            }
            else
            {
                List<Node> result = new List<Node>() { this };
                if (Value != null)
                {
                    result.AddRange(Value.GetUnCompletedNodes());
                }

                return result;
            }
        }
    }
}
