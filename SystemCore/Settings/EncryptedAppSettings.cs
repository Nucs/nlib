using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using nucs.Collections;
using nucs.Cryptography;

namespace nucs.SystemCore.Settings {

    [Serializable]
    public abstract class EncryptedAppSettings<T> where T : new() {
        [NonSerialized]
        public static string DEFAULT_FILENAME = "settings.jsn";
        public static string DEFAULT_PATH = "#BASE";

        public static bool IsSerializedFile(FileInfo file) {
            if (File.Exists(file.FullName) == false) return false;
            try {
                string data;
                using (var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read)) {
                    if (fs.Length > 10e6) {
                        using (var sr = new StreamReader(fs, Encoding.UTF8)) {
                            data = sr.ReadLine().Trim();
                            if ((Regex.IsMatch(data, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None)) == false) {
                                return false;
                            } else {
                                fs.Position = 0;
                                data = sr.ReadToEnd();
                            }
                        }
                    } else {
                        return false;
                    }
                }
                var s = data.Trim();
                if (((s.Length%4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None)) == false)
                    return false;
                var t = default(T);
                serializer.Deserialize<T>(
                    ((EncryptedAppSettings<T>) (object) t).Encryptor.Decrypt(File.ReadAllText(file.FullName)));
                return true;
            }
            catch (IOException) {
                return false;
            }
            catch (Exception) {
                return false;
            }
            catch {
                return false;
            }
        }


        [NonSerialized]
        private static readonly JavaScriptSerializer serializer;
        static EncryptedAppSettings() {
            serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(typeof(SettingsConverterAttribute).GetAllAttributeHolders().Select(t=>t.CreateInstance<JavaScriptConverter>()));
        }

        protected EncryptedAppSettings() {
            Encryptor = new RijndaelEnhanced(GenerateSeed());
        }

        [NonSerialized]
        [ScriptIgnore]
        private readonly RijndaelEnhanced Encryptor;
	
		/// <summary>
        /// 	Generate a constant seed that will be used to encrypt the text.
        /// </summary>
        protected abstract string GenerateSeed();

        /// <summary>
        ///     The filename that was originally loaded from. saving to other file does not change this field!
        /// </summary>
        public virtual void Save(string filename = "#DEFAULT") {
            if (filename.IsNullOrEmpty() || filename== "#DEFAULT") filename = DEFAULT_FILENAME;
            var serialized = serializer.Serialize(this);
            Encryptor.Encrypt(serialized).SaveAs((DEFAULT_PATH == "#BASE" ? "" : DEFAULT_PATH) + filename);
        }

        public static void Save(T pSettings, string filename = "#DEFAULT") {
            if (filename.IsNullOrEmpty() || filename == "#DEFAULT") filename = DEFAULT_FILENAME;
            var serialized = serializer.Serialize(pSettings);
            ((EncryptedAppSettings<T>)(object)pSettings).Encryptor.Encrypt(serialized).SaveAs(filename);
        }

        /// <summary>
        ///     Loads or creates a settings file.
        /// </summary>
        /// <param name="filename">File name, for example "settings.jsn". no path required, just a file name.</param>
        /// <returns>The loaded or freshly new saved object</returns>
        public static T Load(string filename = "#DEFAULT", bool encrypted = true) {
            if (filename.IsNullOrEmpty() || filename == "#DEFAULT") filename = DEFAULT_FILENAME;

            if (File.Exists(filename)) {
                var t = new T();
                return serializer.Deserialize<T>(encrypted ? ((EncryptedAppSettings<T>)(object)t).Encryptor.Decrypt(File.ReadAllText(filename)) : File.ReadAllText(filename));
            }
            var n = new T();
            Save(n, filename);
            return n;
        }

        
    }


    public static class EncryptedAppSettings {
        
        /// <summary>
        ///     Validates that the given file is indeed encrypted and saved in base64
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsEncryptedFile(FileInfo file) {
            if (File.Exists(file.FullName) == false) return false;

            var s = file.ReadAllText().Trim();
            return (s.Length%4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
    }

}