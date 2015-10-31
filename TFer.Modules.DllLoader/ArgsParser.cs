using System;
using System.Collections.Generic;
using System.Linq;

namespace TFer.Modules.DllLoader {
    public static class ArgsParser {
        public const string DELIMITER = ";;";

        public static Dictionary<string, string> Parse(string _arg) {
            if (_arg.StartsWith("\"") && _arg.EndsWith("\""))
                _arg = _arg.Trim('"');
            var args = new Dictionary<string, string>();
            foreach (var arg in _arg.Split(new[] { ";;" }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] pieces = arg.Split('=');
                args[pieces[0]] = pieces.Length > 1 ? pieces[1] : "";
            }
            return args;
        }

        public static string Prepare(Dictionary<string, string> dic) {
            return string.Join("", dic.Select(kv => $"{kv.Key}={kv.Value}{DELIMITER}").ToArray());
        }

    }
}