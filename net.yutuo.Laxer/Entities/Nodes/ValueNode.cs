using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.yutuo.Laxer.Entities.Nodes
{
    abstract class ValueNode : Node
    {
        public override void TrySetCompleted()
        {
            IsComplete = true;
            if (this.Parent != null)
            {
                this.Parent.TrySetCompleted();
            }
        }

        public override void AddNode(Node node)
        {
            if (node is OperateNode)
            {
                Node parent = this.Parent;
                this.Parent = node;
                node.Parent = parent;
                ((OperateNode)node).Left = this;
            }
            else
            {
                throw new LaxerParseException();
            }
        }

        public override List<Node> GetUnCompletedNodes()
        {
            return new List<Node>();
        }
    }
}
