#if NET_4_5|| NET_4_0

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using PetaPoco.Internal;
using nucs.ADO.NET.MySql;
using nucs.SystemCore.Dynamic;
using nucs.SystemCore.Reflection;
using nucs.SystemCore.String;

// ReSharper disable CheckNamespace
namespace PetaPoco {
    public partial class Database {
        public TRes Count<T, TRes>(string where = "") {
            if (where != "" && !where.ToLowerInvariant().Contains("where"))
                where = "WHERE " + where;
            var pd = PocoData.ForType(typeof(T));
            return ExecuteScalar<TRes>(string.Format("SELECT COUNT({0}) FROM {1}{2};", pd.TableInfo.PrimaryKey, _dbType.EscapeTableName(pd.TableInfo.TableName), string.IsNullOrEmpty(where) ? "" : " " + where));
        }
        public TRes Count<T, TRes>(dynamic properties) {
            return Count<T, TRes>(new AnonymousProperties<T>(properties));
        }
        public TRes Count<T, TRes>(AnonymousProperties<T> properties) {
            var dic = Dynamic.ToDictionary(properties);
            if (dic.Count == 0)
                return Count<T, TRes>();
            var query = string.Join(" AND ", dic.Select(kv => _dbType.EscapeSqlIdentifier(kv.Key) + "=" + ClauseIfString(kv.Value)));
            return Count<T, TRes>("WHERE "+query);
        }

        public T FirstOrDefault<T>(dynamic properties) {
            return FirstOrDefault<T>(new AnonymousProperties<T>(properties));
        }
        public T FirstOrDefault<T>(AnonymousProperties<T> properties) {
            var dic = Dynamic.ToDictionary(properties);
            if (dic.Count == 0)
                return FirstOrDefault<T>("");
            var query = string.Join(" AND ", dic.Select(kv => _dbType.EscapeSqlIdentifier(kv.Key) + "=" + ClauseIfString(kv.Value)));
            return FirstOrDefault<T>(Sql.Builder.Where(query));
        }
        
        //dynamic-overload refers to anonymousprop-overload inwhich it translates into string and uses sql-overload to get result.
        public T First<T>(dynamic properties) {
            return First<T>(new AnonymousProperties<T>(properties));
        }
        public T First<T>(AnonymousProperties<T> properties) {
            var dic = Dynamic.ToDictionary(properties);
            if (dic.Count == 0)
                return FirstOrDefault<T>("");
            var query = string.Join(" AND ", dic.Select(kv => _dbType.EscapeSqlIdentifier(kv.Key) + "=" + ClauseIfString(kv.Value)));
            return First<T>(Sql.Builder.Where(query));
        }

        //returns object because usually when giving a poco object, it sets the primary automatically. not in this case, so we return the object that was mapped.
        public T Insert<T>(dynamic properties) where T : class, new()  {
            return Insert<T>(new AnonymousProperties<T>(properties));
        }
        public T Insert<T>(AnonymousProperties<T> properties) where T : class, new() {
            var t = new T();
            Dynamic.Map<T>(properties.AsExpandoObject, t);
            Insert(t);
            return t; //as object
        }
        
        public int DeleteAnonymous<T>(dynamic properties)where T : class, new() {
            return Delete<T>(new AnonymousProperties<T>(properties)); 
        }
        public int Delete<T>(AnonymousProperties<T> properties) where T : class, new() {
            return deleteMultiple<T>(properties);
        }
        public int Delete<T>() where T : class, new() {
            return deleteMultiple<T>(new AnonymousProperties<T>(new { }));
        }
        private int deleteMultiple<T>(AnonymousProperties<T> props) {
            var data = PocoData.ForType(typeof(T));
            var dic = Dynamic.ToDictionary(props);
            var query = string.Join(" AND ", dic.Select(kv => _dbType.EscapeSqlIdentifier(kv.Key) + "=" + ClauseIfString(kv.Value)));
            return Execute(string.Format("DELETE FROM {0}{1};", _dbType.EscapeTableName(data.TableInfo.TableName), string.IsNullOrEmpty(query) ? "" : " WHERE " + query));
        }
        
        public IEnumerable<T> Query<T>(dynamic properties) {
            return Query<T>(new AnonymousProperties<T>(properties));
        }
        public IEnumerable<T> Query<T>(AnonymousProperties<T> properties) {
            var dic = Dynamic.ToDictionary(properties);
            if (dic.Count == 0)
                return Query<T>("");
            return Query<T>(Sql.Builder.Where(string.Join(" AND ", dic.Select(kv => _dbType.EscapeSqlIdentifier(kv.Key) + "=" + ClauseIfString(kv.Value)))));
        }
        public IEnumerable<T> Query<T>() {
            return Query<T>("");
        }

        public List<T> Fetch<T>(dynamic properties) { return Fetch<T>(new AnonymousProperties<T>(properties)); }
        public List<T> Fetch<T>() { return Fetch<T>(""); }
        public List<T> Fetch<T>(AnonymousProperties<T> properties) {
            return Query<T>(properties).ToList();
        }
        //simple different-name-overload to Count, with promised return of int.
        public int Contains<T>(dynamic properties) {
            return Contains<T>(new AnonymousProperties<T>(properties));
        }
        public int Contains<T>(AnonymousProperties<T> propreties) {
            return Count<T, int>(propreties);
        }
        /*public int Contains<T>(T poco) {
            
        }*//*
        /// <summary>
        /// Completes the poco incase it is not completed already, for e.g. given to Class, Grade and SubGrade -> Id will be added
        /// </summary>
        /// <typeparam name="T">Type of Poco</typeparam>
        /// <param name="poco">As many as possible filled parameters of the 'T' Poco</param>
        /// <returns>Fixed Poco, full of the details</returns>
        /// <exception cref="ArgumentException">Might be thrown where more than 2 fixures were found</exception>
        /// <exception cref="InstanceNotFoundException">Might be thrown where more than 2 fixures were found</exception>
        public void CompletePoco<T>(ref T poco) where T : class {
            var pocoClone = poco;
            var dyn = Dynamic.ToAnonymous(ReflectionTools.GetNotDefaultValues(pocoClone));
            var clones = Count<T, long>(dyn);
            if (clones == 0)
                throw new InstanceNotFoundException("Could not find the complete instance");
            if (clones > 1)
                throw new ArgumentException("Too many results for this sample, be more specific");
            poco = First<T>(dyn);
        }*/

        public int Update<T>(dynamic where, dynamic properties) {
            return Update<T>(new AnonymousProperties<T>(where), new AnonymousProperties<T>(properties));
        }
        public int Update<T>(AnonymousProperties<T> where, AnonymousProperties<T> properties) {
            var t = Internal.PocoData.ForType(typeof(T));
            var props = Dynamic.ToDictionary(properties);
            if (props.Count == 0)
                return 0;
            var w = Dynamic.ToDictionary(where);

            var q = "UPDATE " + _dbType.EscapeTableName(t.TableInfo.TableName) + " SET "
                + string.Join(" ,", props.Select(kv => _dbType.EscapeSqlIdentifier(kv.Key) + "=" + ClauseIfString(kv.Value)));
            if (w.Count > 0)
                q += " WHERE "
                    + string.Join(" AND ", w.Select(kv => _dbType.EscapeSqlIdentifier(kv.Key) + "=" + ClauseIfString(kv.Value)));
            q += ";";
                    
                

            return Execute(q);
        }

        //SELECT c.SubGrade, s.Class_Id FROM classes c, students s WHERE c.Grade = 14; returns all subgrade and class_id where class subgrade is 0
        //INSERT INTO students (Name, LastName, PersonalID, CardKey, Attended, class_Id) VALUES ('asd', 'dsa', 123, 'asd', 1, 1);

        private string ClauseIfString(object value) {
            if (value is string) return "\"" + value + "\"";
            return value.ToString();
        }

        public class NameObjectPair {
            public string Key { get; set; }
            public object Value { get; set; }

            public NameObjectPair(KeyValuePair<string, object> kv) {
                Key = kv.Key;
                Value = kv.Value;
            }

            public NameObjectPair(string Key, string Value) {
                this.Key = Key;
                this.Value = Value;
            }

            public override string ToString() {
                return Key + "=" + Value;
            }

        }
    }
}
#endif