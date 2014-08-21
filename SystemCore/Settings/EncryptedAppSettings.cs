using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using nucs.Collections;
using nucs.Cryptography;

namespace nucs.SystemCore.Settings {
    public abstract class EncryptedAppSettings<T> where T : new() {
        public const string DEFAULT_FILENAME = "settings.jsn";

// ReSharper disable once StaticFieldInGenericType
        private static readonly JavaScriptSerializer serializer;
        static EncryptedAppSettings() {
            serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(typeof(SettingsConverterAttribute).GetAllAttributeHolders().Select(t=>t.CreateInstance<JavaScriptConverter>()));
        }

        protected EncryptedAppSettings() {
            Encryptor = new RijndaelEnhanced(GenerateSeed());
        }

        [ScriptIgnore]
        private readonly RijndaelEnhanced Encryptor;
	
		/// <summary>
        /// 	Generate a constant seed that will be used to encrypt the text.
        /// </summary>
        public abstract string GenerateSeed();

        /// <summary>
        /// The filename that was originally loaded from. saving to other file does not change this field!
        /// </summary>
        public virtual void Save(string filename = DEFAULT_FILENAME) {
            var serialized = serializer.Serialize(this);
            Encryptor.Encrypt(serialized).SaveAs(filename);
        }

        public static void Save(T pSettings, string fileName = DEFAULT_FILENAME) {
            var serialized = serializer.Serialize(pSettings);
            ((EncryptedAppSettings<T>)(object)pSettings).Encryptor.Encrypt(serialized).SaveAs(fileName);
        }

        /// <summary>
        /// Loads or creates a settings file.
        /// </summary>
        /// <param name="fileName">File name, for example "settings.jsn". no path required, just a file name.</param>
        /// <returns>The loaded or freshly new saved object</returns>
        public static T Load(string fileName = DEFAULT_FILENAME) {
            if (File.Exists(fileName)) {
                var t = new T();
                return serializer.Deserialize<T>(((EncryptedAppSettings<T>)(object)t).Encryptor.Decrypt(File.ReadAllText(fileName)));
            }
            var n = new T();
            Save(n, fileName);
            return n;
        }
    }
}