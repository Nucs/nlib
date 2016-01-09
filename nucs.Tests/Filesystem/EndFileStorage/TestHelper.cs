using System.IO;

namespace nucs.EndData.Tests {
    public static class TestHelper {
        public static FileInfo GetTemp() {
            return new FileInfo(Path.GetTempFileName());
        }
    }
}