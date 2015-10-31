using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFer.Modules.TestModule {
    public static class ArgsParser {
        public const string DELIMITER = ";;";

        public static Dictionary<string, string> Parse(string _arg) {
            _arg = Encoding.UTF8.GetString(Convert.FromBase64String(_arg));
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
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join("", dic.Select(kv => $"{kv.Key}={kv.Value}{DELIMITER}").ToArray())));
        }

    }
}