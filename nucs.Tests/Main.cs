using System;
using System.Text;
using nucs.SystemCore;

namespace nucs.Network {
    public class Program {
        public static void Main() {
            var t = new Cache<string>("what", () => StringGenerator.Generate(), 5000, false);

            string o = t;

            Console.WriteLine(o);
            Console.ReadLine();
        }

        internal static class StringGenerator
        {
            private static Random rand = null;

            public static string Generate(int len = 10)
            {
                return Generate(rand ?? (rand = new Random()), len);
            }


            public static string Generate(Random rand, int len = 10)
            {
                if (len <= 0) return "";
                char ch;
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < len; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 65)));
                    builder.Append(ch);
                }
                for (int i = 0; i < len; i++)
                {
                    if (rand.Next(1, 3) == 1)
                        builder[i] = char.ToLowerInvariant(builder[i]);
                }
                return builder.ToString();
            }
        }
    }
}