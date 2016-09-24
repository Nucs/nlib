#if NET45|| NET40
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using nucs.SystemCore.Reflection;
using nucs.Collections.Extensions;
namespace nucs.SystemCore.Dynamic {
    public class AnonymousProperties : DynamicObject, IAggregatableObject<AnonymousProperties> {
        /// <summary>
        /// Represents 'null', which is also what the value in the AnonymousType needs to be inorder to ignore the value at subtraction
        /// </summary>
        public static readonly AcceptAnyValue AnyValue = new AcceptAnyValue();
        public object this[string key] {
            get { return AsDictionary.FirstOrDefault(kv=>kv.Key.Equals(key)).Value; }
            set {
                if (AsDictionary.ContainsKey(key))
                    AsDictionary[key] = value;
                else
                    AsDictionary.Add(key, value);
            }
        }
        protected dynamic _properties;

        protected virtual dynamic Properties {
            get { return _properties; }
            set { _properties = value; }
        }

        public AnonymousProperties(dynamic props) {
             if (props == null) goto _throw;
            if (Dynamic.IsObjectAnonymous(props)) {
                Properties = Dynamic.ToExpandoObject(Dynamic.ToDictionary(props));
                return;
            }
            if (((object) props).GetType().Name.Contains("ExpandoObject")) {
                Properties = (ExpandoObject)props;
                return;
            }
            _throw: throw new ArgumentException("Given 'dynamic props' is either null or not an Anonymous Type. " + props == null ? "props=null" : "props=" + ((object)(props)).GetType().Name, "props");
        }

        public static ExpandoObject AttemptDynamicToExpando(dynamic dyn) {
            return Dynamic.ToExpandoObject(Dynamic.ToDictionary(dyn));
            
        }

        public AnonymousProperties(ExpandoObject obj) {
            Properties = obj;
        }

        public AnonymousProperties() {
            _properties = new ExpandoObject();
        }

        public virtual ExpandoObject AsExpandoObject { get { return (ExpandoObject)Properties; } }

        public virtual dynamic AsDynamic { get { return Properties; } }

        public virtual IDictionary<string, object> AsDictionary { get { return (IDictionary<string, object>) Properties; } } 

        public virtual void Add(AnonymousProperties b) {
            var dic = AsDictionary;
            foreach (var kv in b.AsExpandoObject)
                if (dic.ContainsKey(kv.Key))
                    dic[kv.Key] = kv.Value;
                else
                    dic.Add(kv);
        }

        public virtual void Subtract(AnonymousProperties b) {
            var dic = AsDictionary;
            foreach (var kv in b.AsExpandoObject) {
                if (dic.ContainsKey(kv.Key))
                    if (kv.Value is AcceptAnyValue)
                        dic.Remove(kv.Key);
                    else if (dic[kv.Key] == kv.Value)
                        dic.Remove(kv.Key);
            }
        }

        public AnonymousProperties<T> ConvertToTyped<T>(bool acceptFields = false) {
            return new AnonymousProperties<T>(AsExpandoObject, acceptFields);
        }
        #region Math Operations
        public static AnonymousProperties operator +(AnonymousProperties a, AnonymousProperties b) {
            a.Add(b);
            return a;
        }

        public static AnonymousProperties operator -(AnonymousProperties a, AnonymousProperties b) {
            a.Subtract(b);
            return a;
        }

        public static AnonymousProperties operator +(AnonymousProperties a, AnonymousProperties<dynamic> b) {
            a.Add(b);
            return a;
        }

        public static AnonymousProperties operator -(AnonymousProperties a, AnonymousProperties<dynamic> b) {
            a.Subtract(b);
            return a;
        }

        public static AnonymousProperties operator +(AnonymousProperties a, dynamic b) {
            a.Add(Dynamic.ToExpandoObject(b));
            return a;
        }

        public static AnonymousProperties operator -(AnonymousProperties a, dynamic b) {
            a.Subtract(Dynamic.ToExpandoObject(b));
            return a;
        }
        #endregion
        #region DynamicObject Implementation
        public override IEnumerable<string> GetDynamicMemberNames() {
            return AsDictionary.Select(p => p.Key);
        }

        public override bool TryDeleteMember(DeleteMemberBinder binder) {
            return AsDictionary.Remove(binder.Name);
        }


        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            result = this[binder.Name];
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value) {
            this[binder.Name] = value;
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result) {
            result = this[indexes[0].ToString()];
            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value) {
            this[indexes[0].ToString()] = value;
            return true;
        }

        #endregion

        public void ApplyProperties(object obj) { //todo apply anonymously to any type
            var props = obj.GetType().GetProperties().Cast<MemberInfo>().ToList();
            props.AddRange(obj.GetType().GetFields());
            var _typeVariables = props.ToDictionary(t => t.Name, t=> t);
            foreach (var prop in AsExpandoObject.Where(prop => _typeVariables.ContainsKey(prop.Key))) {
                try {
                    var variable = _typeVariables[prop.Key];
                    if (variable.MemberType == MemberTypes.Property)
#if NET45 || NET451
                                        ((PropertyInfo)variable).SetValue(obj, prop.Value);
#else
                        ((PropertyInfo)variable).SetValue(obj, prop.Value,null);
#endif
                    else
                        ((FieldInfo)variable).SetValue(obj, prop.Value);
                }
                catch { }
            }
        }

        /// <summary>
        /// Figures out if <see cref="MemberInfo"/> is a FieldInfo, PropertyInfo or TypeInfo. if true, returns the type they represent, otherwise null.
        /// </summary>
        /// <returns>Type that the MemberInfo represents</returns>
        public static Type MemberInfoToType(MemberInfo info) {
            switch (info.MemberType) {
                case MemberTypes.Field:
                    return ((FieldInfo) info).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo) info).PropertyType;
                case MemberTypes.TypeInfo:
#if NET45
                    return ((TypeInfo) info).AsType();
#else
                    return info.ReflectedType;
#endif
            }
            return null;
        }


    }
    
    public sealed class AnonymousProperties<T> : AnonymousProperties, IAggregatableObject<AnonymousProperties<T>> {
        public static Dictionary<string, MemberInfo> _typeVariables;
        static AnonymousProperties() {
            _typeVariables = new Dictionary<string, MemberInfo>();
            foreach (var prop in typeof(T).GetProperties()) 
                _typeVariables.Add(prop.Name, prop);
            foreach (var field in typeof(T).GetFields()) 
                _typeVariables.Add(field.Name, field);
        }

        public Type Type { get { return typeof (T); } }

        public bool AcceptFields { get; set; }

        private new dynamic Properties { //todo notice the never used!
            get { return _properties; }
            set { _properties = Dynamic.ToExpandoObject(Dynamic.FilterByType<T>(value, AcceptFields));
            }
        }


        internal AnonymousProperties(ExpandoObject props, bool acceptFields = false) {
            AcceptFields = acceptFields;
            Properties = props;
        }

        public AnonymousProperties(dynamic props, bool acceptFields = false) { 
            AcceptFields = acceptFields;
            if (props == null) goto _throw;
            if (Dynamic.IsObjectAnonymous(props)) {
                Properties = Dynamic.ToExpandoObject(Dynamic.ToDictionary(props));
                return;
            }
            if (((object) props).GetType().Name.Contains("ExpandoObject")) {
                Properties = (ExpandoObject)props;
                return;
            }
            
            _throw: throw new ArgumentException("Given 'dynamic props' is either null or not an Anonymous Type. " + props == null ? "props=null" : "props=" + ((object)(props)).GetType().Name, "props");
        }


        public override bool TrySetMember(SetMemberBinder binder, object value) {
            if (AcceptFields) {
                if (_typeVariables.Any(n => n.Key.Equals(binder.Name) && MemberInfoToType(n.Value) == value.GetType()) == false)
                    return false;
            } else
                if (_typeVariables.Where(kv => kv.Value.MemberType == MemberTypes.Property).Any(n => n.Key.Equals(binder.Name) && MemberInfoToType(n.Value) == value.GetType()) == false)
                    return false;

            base.TrySetMember(binder, value);
            return true;

        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value) {
            if (AcceptFields) {
                if (_typeVariables.Where(kv => kv.Value.MemberType == MemberTypes.Property).Any(n => n.Key.Equals(indexes[0].ToString()) && MemberInfoToType(n.Value) == value.GetType()) == false)
                    return false;
            } else
                if (_typeVariables.Any(n => n.Key.Equals(indexes[0].ToString()) && MemberInfoToType(n.Value) == value.GetType()) == false)
                    return false;
            base.TrySetIndex(binder, indexes, value);
            return true;
            
        }

        public T ApplyProperties(T obj) {
            foreach (var prop in AsExpandoObject) {
                if (_typeVariables.ContainsKey(prop.Key) == false)
                    continue;
                try {
                    var variable = _typeVariables[prop.Key];
                    if (variable.MemberType == MemberTypes.Property)
#if NET45 || NET451
                        ((PropertyInfo)variable).SetValue(obj, prop.Value);
#else
                        ((PropertyInfo)variable).SetValue(obj, prop.Value,null);

#endif
                    else
                        ((FieldInfo)variable).SetValue(obj, prop.Value);
                } catch {}
            }
            return obj;
        }
        #region Math Operations 
        public static AnonymousProperties<T> operator +(AnonymousProperties<T> a, AnonymousProperties<T> b) {
            a.Add(b);
            return a;
        }

        public static AnonymousProperties<T> operator -(AnonymousProperties<T> a, AnonymousProperties<T> b) {
            a.Subtract(b);
            return a;
        }

        public static AnonymousProperties<T> operator +(AnonymousProperties<T> a, AnonymousProperties b) {
            var filtered = Dynamic.FilterByType<T>(b.AsDictionary, a.AcceptFields);
            var dic = a.AsDictionary;
            foreach (var kv in filtered)
                if (dic.ContainsKey(kv.Key))
                    dic[kv.Key] = kv.Value;
                else
                    dic.Add(kv);
            return a;
        }

        public static AnonymousProperties<T> operator -(AnonymousProperties<T> a, AnonymousProperties b) {
            var filtered = Dynamic.FilterByType<T>(b.AsDictionary, a.AcceptFields);
            var dic = a.AsDictionary;
            foreach (var kv in filtered) {
                if (dic.ContainsKey(kv.Key))
                    if (kv.Value is AcceptAnyValue)
                        dic.Remove(kv.Key);
                    else if (dic[kv.Key] == kv.Value)
                        dic.Remove(kv.Key);
            }
            return a;
        }


        public static AnonymousProperties<T> operator +(AnonymousProperties<T> a, dynamic b) {
            a.Add(Dynamic.ToExpandoObject(Dynamic.FilterByType<T>(b, a.AcceptFields)));
            return a;
        }

        public static AnonymousProperties<T> operator -(AnonymousProperties<T> a, dynamic b) {
            a.Subtract(Dynamic.ToExpandoObject(Dynamic.FilterByType<T>(b, a.AcceptFields)));
            return a;
        }

        public void Add(AnonymousProperties<T> b) {
            base.Add(b);
        }

        public void Subtract(AnonymousProperties<T> b) {
            base.Subtract(b);
        }
        #endregion
    }


    public struct AcceptAnyValue { }

}
#endif