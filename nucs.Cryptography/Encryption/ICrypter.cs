namespace nucs.Cryptography {
    public interface ICrypter {
        string Encrypt(string data, string key);
        string Decrypt(string data, string key);
        string Encrypt(byte[] data, string key);
        string Decrypt(byte[] data, string key);
        byte[] EncryptToBytes(string data, string key);
        byte[] DecryptToBytes(string data, string key);
        byte[] EncryptToBytes(byte[] data, string key);
        byte[] DecryptToBytes(byte[] data, string key);
    }
}