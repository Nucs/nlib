using System;
using System.Diagnostics;
using System.Media;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nucs.Network.RPC;

namespace nucs.Network {
    [TestClass]
    public class ExecutionTests {
        public static string iexeret = @"
public class xx : IExecute<string> {
    public string Execute() {
        return ""potato"";
    }
}
";
        public static string iexe = @"
public class xx : IExecute {
    public void Execute() {
        SystemSounds.Beep.Play();
    }
}

";
        [TestMethod]
        public void SimpleCompileRunWithNamespace() {
            RemoteExecuter.Execute(iexe, new [] { "System.Media" });
        }

        [TestMethod]
        [ExpectedException(typeof(MissingNamespaceException))]
        public void SimpleCompileRunWitouthNamespaceExpectedMissingNamespaceException() {
            RemoteExecuter.Execute(iexe);
        }

        [TestMethod]
        public void SimpleCompileRunOfRetFunc() {
            Assert.IsTrue(RemoteExecuter.ExecuteReturn(iexeret)     ==      "potato");
        }

        [TestMethod]
        public void PrepareAddBodyWithReturn() {
            Assert.IsTrue((int)RemoteExecuter.ExecuteReturn("return 5;")     ==      5);
        }

        [TestMethod]
        public void PrepareAddBodyWithoutReturn() {
            RemoteExecuter.Execute("new Action(delegate() { }).Invoke();");
            RemoteExecuter.Execute("if (1==1);");
        }


        public class xx : IExecute<string> {
            public string Execute() {
                return "potato";
            }
        }
    }
}