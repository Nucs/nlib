namespace nucs.Cryptography {
    public class RijndaelCrypter : ICrypter {
        public string Encrypt(string data, string key) {
            var rij = new RijndaelEnhanced(key);
            return rij.Encrypt(data);
        }

        public string Decrypt(string data, string key) {
            var rij = new RijndaelEnhanced(key);
            return rij.Decrypt(data);

        }

        public string Encrypt(byte[] data, string key) {
            var rij = new RijndaelEnhanced(key);
            return rij.Encrypt(data);
        }

        public string Decrypt(byte[] data, string key) {
            var rij = new RijndaelEnhanced(key);
            return rij.Decrypt(data);
        }

        public byte[] EncryptToBytes(string data, string key) {
            var rij = new RijndaelEnhanced(key);
            return rij.EncryptToBytes(data);
        }

        public byte[] DecryptToBytes(string data, string key) {
            var rij = new RijndaelEnhanced(key);
            return rij.DecryptToBytes(data);
        }

        public byte[] EncryptToBytes(byte[] data, string key) {
            var rij = new RijndaelEnhanced(key);
            return rij.EncryptToBytes(data);
        }

        public byte[] DecryptToBytes(byte[] data, string key) {
            var rij = new RijndaelEnhanced(key);
            return rij.DecryptToBytes(data);
        }
    }
}