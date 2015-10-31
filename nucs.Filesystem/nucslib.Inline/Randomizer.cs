using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nucs.SystemCore {
    internal static class Randomizer {

        public static string RandomString(uint length, CaseType _case = CaseType.Any) {
            string opts;
            switch (_case) {
                case CaseType.BigLetters:
                    opts = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    break;
                case CaseType.SmallLetters:
                    opts = "abcdefghijklmnopqrstuvwxyz";
                    break;
                case CaseType.Any:
                    opts = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_case));
            }

            return RandomString(opts, length);
        }

        public static T Chance<T>(this Random rand, double @truechance, Func<T> @ontrue, Func<T> @onfalse) {
            if (@truechance > 100 || @truechance <= 0) throw new ArgumentOutOfRangeException(nameof(@truechance));

            return @truechance/100 >= rand.NextDouble() ? @ontrue() : @onfalse();
        }

        public static T Chance<T>(this Random rand, double @truechance, T @ontrue, T @onfalse) {
            if (@truechance > 100 || @truechance <= 0) throw new ArgumentOutOfRangeException(nameof(@truechance));

            return @truechance/100 >= rand.NextDouble() ? @ontrue : @onfalse;
        }        public static string Chance(this Random rand, double @truechance, object @ontrue, object @onfalse) {
            if (@truechance > 100 || @truechance <= 0) throw new ArgumentOutOfRangeException(nameof(@truechance));

            return @truechance/100 >= rand.NextDouble() ? @ontrue.ToString() : @onfalse.ToString();
        }        public static string Chance(this Random rand, double @truechance, object @ontrue) {
            if (@truechance > 100 || @truechance <= 0) throw new ArgumentOutOfRangeException(nameof(@truechance));

            return @truechance/100 >= rand.NextDouble() ? @ontrue.ToString() : null;
        }

        public static bool Chance(this Random rand, double @truechance) {
            if (@truechance > 100 || @truechance <= 0) throw new ArgumentOutOfRangeException(nameof(@truechance));

            return @truechance/100 >= rand.NextDouble();
        }

        /// <summary>
        ///     A Random extension method that flip a coin toss.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true 50% of time, otherwise false.</returns>
        /// <example>
        ///     <code>
        ///           using System;
        ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
        ///           using Z.ExtensionMethods;
        ///           
        ///           namespace ExtensionMethods.Examples
        ///           {
        ///               [TestClass]
        ///               public class System_Random_CoinToss
        ///               {
        ///                   [TestMethod]
        ///                   public void CoinToss()
        ///                   {
        ///                       // Type
        ///                       var @this = new Random();
        ///           
        ///                       // Examples
        ///                       bool value = @this.CoinToss(); // return true or false at random.
        ///                   }
        ///               }
        ///           }
        ///     </code>
        /// </example>
        public static bool CoinToss(this Random @this) {
            return @this.Next(2) == 0;
        }

        public static T CoinToss<T>(this Random @this, Func<T> @true, Func<T> @false) {
            return @this.Next(2) == 0 ? @true() : @false();
        }

        public static T CoinToss<T>(this Random @this, Func<T> @true) {
            return @this.Next(2) == 0 ? @true() : default(T);
        }

        public static string CoinToss(this Random @this, object @true, object @false) {
            return @this.Next(2) == 0 ? @true.ToString() : @false.ToString();
        }

        public static string CoinToss(this Random @this, object @true) {
            return @this.Next(2) == 0 ? @true.ToString() : "";
        }

        public static string RandomString(string chars, uint length) {
            var arr = chars.ToCharArray();
            var r = new Random();
            string res = "";
            for (int i = 0; i < length; i++)
                res += arr[r.Next(0, arr.Length - 1)];
            return res;
        }

        public enum CaseType {
            BigLetters,
            SmallLetters,
            Any
        }
    }
}
