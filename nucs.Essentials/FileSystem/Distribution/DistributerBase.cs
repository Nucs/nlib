﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using nucs.SystemCore.Boolean;

namespace nucs.Filesystem.Distribution {
    public abstract class DistributerBase {
        public const int RandomNameConstantLength = 10;
        /// <summary>
        ///     Is this distributer provide directories that are static through every computer.
        /// </summary>
        public abstract bool IsStatic { get; }
        /// <summary>
        ///     Do not forget to dispose!
        /// </summary>
        /// <returns></returns>
        public static MemoryStream LoadSelfExeToMemory() {
            var fs = new FileStream(Paths.ExecutingExe.ToString(), FileMode.Open, FileAccess.Read);
            var bf = new byte[fs.Length];
            var l = fs.Read(bf, 0, bf.Length);
            fs.Close();
            return new MemoryStream(l);
        }

        private static readonly Random rnd = new Random();
        /// <summary>
        ///     Generates a random with constant signature, for eg, _CsC rand 7 letters -> _AcGrgxnjee
        /// </summary>
        public static string GenerateRandomName {
            get {
                var r = StringGenerator.Generate(rnd, RandomNameConstantLength - 1);
                return "_" + char.ToUpper(r[0]) + char.ToLower(r[1]) + char.ToUpper(r[2]) + r.Substring(3, RandomNameConstantLength - 4);
            }
        }

        /// <summary>
        ///     Validates all the predefined name config of the exe.
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        public static bool IsRandomallyGeneratedName(FileInfo fi) {
            var a = fi.Name; //short reference to a.
            return a.Length == RandomNameConstantLength && a[0] == '_' && a.Skip(1).Take(3).All(c=>char.IsLetter(c) && char.IsUpper(a[1]) && char.IsLower(a[2]) &&
                   char.IsUpper(a[3]));
        }


        protected List<FileInfo> _distributedCache = null;

        /// <summary>
        ///     Returns the distributed locations, if none or null, attempts to find them. but wont perform Distribute().
        /// </summary>
        public List<FileInfo> Distributed {
            get {
                if (_distributedCache == null || _distributedCache.Count == 0) {
                    return _distributedCache = FindDistributed();
                }
                return _distributedCache;
            }
        }

        /// <summary>
        ///     Gets all the directories inwhich distribution can happen.
        /// </summary>
        public abstract IEnumerable<DirectoryInfo> Distributables();
        /// <summary>
        ///     Accessiable 
        /// </summary>
        /// <returns></returns>
        protected abstract List<FileInfo> FindDistributed();

    }

    internal static class StringGenerator {
        private static Random rand = null;

        public static string Generate(int len = 10) {
            return Generate(rand ?? (rand = new Random()), len);
        }


        public static string Generate(Random rand, int len = 10) {
            if (len <= 0) return "";
            char ch;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < len; i++) {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26*rand.NextDouble() + 65)));
                builder.Append(ch);
            }
            for (int i = 0; i < len; i++) {
                if (rand.Next(1, 3) == 1)
                    builder[i] = char.ToLowerInvariant(builder[i]);
            }
            return builder.ToString();
        }
    }
}