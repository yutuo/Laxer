using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using net.yutuo.Laxer.Entities.Common;

namespace net.yutuo.Laxer.Entities.Nodes
{
    class OperateNode : Node
    {
        public readonly Operate Operate;
        public Node Left { get; set; }
        public Node Right { get; set; }

        public OperateNode(Operate operate)
        {
            Operate = operate;
        }

        public static Operate GetOperate(char operateChar)
        {
            return Operate.OperateDict[operateChar];
        }

        public static char[] GetMatchChars()
        {
            return Operate.OperateDict.Keys.ToArray();
        }

        public override ResultValue getValue(Dictionary<String, Object> staticValues, Dictionary<String, Object> rowValus)
        {
            ResultValue leftResult = null;
            if (Left != null)
            {
                leftResult = Left.getValue(staticValues, rowValus);
            }
            ResultValue rightResult = Right.getValue(staticValues, rowValus);
            return Operate.Count(leftResult, rightResult);
        }

        public override void TrySetCompleted()
        {
            if (Right == null)
            {
                return;
            }
            else
            {
                IsComplete = true;
                if (this.Parent != null)
                {
                    this.Parent.TrySetCompleted();
                }
            }
        }

        public override void AddNode(Node node)
        {
            if (this.Right == null)
            {
                if ((node is OperateNode))
                {
                    throw new LaxerParseException();
                }
                else
                {
                    node.Parent = this;
                    this.Right = node;
                    if (node.IsComplete)
                    {
                        this.TrySetCompleted();
                    }
                }
            }
            else
            {
                if (!IsComplete)
                {
                    this.Right.AddNode(node);
                }
                else
                {
                    if (!(node is OperateNode))
                    {
                        throw new LaxerParseException();
                    }

                    this.SetUnComplete();

                    OperateNode operateNode = (OperateNode)node;
                    if (this.Operate.Priority < operateNode.Operate.Priority)
                    {
                        operateNode.Left = this.Right;
                        operateNode.Parent = this;

                        this.Right = operateNode;
                    }
                    else
                    {
                        Node parent = this.Parent;
                        operateNode.Left = this;
                        this.Parent = operateNode;
                        operateNode.Parent = parent;

                        if (parent is OperateNode)
                        {
                            OperateNode operateParent = (OperateNode)parent;
                            operateParent.Right = null;
                            operateParent.AddNode(operateNode);
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            string result = Operate.OperateChar.ToString() + " " + Left + " " + Right;
            return result;
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
                if (Right != null)
                {
                    result.AddRange(Right.GetUnCompletedNodes());
                }

                return result;
            }
        }
    }
}
