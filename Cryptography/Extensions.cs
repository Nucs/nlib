using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace nucs.Cryptography {
    public static class Extensions {
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

        /// <param name="raw">raw as true will add '0x' at the beggining for e.g. 0x1ffa0</param>
        public static string ToSHA1asHex(this string toEncrypt, bool raw = false) {
            return ToHex(ToSHA1(toEncrypt), raw);
        }

/*        /// <summary>
        /// CME stands for Complete Mess Encryption, it can be easly encrypted and decrypted if willed.
        /// CME uses UTF8 Encoding to do the encryption, you also can use the overload to change that. do it with caution.
        /// </summary>
        public static string ToCME(this string toEncrypt) {
            toEncrypt = new string(toEncrypt.Reverse().ToArray());
            var s = Encoding.UTF8.GetBytes(toEncrypt);
            int leftover = s.Length % 3;
            //s.ToList().ForEach(c=>Console.Write(c+" "));




            return toEncrypt;
        }*/


    }
}
