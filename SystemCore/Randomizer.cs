using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nucs.SystemCore {
    public static class Randomizer {

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
                    throw new ArgumentOutOfRangeException("_case");
            }

            return RandomString(opts, length);
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
