using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nucs.Network.Discovery;

namespace nucs.Network {
    [TestClass]
    public class NodesListTest {

        static NodesListTest() {}

        [TestMethod]
        public void TestCreation() {
            var p = new NodesList<Node>();
        }
        [TestMethod]
        public void TestAddition() {
            var p = new NodesList<Node>();
            p.Add(new Node("1.1.1.1"));
            p.Add(new Node("1.1.1.1"));
            p.Add(new Node("1.1.1.2"));
            Assert.IsTrue(p.Count==2);
        }

        [TestMethod]
        public void TestMergeInto() {
            var p = new NodesList<Node>();
            p.Add(new Node("1.1.1.1"));
            p.Add(new Node("1.1.1.1"));
            p.Add(new Node("1.1.1.2"));
            p.Add(new Node("1.1.1.4"));

            var p2 = new NodesList<Node>();
            p2.Add(new Node("1.1.1.1"));
            p2.Add(new Node("1.1.1.1"));
            p2.Add(new Node("1.1.1.2"));
            p2.Add(new Node("1.1.1.3"));

            p.MergeInto(p2);
            Assert.IsTrue(p.Count==4);
        }
    }
}
