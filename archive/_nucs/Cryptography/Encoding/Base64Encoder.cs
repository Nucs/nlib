using System;

namespace nucs.Cryptography {
    public class Base64Encoder : IEncoder {
        public string Encode(string data) {
            return Encode(data, System.Text.Encoding.UTF8);
        }

        public string Decode(string data) {
            return Decode(data, System.Text.Encoding.UTF8);
        }

        public string Encode(string data, System.Text.Encoding encoding = null) {
            encoding = encoding ?? System.Text.Encoding.UTF8;
            return Convert.ToBase64String(encoding.GetBytes(data));
        }

        public string Decode(string data, System.Text.Encoding encoding = null) {
            encoding = encoding ?? System.Text.Encoding.UTF8;
            return Convert.ToBase64String(encoding.GetBytes(data));
        }
    }
}