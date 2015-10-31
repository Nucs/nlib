using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;

namespace nucs.SystemCore.Settings {
    public abstract class AppSettings<T> : ISaveable where T : new() {
        public const string DEFAULT_FILENAME = "settings.jsn";

        // ReSharper disable once StaticFieldInGenericType
        private static readonly JavaScriptSerializer serializer;
        static AppSettings() {
            serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(GetAllAttributeHolders(typeof(SettingsConverterAttribute)).Select(CreateInstance<JavaScriptConverter>));
        }

        /// <summary>
        /// The filename that was originally loaded from. saving to other file does not change this field!
        /// </summary>
        public virtual void Save(string filename = DEFAULT_FILENAME) {
            File.WriteAllText(filename, serializer.Serialize(this));
        }

        /// <summary>
        /// The filename that was originally loaded from. saving to other file does not change this field!
        /// </summary>
        public virtual void Save() {
            File.WriteAllText(DEFAULT_FILENAME, serializer.Serialize(this));
        }

        public static void Save(T pSettings, string filename = DEFAULT_FILENAME) {
            File.WriteAllText(filename, serializer.Serialize(pSettings));
        }

        /// <summary>
        /// Loads or creates a settings file.
        /// </summary>
        /// <param name="fileName">File name, for example "settings.jsn". no path required, just a file name.</param>
        /// <returns>The loaded or freshly new saved object</returns>
        public static T Load(string fileName = DEFAULT_FILENAME) {
            if (File.Exists(fileName))
                try {
                    var fc = File.ReadAllText(fileName);
                    if (string.IsNullOrEmpty((fc ?? "").Trim()))
                        goto _save;
                    return serializer.Deserialize<T>(fc);
                } catch (InvalidOperationException e) {
                    if (e.Message.Contains("Cannot convert"))
                        throw new Exception("Unable to deserialize settings file, value<->type mismatch. see inner exception", e);
                    throw e;
                } catch (System.ArgumentException e) {
                    if (e.Message.StartsWith("Invalid"))
                        throw new Exception("Settings file is corrupt.");
                    throw e;
                }
            _save:
            var t = new T();
            Save(t, fileName);
            return t;
        }

        /// <summary>
        /// Gives you all the types that has <param name="attribute"></param> attached to it in the entire <see cref="AppDomain"/>.
        /// </summary>
        /// <param name="attribute">the type of the attribute.</param>
        /// <returns></returns>
        private static IEnumerable<Type> GetAllAttributeHolders(Type attribute)
        {
            return from assmb in AppDomain.CurrentDomain.GetAssemblies() from type in gettypes(assmb) where type.GetCustomAttributes(attribute, true).Length > 0 select type;
        }

        private static Type[] gettypes(Assembly assmb)
        {
            return !File.Exists(AssemblyDirectory(assmb)) ? new Type[0] : assmb.GetTypes();
        }

        private static string AssemblyDirectory(Assembly asm)
        {
            string codeBase = asm.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
        private static T CreateInstance<T>(Type @this)
        {
            return (T)Activator.CreateInstance(@this);
        }
    }
    
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class SettingsConverterAttribute : Attribute {
        public SettingsConverterAttribute() { }
    }

}