using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using net.yutuo.Laxer.Entities.Common;


namespace net.yutuo.Laxer.Entities.Nodes
{
    class FunctionNode : Node
    {
        private bool paramEnded = true;
        public readonly Function Function;
        public List<Node> Params { get; set; }

        public FunctionNode(Function function)
        {
            Function = function;
            Params = new List<Node>();
        }

        public static Function GetFunction(string functionName)
        {
            return Function.FunctionDict[functionName.ToUpper()];
        }

        public static string[] GetMatchStrings()
        {
            return Function.FunctionDict.Keys.ToArray();
        }

        public override ResultValue getValue(Dictionary<String, Object> staticValues, Dictionary<String, Object> rowValus)
        {
            List<ResultValue> paramValus = new List<ResultValue>();
            foreach (Node param in Params)
            {
                paramValus.Add(param.getValue(staticValues, rowValus));
            }
            return Function.Count(paramValus);
        }

        public override void TrySetCompleted()
        {
            return;
        }

        public void SetParamCompleted()
        {
            if (paramEnded)
            {
                throw new LaxerParseException();
            }
            if (!Params.Last().IsComplete)
            {
                throw new LaxerParseException();
            }
            else
            {
                paramEnded = true;
            }
        }

        public void SetCompleted()
        {
            if (!paramEnded || Params.Count == 0)
            {
                if (Params.Count < Function.MinParam)
                {
                    throw new LaxerParseException();
                }
                else
                {
                    paramEnded = true;
                    IsComplete = true;
                    if (this.Parent != null)
                    {
                        this.Parent.TrySetCompleted();
                    }
                }                
            }
            else 
            {
                throw new LaxerParseException();
            }
        }

        public override void AddNode(Node node)
        {
            // TODO Check Param Count
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
            else if (paramEnded || Params.Count == 0)
            {
                if (Params.Count > Function.MaxParam)
                {
                    throw new LaxerParseException();
                }
                else
                {
                    node.Parent = this;
                    Params.Add(node);
                    paramEnded = false;
                }                
            }
            else
            {
                Node last = Params.Last();
                last.AddNode(node);
                while (!Object.ReferenceEquals(this, last.Parent))
                {
                    last = last.Parent;
                }
                Params[Params.Count - 1] = last;
            }
        }

        public override string ToString()
        {
            string result = Function.Name + "(";
            for (int i = 0; i < Params.Count; i++)
            {
                if (i > 0)
                {
                    result += ", ";
                }
                result += Params[i].ToString();
            }

            result += ")";
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
                for (int i = 0; i < Params.Count; i++)
                {
                    if (!Params[i].IsComplete)
                    {
                        result.AddRange(Params[i].GetUnCompletedNodes());
                    }
                }

                return result;
            }
        }
    }
}
