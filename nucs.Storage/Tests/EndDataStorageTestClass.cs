using System;
using System.Collections.Generic;

namespace TFer.EndData.Tests {

    [Serializable]
    internal class EndDataStorageTestClass {

        public List<string> Wow = new List<string>();
        public string Var;
        public int Num;
        public static EndDataStorageTestClass Fetch() {
            var a = new EndDataStorageTestClass();
            a.Wow.Add("amazing");
            a.Var = "You are the man!";
            a.Num = 15;
            return a;
        }

        public static bool Verify(EndDataStorageTestClass s) {
            try {
                if (s.Wow[0] != "amazing" && s.Var != "You are the man!" && s.Num != 15)
                    return false;
            } catch {
                return false;
            }
            return true;
        }
         
    }
}