using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace nucs.Cryptography {
    public static class Hash {
        private static SHA1 sha; //null by default
        
        public static byte[] ToSHA1(this string toEncrypt) {
            if (sha == null) sha = new SHA1CryptoServiceProvider(); 
            return sha.ComputeHash(Encoding.UTF8.GetBytes(toEncrypt));
        }

        /// <param name="raw">raw as true will add '0x' at the beggining for e.g. 0x1ffa0</param>
        public static string ToHex(this byte[] me, bool raw = false) {
            string result = raw ? "" : "0x";
            for (int i = 0; i < me.Length; i++) {
                result += me[i].ToString("X2");
            }
            return result;
        }

        /// <param name="raw">raw as false will add '0x' at the beggining for e.g. 0x1ffa0</param>
        public static string ToSHA1asHex(this string toEncrypt, bool raw = false) {
            return ToHex(ToSHA1(toEncrypt), raw);
        }
    }
}
