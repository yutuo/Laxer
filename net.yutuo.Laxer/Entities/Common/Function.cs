using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using net.yutuo.Laxer.Entities.Nodes;

namespace net.yutuo.Laxer.Entities.Common
{
    class Function
    {
        public delegate ResultValue CountMethod(List<ResultValue> paramList);

        public readonly string Name;
        public readonly int MinParam;
        public readonly int MaxParam;
        public readonly CountMethod Count;

        private static Dictionary<String, Function> functionDict;

        public static Dictionary<String, Function> FunctionDict
        {
            get
            {
                if (functionDict == null)
                {
                    initFunction();
                }
                return functionDict;
            }
        }

        private static void initFunction()
        {
            functionDict = new Dictionary<String, Function>();
            functionDict.Add("NOT", new Function("Not", 1, 1, Not));

            functionDict.Add("SUBSTRING", new Function("SubString", 2, 3, SubString));
            functionDict.Add("TRIM", new Function("Trim", 1, 2, TODOFUC));
            functionDict.Add("LEFTTRIM", new Function("LeftTrim", 1, 2, TODOFUC));
            functionDict.Add("RIGHTTRIM", new Function("RightTrim", 1, 2, TODOFUC));
            functionDict.Add("PAD", new Function("Pad", 1, 2, TODOFUC));
            functionDict.Add("LEFTPAD", new Function("LeftPad", 1, 2, TODOFUC));
            functionDict.Add("REPLACE", new Function("Replace", 3, 3, TODOFUC));
            functionDict.Add("REGREPLACE", new Function("RegReplace", 3, 3, TODOFUC));
            functionDict.Add("IF", new Function("If", 2, 3, TODOFUC));
            functionDict.Add("IFNULL", new Function("IfNull", 2, 3, TODOFUC));
            functionDict.Add("IFBLANK", new Function("IfBlank", 2, 3, TODOFUC));
            functionDict.Add("IFZERO", new Function("IfZero", 2, 3, TODOFUC));
            functionDict.Add("FORMAT", new Function("Format", 2, 2, TODOFUC));
        }

        private Function(string name, int minParam, int maxParam, CountMethod count)
        {
            Name = name;
            MinParam = minParam;
            MaxParam = maxParam;
            Count = count;
        }

        private static ResultValue Not(List<ResultValue> paramList)
        {
            if (ResultValue.HasNullResult(paramList))
            {
                return ResultNullValue.Instance;
            }

            if (!(paramList[0] is ResultBoolValue))
            {
                throw new LaxerCalculateException();
            }
            bool result = !((ResultBoolValue)paramList[0]).Value;
            return new ResultBoolValue(result);
        }

        private static ResultValue SubString(List<ResultValue> paramList)
        {
            if (ResultValue.HasNullResult(paramList))
            {
                return ResultNullValue.Instance;
            }

            if (!(paramList[0] is ResultStringValue) || !(paramList[1] is ResultNumberValue))
            {
                throw new LaxerCalculateException();
            }

            if (paramList.Count == 3 && !(paramList[2] is ResultNumberValue))
            {
                throw new LaxerCalculateException();
            }

            string baseString = ((ResultStringValue)paramList[0]).Value;
            int stratIndex = Decimal.ToInt32(((ResultNumberValue)paramList[1]).Value);
            int length;

            if (paramList.Count == 3)
            {
                length = Decimal.ToInt32(((ResultNumberValue)paramList[2]).Value);
            }
            else
            {
                length = baseString.Length - stratIndex;
            }
            string result = baseString.Substring(stratIndex, length);
            return new ResultStringValue(result);
        }

        private static ResultValue TODOFUC(List<ResultValue> paramList)
        {
            return null;
        }
    }
}
