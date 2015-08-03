using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.yutuo.Laxer.Entities.Nodes
{
    class DataNode : Node
    {
        public const char Static = '{';
        public const char Row = '[';

        public readonly char Type;
        public readonly string Key;

        public DataNode(char type, string key)
        {
            Type = type;
            Key = key;
            TrySetCompleted();
        }

        public static char[] GetMatchChars()
        {
            return new char[] { Static, Row };
        }

        public override ResultValue getValue(Dictionary<String, Object> staticValues, Dictionary<String, Object> rowValus)
        {
            return null;
        }

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
                this.Parent = node;
                ((OperateNode)node).Left = this;
            }
            else
            {
                throw new LaxerParseException();
            }
        }

        public override string ToString()
        {
            if (Type == '{')
            {
                return "{" + this.Key.ToString() + "}";
            }
            else
            {
                return "[" + this.Key.ToString() + "]";
            }
            
        }

        public override List<Node> GetUnCompletedNodes()
        {
            return new List<Node>();
        }
    }
}
