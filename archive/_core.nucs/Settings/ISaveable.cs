namespace nucs.Settings {
    public interface ISaveable {
        void Save(string filename);
        void Save();
    }
}
