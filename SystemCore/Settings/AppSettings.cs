using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using nucs.Collections;

namespace nucs.SystemCore.Settings {
    public abstract class AppSettings<T> where T : new() {
        public const string DEFAULT_FILENAME = "settings.jsn";

        // ReSharper disable once StaticFieldInGenericType
        private static readonly JavaScriptSerializer serializer;
        static AppSettings() {
            serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(typeof(SettingsConverterAttribute).GetAllAttributeHolders().Select(t=>t.CreateInstance<JavaScriptConverter>()));
        }

        /// <summary>
        /// The filename that was originally loaded from. saving to other file does not change this field!
        /// </summary>
        public virtual void Save(string filename = DEFAULT_FILENAME) {
            serializer.Serialize(this).SaveAs(filename); 
        }

        public static void Save(T pSettings, string fileName = DEFAULT_FILENAME) {
            serializer.Serialize(pSettings).SaveAs(fileName);
        }

        /// <summary>
        /// Loads or creates a settings file.
        /// </summary>
        /// <param name="fileName">File name, for example "settings.jsn". no path required, just a file name.</param>
        /// <returns>The loaded or freshly new saved object</returns>
        public static T Load(string fileName = DEFAULT_FILENAME) {
            if (File.Exists(fileName)) 
                return serializer.Deserialize<T>((File.ReadAllText(fileName)));
            var t = new T();
            Save(t, fileName);
            return t;
        }
    }
    
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class SettingsConverterAttribute : Attribute {
        public SettingsConverterAttribute() { }
    }

}