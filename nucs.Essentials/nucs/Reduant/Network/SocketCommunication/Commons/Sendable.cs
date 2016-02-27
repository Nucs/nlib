/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace nucs.SocketCommunication.Commons {
    internal abstract class Sendable<T> {
        public static readonly string Seperator = "╓";
        public virtual string PrepareForSend(T instance) {
            var b = new StringBuilder();
            foreach (var p in GetPublicProperties()) {
                b.Append(p.GetValue(instance) + Seperator);
            }
            b.Remove(b.Length - 1, 1);

            return b.ToString();
        }

        public virtual void CompileObject(ref T obj, string receivedMessage) {
            
            var s = receivedMessage.Split(Seperator[0]);
            var props = GetPublicProperties();
            for (int i = 0; i < s.Length; i++) {
                props[i].SetValue(obj, s[i]);
            }
        }

        public static List<PropertyInfo> GetPublicProperties() {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite).OrderBy(p => p.Name).ToList();
        }
    }
}
*/
