using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nucs.Filesystem;
using nucs.SystemCore;

namespace nucs.Network {
    [TestClass]
    public class FileInfoExtensions {

        static FileInfoExtensions() {}

        [TestMethod]
        public void HandleDuplicateName() {
            var @base = new DirectoryInfo(Path.Combine(Path.GetTempPath(), "temp" + Randomizer.RandomString(10) + "/"));
            if (@base.Exists==false)
                @base.Create();
            try {
                var org_file = @base.PathCombineFile(Randomizer.RandomString(10) + ".potato");
                var file = org_file.HandlePossibleDuplicate();
                Assert.AreEqual(org_file.FullName, file.FullName);
                File.WriteAllText(file.FullName, "");
                file = org_file.HandlePossibleDuplicate();
                Assert.IsTrue(file.FullName.EndsWith($"{org_file.GetFileNameWithoutExtension()} (1){org_file.Extension}"));
            } finally {
                @base.Delete(true);
            }
        }
    }
}
