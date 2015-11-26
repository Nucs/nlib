namespace nucs.Network.RPC {
    public static class PreparedCodes {
        public static string Beep => @"SystemSounds.Play.Beep();";
        public static string ConsoleWrite => @"Console.WriteLine(""Im executed from a far far galaxy!"");";
        public static string WriteFile => @"
                using File.IO;
                public class HeadlessClass : IExecute {
                    public void Execute() {
                        File.WriteAllText(""C:\some.txt"", ""I text YOU"");
                    }
                }
";
    }
}