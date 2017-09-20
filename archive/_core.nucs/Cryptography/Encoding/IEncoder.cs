namespace nucs.Cryptography {
    public interface IEncoder {
        string Encode(string data);
        string Decode(string data);
    }
}