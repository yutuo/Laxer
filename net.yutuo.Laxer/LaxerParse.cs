using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using net.yutuo.Laxer.Entities.Nodes;
using net.yutuo.Laxer.Entities.Common;

namespace net.yutuo.Laxer
{

    public class LaxerParse
    {
        private TextReader reader;
        private Node rootNode;
        private int col;
        private StringBuilder originalValue = new StringBuilder();

        public Node Parse(string value)
        {
            return this.Parse(new StringReader(value));
        }

        public Node Parse(TextReader reader)
        {
            this.reader = reader;
            this.rootNode = null;

            while (this.Next() > 0) { }
            return this.rootNode;
        }

        private int Next()
        {
            int nextCharInt;
            while ((nextCharInt = ReaderRead()) != -1)
            {
                char nextChar = (char)nextCharInt;
                if (nextChar == ' ' || nextChar == '\t')
                {
                    continue;
                }
                else if (nextChar == '\'')
                {
                    string value = ReadString();
                    this.AddToNode(new StringNode(value));
                }
                else if (Char.IsDigit(nextChar))
                {
                    string value = ReadDigit(nextChar);
                    this.AddToNode(new NumberNode(value));
                }
                else if (nextChar == '#')
                {
                    string value = ReadStartEnd(nextChar);
                    this.AddToNode(new DateNode(value));
                }
                else if (nextChar == '(')
                {
                    this.AddToNode(new BracketNode());
                }
                else if (nextChar == ')')
                {
                    SetBracketEnd();
                }
                else if (nextChar == ',')
                {
                    SetParamEnd();
                }
                else if (DataNode.GetMatchChars().Contains(nextChar))
                {
                    string value = ReadStartEnd(nextChar);
                    this.AddToNode(new DataNode(nextChar, value));
                }
                else if (OperateNode.GetMatchChars().Contains(nextChar))
                {
                    Operate operate = OperateNode.GetOperate(nextChar);
                    this.AddToNode(new OperateNode(operate));
                }
                else if (Char.IsLetter(nextChar) || '_' == nextChar)
                {
                    string value = ReadIdent(nextChar);
                    string indent = value.ToUpper();
                    if ("TRUE" == indent || "FALSE" == indent)
                    {
                        this.AddToNode(new BoolNode(value));
                    }
                    else if (FunctionNode.GetMatchStrings().Contains(indent))
                    {
                        Function function = FunctionNode.GetFunction(indent);
                        this.AddToNode(new FunctionNode(function));
                    }
                    else
                    {
                        throw new LaxerParseException();
                    }
                }
                else
                {
                    throw new LaxerParseException();
                }
            }

            return 0;
        }

        private void AddToNode(Node node)
        {
            if (this.rootNode == null)
            {
                this.rootNode = node;
            }
            else
            {
                this.rootNode.AddNode(node);
                while (node.Parent != null)
                {
                    node = node.Parent;
                }
                this.rootNode = node;
            }
        }


        private void SetBracketEnd()
        {
            if (this.rootNode == null || this.rootNode.IsComplete)
            {
                throw new LaxerParseException();
            }

            List<Node> unComplateNodes = this.rootNode.GetUnCompletedNodes();

            bool isSeted = false;
            for (int i = unComplateNodes.Count - 1; i >= 0; i--)
            {
                if (unComplateNodes[i] is BracketNode)
                {
                    ((BracketNode)unComplateNodes[i]).SetCompleted();
                    isSeted = true;
                    break;
                }
            }

            if (!isSeted)
            {
                throw new LaxerParseException();
            }
        }

        private void SetParamEnd()
        {
            if (this.rootNode == null || this.rootNode.IsComplete)
            {
                throw new LaxerParseException();
            }

            List<Node> unComplateNodes = this.rootNode.GetUnCompletedNodes();

            bool isSeted = false;
            for (int i = unComplateNodes.Count - 1; i >= 0; i--)
            {
                if (unComplateNodes[i] is FunctionNode)
                {
                    ((FunctionNode)unComplateNodes[i]).SetParamCompleted();
                    isSeted = true;
                    break;
                }
            }

            if (!isSeted)
            {
                throw new LaxerParseException();
            }
        }
        /// <summary>
        /// 读取下一个字符，并且推进一列
        /// </summary>
        /// <returns></returns>
        private int ReaderRead()
        {
            ++col;
            return reader.Read();
        }
        /// <summary>
        /// 读取下一个字符，但指针并不推进
        /// </summary>
        /// <returns></returns>
        private int ReaderPeek()
        {
            return reader.Peek();
        }
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <returns></returns>
        String ReadString()
        {
            originalValue.Length = 0;
            int nextChar;
            while ((nextChar = ReaderRead()) != -1)
            {
                char ch = (char)nextChar;

                if (ch == '\'')
                {
                    int nnChar = reader.Peek();
                    if (nnChar == -1)
                    {
                        break;
                    }
                    char nnch = (char)nnChar;
                    if (nnch == '\'')
                    {
                        ReaderRead();
                        originalValue.Append('\'');
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    originalValue.Append(ch);
                }
            }

            return originalValue.ToString();
        }

        /// <summary>
        /// 读取数字
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private string ReadDigit(char ch)
        {
            originalValue.Length = 0;
            originalValue.Append(ch);

            bool isdouble = false;
            char peek = (char)ReaderPeek();

            while (Char.IsDigit(peek) || '.' == peek)
            {
                if ('.' == peek)
                {
                    if (isdouble)
                    {
                        break;
                    }
                    else
                    {
                        originalValue.Append((char)ReaderRead());
                        isdouble = true;
                    }
                }
                else
                {
                    originalValue.Append((char)ReaderRead());
                }
                peek = (char)ReaderPeek();
            }

            return originalValue.ToString();
        }

        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <returns></returns>
        String ReadStartEnd(char beforeChar)
        {
            Dictionary<char, char> startEnd = new Dictionary<char, char>() { 
                { '{', '}'},
                { '[', ']'},
                { '#', '#'}
            };

            char endChar = startEnd[beforeChar];

            originalValue.Length = 0;
            int nextChar;
            while ((nextChar = ReaderRead()) != -1)
            {
                char ch = (char)nextChar;
                if (ch == endChar)
                {
                    break;
                }
                else
                {
                    originalValue.Append(ch);
                }
            }

            return originalValue.ToString();
        }

        /// <summary>
        /// 读取标识符
        /// </summary>
        /// <param name="ch">传进来的字符</param>
        /// <returns></returns>
        string ReadIdent(char ch)
        {
            originalValue.Length = 0;
            originalValue.Append(ch);

            char peek = (char)ReaderPeek();

            while (Char.IsLetter(peek) || '_' == peek)
            {
                originalValue.Append((char)ReaderRead());
                peek = (char)ReaderPeek();
            }
            return originalValue.ToString();
        }
    }


}
