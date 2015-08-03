using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.yutuo.Laxer.Entities.Nodes
{
    public abstract class Node
    {
        public Node Parent { get; set; }        
        public bool IsComplete { get; set; }
        public abstract ResultValue getValue(Dictionary<String, Object> staticValues, Dictionary<String, Object> rowValus);
        public void SetUnComplete() {
            Node temp = this;
            while (temp != null)
            {
                temp.IsComplete = false;
                temp = temp.Parent;
            }
        }

        public abstract List<Node> GetUnCompletedNodes();
        public abstract void TrySetCompleted();
        public abstract void AddNode(Node node);
    }
}
