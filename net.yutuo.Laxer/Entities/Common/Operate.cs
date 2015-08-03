using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using net.yutuo.Laxer.Entities;

namespace net.yutuo.Laxer.Entities.Common
{
    class Operate
    {
        public delegate ResultValue CountMethod(ResultValue left, ResultValue right);

        public readonly string Name;
        public readonly char OperateChar;
        public readonly int Priority;
        public readonly CountMethod Count;

        private static Dictionary<char, Operate> operateDict;

        public static Dictionary<char, Operate> OperateDict
        {
            get
            {
                if (operateDict == null)
                {
                    initOperates();
                }
                return operateDict;
            }
        }

        private static void initOperates()
        {
            operateDict = new Dictionary<char, Operate>();

            operateDict.Add('^', new Operate("Xor", '^', 10, XorAndPower));
            operateDict.Add('|', new Operate("Or", '|', 10, Or));
            operateDict.Add('&', new Operate("And", '&', 20, And));

            operateDict.Add('>', new Operate("Greater", '>', 30, Greater));
            operateDict.Add('<', new Operate("Lesser", '<', 30, Lesser));
            operateDict.Add('=', new Operate("Equal", '=', 30, Equal));
            operateDict.Add('~', new Operate("NotEqual", '~', 30, NotEqual));

            operateDict.Add('+', new Operate("Add", '+', 110, Add));
            operateDict.Add('-', new Operate("Subtract", '-', 110, Subtract));
            operateDict.Add('*', new Operate("Multiply", '*', 120, Multiply));
            operateDict.Add('/', new Operate("Divide", '/', 120, Divide));
            operateDict.Add('%', new Operate("Remainder", '%', 120, Remainder));
        }

        private Operate(string name, char operateChar, int priority, CountMethod count)
        {
            Name = name;
            OperateChar = operateChar;
            Priority = priority;
            Count = count;
        }

        private static ResultValue XorAndPower(ResultValue left, ResultValue right)
        {
            if (ResultValue.HasNullResult(left, right))
            {
                return ResultNullValue.Instance;
            }

            if ((left is ResultNumberValue) && (right is ResultNumberValue))
            {
                double result = Math.Pow(Decimal.ToDouble(((ResultNumberValue)left).Value), Decimal.ToDouble(((ResultNumberValue)right).Value));
                return new ResultNumberValue(new decimal(result));
            }
            else if ((left is ResultBoolValue) && (right is ResultBoolValue))
            {
                bool result = ((ResultBoolValue)left).Value ^ ((ResultBoolValue)right).Value;
                return new ResultBoolValue(result);
            }
            else
            {
                throw new LaxerCalculateException();
            }
        }

        private static ResultValue Or(ResultValue left, ResultValue right)
        {
            if (ResultValue.HasNullResult(left, right))
            {
                return ResultNullValue.Instance;
            }

            if ((left is ResultBoolValue) && (right is ResultBoolValue))
            {
                bool result = ((ResultBoolValue)left).Value || ((ResultBoolValue)right).Value;
                return new ResultBoolValue(result);
            }
            else
            {
                throw new LaxerCalculateException();
            }
        }

        private static ResultValue And(ResultValue left, ResultValue right)
        {
            if (ResultValue.HasNullResult(left, right))
            {
                return ResultNullValue.Instance;
            }

            if ((left is ResultBoolValue) && (right is ResultBoolValue))
            {
                bool result = ((ResultBoolValue)left).Value && ((ResultBoolValue)right).Value;
                return new ResultBoolValue(result);
            }
            else
            {
                throw new LaxerCalculateException();
            }
        }

        private static ResultValue Greater(ResultValue left, ResultValue right)
        {
            if (ResultValue.HasNullResult(left, right))
            {
                return ResultNullValue.Instance;
            }

            if (left.GetType().Equals(right.GetType()))
            {
                bool result = left.ComparaValue().CompareTo(right.ComparaValue()) > 0;
                return new ResultBoolValue(result);
            }
            else
            {
                throw new LaxerCalculateException();
            }
        }

        private static ResultValue Lesser(ResultValue left, ResultValue right)
        {
            if (ResultValue.HasNullResult(left, right))
            {
                return ResultNullValue.Instance;
            }

            if (left.GetType().Equals(right.GetType()))
            {
                bool result = left.ComparaValue().CompareTo(right.ComparaValue()) < 0;
                return new ResultBoolValue(result);
            }
            else
            {
                throw new LaxerCalculateException();
            }
        }

        private static ResultValue Equal(ResultValue left, ResultValue right)
        {
            if (ResultValue.HasNullResult(left, right))
            {
                return ResultNullValue.Instance;
            }

            if (left.GetType().Equals(right.GetType()))
            {
                bool result = left.ComparaValue().CompareTo(right.ComparaValue()) == 0;
                return new ResultBoolValue(result);
            }
            else
            {
                throw new LaxerCalculateException();
            }
        }

        private static ResultValue NotEqual(ResultValue left, ResultValue right)
        {
            if (ResultValue.HasNullResult(left, right))
            {
                return ResultNullValue.Instance;
            }

            if (left.GetType().Equals(right.GetType()))
            {
                bool result = left.ComparaValue().CompareTo(right.ComparaValue()) != 0;
                return new ResultBoolValue(result);
            }
            else
            {
                throw new LaxerCalculateException();
            }
        }

        private static ResultValue Add(ResultValue left, ResultValue right)
        {
            if (ResultValue.HasNullResult(left, right))
            {
                return ResultNullValue.Instance;
            }

            if ((left is ResultNumberValue) && (right is ResultNumberValue))
            {
                decimal result = ((ResultNumberValue)left).Value + ((ResultNumberValue)right).Value;
                return new ResultNumberValue(result);
            }
            else if ((left is ResultDateValue) && (right is ResultNumberValue))
            {
                DateTime result = ((ResultDateValue)left).Value.AddDays(Decimal.ToDouble(((ResultNumberValue)right).Value));
                return new ResultDateValue(result);
            }
            else if ((left is ResultStringValue))
            {
                string result = ((ResultStringValue)left).Value + right.StrValue();
                return new ResultStringValue(result);
            }
            else
            {
                throw new LaxerCalculateException();
            }
        }

        private static ResultValue Subtract(ResultValue left, ResultValue right)
        {
            if (ResultValue.HasNullResult(left, right))
            {
                return ResultNullValue.Instance;
            }

            if ((left is ResultNumberValue) && (right is ResultNumberValue))
            {
                decimal result = ((ResultNumberValue)left).Value - ((ResultNumberValue)right).Value;
                return new ResultNumberValue(result);
            }
            else if ((left is ResultDateValue) && (right is ResultNumberValue))
            {
                DateTime result = ((ResultDateValue)left).Value.AddDays(-Decimal.ToDouble(((ResultNumberValue)right).Value));
                return new ResultDateValue(result);
            }
            else
            {
                throw new LaxerCalculateException();
            }
        }

        private static ResultValue Multiply(ResultValue left, ResultValue right)
        {
            if (ResultValue.HasNullResult(left, right))
            {
                return ResultNullValue.Instance;
            }

            if ((left is ResultNumberValue) && (right is ResultNumberValue))
            {
                decimal result = ((ResultNumberValue)left).Value * ((ResultNumberValue)right).Value;
                return new ResultNumberValue(result);
            }
            else if ((left is ResultStringValue) && (right is ResultNumberValue))
            {
                StringBuilder sb = new StringBuilder();
                string baseString = ((ResultStringValue)left).Value;
                int loopCount = Decimal.ToInt32(((ResultNumberValue)right).Value);
                for (int i = 0; i < loopCount; i++)
                {
                    sb.Append(baseString);
                }
                return new ResultStringValue(sb.ToString());
            }
            else
            {
                throw new LaxerCalculateException();
            }
        }

        private static ResultValue Divide(ResultValue left, ResultValue right)
        {
            if (ResultValue.HasNullResult(left, right))
            {
                return ResultNullValue.Instance;
            }

            if ((left is ResultNumberValue) && (right is ResultNumberValue))
            {
                decimal result = ((ResultNumberValue)left).Value * ((ResultNumberValue)right).Value;
                return new ResultNumberValue(result);
            }
            else
            {
                throw new LaxerCalculateException();
            }
        }

        private static ResultValue Remainder(ResultValue left, ResultValue right)
        {
            if (ResultValue.HasNullResult(left, right))
            {
                return ResultNullValue.Instance;
            }

            if ((left is ResultNumberValue) && (right is ResultNumberValue))
            {
                decimal result = ((ResultNumberValue)left).Value % ((ResultNumberValue)right).Value;
                return new ResultNumberValue(result);
            }
            else
            {
                throw new LaxerCalculateException();
            }
        }
    }
}
