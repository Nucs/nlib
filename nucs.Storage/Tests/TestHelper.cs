using System.IO;

namespace TFer.EndData.Tests {
    public static class TestHelper {
        public static FileInfo GetTemp() {
            return new FileInfo(Path.GetTempFileName());
        }
    }
}