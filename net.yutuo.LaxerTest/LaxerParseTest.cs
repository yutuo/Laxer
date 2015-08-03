using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using net.yutuo.Laxer;
using net.yutuo.Laxer.Entities.Nodes;

namespace net.yutuo.LaxerTest
{
    [TestClass]
    public class LaxerParseTest
    {
        [TestMethod]
        public void TestLaxerParse()
        {
            LaxerParse parse = new LaxerParse();
            Node node;

            Dictionary<String, Object> staticValues = new Dictionary<string, object>() {
                { "TEST", "staticTestValue"},
            };
            Dictionary<String, Object> rowValus = new Dictionary<string, object>() {
                { "TEST", "rowTestValue"},
            };

            node = parse.Parse("'asdfasdf'");
            Assert.AreEqual("\"asdfasdf\"", node.ToString());
            Assert.AreEqual("\"asdfasdf\"", node.getValue(staticValues, rowValus).ToString());

            node = parse.Parse("1");
            Assert.AreEqual("1", node.ToString());
            Assert.AreEqual("1", node.getValue(staticValues, rowValus).ToString());

            node = parse.Parse("1.2");
            Assert.AreEqual("1.2", node.ToString());
            Assert.AreEqual("1.2", node.getValue(staticValues, rowValus).ToString());

            node = parse.Parse("#2015-7-8#");
            Assert.AreEqual("#2015/07/08 0:00:00#", node.ToString());
            Assert.AreEqual("2015/07/08 0:00:00", node.getValue(staticValues, rowValus).ToString());

            node = parse.Parse("1 - 2");
            Assert.AreEqual("- 1 2", node.ToString());
            Assert.AreEqual("-1", node.getValue(staticValues, rowValus).ToString());

            node = parse.Parse("1 - 2 + 12");
            Assert.AreEqual("+ - 1 2 12", node.ToString());
            Assert.AreEqual("11", node.getValue(staticValues, rowValus).ToString());

            node = parse.Parse("1 - 2 * 12");
            Assert.AreEqual("- 1 * 2 12", node.ToString());
            Assert.AreEqual("-23", node.getValue(staticValues, rowValus).ToString());

            node = parse.Parse("3 < 1 - 2 + 12");
            Assert.AreEqual("< 3 + - 1 2 12", node.ToString());
            Assert.AreEqual("True", node.getValue(staticValues, rowValus).ToString());

            node = parse.Parse("11 = 1 - 2 + 12");
            Assert.AreEqual("= 11 + - 1 2 12", node.ToString());
            Assert.AreEqual("True", node.getValue(staticValues, rowValus).ToString());

            node = parse.Parse("'12' + '12''12'");
            Assert.AreEqual("+ \"12\" \"12'12\"", node.ToString());
            Assert.AreEqual("\"1212'12\"", node.getValue(staticValues, rowValus).ToString());

            node = parse.Parse("1 - (2 + 12)");
            Assert.AreEqual("- 1 (+ 2 12)", node.ToString());
            Assert.AreEqual("-13", node.getValue(staticValues, rowValus).ToString());

            node = parse.Parse("(SubString '1212', 2) + 12");
            Assert.AreEqual("+ SubString(\"1212\", 2) 12", node.ToString());
            Assert.AreEqual("\"1212\"", node.getValue(staticValues, rowValus).ToString());

            node = parse.Parse("(SubString (SubString '1212', 2), 3) + 12");
            Assert.AreEqual("+ SubString(SubString(\"1212\", 2), 3) 12", node.ToString());

            node = parse.Parse("(11 + 12) * 13");
            Assert.AreEqual("* (+ 11 12) 13", node.ToString());

            node = parse.Parse("(11 + 12) * {TEST}");
            Assert.AreEqual("* (+ 11 12) {TEST}", node.ToString());

            node = parse.Parse("(11 + 12) * [TEST]");
            Assert.AreEqual("* (+ 11 12) [TEST]", node.ToString());

            node = parse.Parse("(11 + 12) * #2014/2/2#");
            Assert.AreEqual("* (+ 11 12) #2014/02/02 0:00:00#", node.ToString());
        }
    }
}
