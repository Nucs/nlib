using System.Text;

namespace nucs.Cryptography {
    public abstract class BaseEncodingCrypter : ICrypter {
        public Encoding Encoding { get; } = Encoding.UTF8;

        public abstract string Encrypt(string data, string key);
        public abstract string Decrypt(string data, string key);


        public string Encrypt(byte[] data, string key) {
            return Encrypt(Encoding.UTF8.GetString(data), key);
        }

        public string Decrypt(byte[] data, string key) {
            return Decrypt(Encoding.UTF8.GetString(data), key);
        }

        public byte[] EncryptToBytes(string data, string key) {
            return Encoding.UTF8.GetBytes(Encrypt(data, key));
        }

        public byte[] DecryptToBytes(string data, string key) {
            return Encoding.UTF8.GetBytes(Decrypt(data, key));
        }

        public byte[] EncryptToBytes(byte[] data, string key) {
            return Encoding.UTF8.GetBytes(Encrypt(data, key));
        }

        public byte[] DecryptToBytes(byte[] data, string key) {
            return Encoding.UTF8.GetBytes(Decrypt(data, key));
        }
    }
}