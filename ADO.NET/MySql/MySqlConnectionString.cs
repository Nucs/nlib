using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;
using nucs.SystemCore.String;

namespace nucs.ADO.NET.MySql {
    /// <summary>
    /// Represents a connection string with a charset constant of utf8, but changeable
    /// </summary>
    public class MySqlConnectionString {
        public string OriginalString { get; private set; }
        public MySqlConnectionString(string query, string constants = "") {
            UpdateConstants(constants);
            Process(query);
        }

        
        private List<MySqlConnectionStringNameValuePair> translateToNVP(string query) {
            var kvs = query.Split(';').Where(v => string.IsNullOrEmpty(v) == false && v.Split('=').Length == 2)
                .Select(v =>
                {
                    var nv = v.Split('=');
                    return new KeyValuePair<string, string>(nv[0].Trim().Trim('\'').ToLowerInvariant(), nv[1].Trim().Trim('\''));
                }).Select(kv => (MySqlConnectionStringNameValuePair)kv).ToList();
            return kvs;
        }

        /// <summary>
        /// Given a query for ex: "server='localhost';uid='potato';pwd='';port=3306;" will be assigned to strong-typed values.
        /// </summary>
        /// <param name="query"></param>
        public void Process(string query) {
            if (query.IsNullOrEmpty())
                return;

            var kvs = translateToNVP(query);

            if (kvs.Count == 0)
                throw new ArgumentException("Passed argument 'query' is considered empty after proccessing, please check its context: '"+query+"'", "query");
            if (Constants != null && Constants.Count > 0)
                kvs = kvs.Where(kv=>Constants.Any(conkv=> conkv.Key.Equals(kv.Key)) == false).ToList();

            #region MustHave
            var a = kvs.FirstOrDefault(kv => kv.Key == "server");
            if (a == null || a.Key == null)
                throw new ArgumentException("Could not find 'server' inside the query: '"+query+"'", "query");

            var b = kvs.FirstOrDefault(kv => kv.Key == "uid" || kv.Key == "user id");
            if (b == null || b.Key == null)
                throw new ArgumentException("Could not find 'uid' inside the query: '" + query + "'", "query");

            var c= kvs.FirstOrDefault(kv => kv.Key == "pwd" || kv.Key == "password");
            if (c == null || c.Key == null)
                throw new ArgumentException("Could not find 'pwd'|'password' inside the query: '" + query + "'", "query");
            
            Clear(); //clears only after verifying the must have exists
            host = a;
            userName = b;
            password = c;

            #endregion

            #region Optional
            var d = kvs.FirstOrDefault(kv => kv.Key == "database");
            if (d != null && d.Key != null)
                database = d;

            var _port = kvs.FirstOrDefault(kv => kv.Key == "port");
            if (_port != null && _port.Key != null)
                port = _port;

            /*var __charset = kvs.FirstOrDefault(kv => kv.Key == "charset");
            if (_port.Key != null)
                charset = __charset;*/

            Others = kvs.Where(kv => kv.Key != "server" && kv.Key != "database" 
                && kv.Key != "uid" && kv.Key != "user id" && kv.Key != "pwd" && kv.Key != "password"
                && kv.Key != "port" && kv.Key != "charset")
                .ToList();
            #endregion

            OriginalString = query;
        }

        public void UpdateConstants(string constants) {
            if (Charset == "")
                Charset = "utf8";
            if (constants.IsNullOrEmpty())
                return;

            var kvs = translateToNVP(constants);

            if (kvs.Count == 0)
                return;

            Constants = kvs.Where(kv => kv.Key != "server" && kv.Key != "database"
                && kv.Key != "uid" && kv.Key != "user id" && kv.Key != "pwd" && kv.Key != "password"
                && kv.Key != "port" && kv.Key != "charset")
                .Select(kv => kv).ToList();
            var others = kvs.Except(Constants);
            MySqlConnectionStringNameValuePair a;
            if ((a = others.FirstOrDefault(pair => pair.Key.ToLowerInvariant() == "charset")) != null) 
                charset = a;
        }

        private void Clear() {
            host = null;
            database = null;
            userName = null;
            password = null;
            port = null;
            Others = null;
        }

        #region Properties
        private MySqlConnectionStringNameValuePair host;
        private MySqlConnectionStringNameValuePair database;
        private MySqlConnectionStringNameValuePair userName;
        private MySqlConnectionStringNameValuePair password;
        private MySqlConnectionStringNameValuePair port;
        private MySqlConnectionStringNameValuePair charset;
        public List<MySqlConnectionStringNameValuePair> Constants { get; set; }
        public List<MySqlConnectionStringNameValuePair> Others { get; set; }

        public string Host { get { if (host != null) return host.Value; return ""; } set { if (host == null) host = new MySqlConnectionStringNameValuePair("server", value); else host.Value = value; } }
        public string Database { get { if (database != null) return database.Value; return ""; } set { if (database == null) database = new MySqlConnectionStringNameValuePair("database", value); else database.Value = value; } }
        public string UserName { get { if (userName != null) return userName.Value; return ""; } set { if (userName == null) userName = new MySqlConnectionStringNameValuePair("uid", value); else userName.Value = value; } }
        public string Password { get { if (password != null) return password.Value; return ""; } set { if (password == null) password = new MySqlConnectionStringNameValuePair("pwd", value); else password.Value = value; } }
        public string Port { get { if (port != null) return port.Value; return ""; } set { if (port == null) port = new MySqlConnectionStringNameValuePair("port", value); else port.Value = value; } }
        public string Charset { get { if (charset != null) return charset.Value; return ""; } set { if (charset == null) charset = new MySqlConnectionStringNameValuePair("charset", value); else charset.Value = value; } }


        public string OthersStringy { get { if (Others != null && Others.Count > 0) return stringfy(Others); return ""; } }
        public string ConstantsStringy { get { if (Constants != null && Constants.Count > 0) return stringfy(Constants); return ""; } }

        public bool IsDefaultQuery { get { try { return  Host == "localhost" && UserName == "root" && Password == "" && Database == ""; } catch { return false; } } }
        #endregion

        #region Output

        /// <summary>
        /// Full connection query, all properties inserted, must include database!
        /// </summary>
        /// <param name="includeOthers"></param>
        /// <returns></returns>
        public string ConnectionQuery(bool includeOthers = true) {
            try {
                return host + IfNullThrow(database) + IfNotNull(port) + userName + password + IfNotNull(charset) + IfNotEmptyOrNull(Constants) + IfTrueToInsertOthers(includeOthers);
            } catch (Exception inner) {
                throw new MySqlConnectionQueryException("Failed creating a connection query as requested, see inner exception", inner);
            }
        }

        /// <summary>
        /// host, username, password, addons
        /// </summary>
        public string LoginQuery(bool includeOthers = true) {
            try {
                return "" + host + IfNotNull(port) + userName + password + IfNotNull(charset) + IfNotEmptyOrNull(Constants) + IfTrueToInsertOthers(includeOthers);
            } catch (Exception inner) {
                throw new MySqlConnectionQueryException("Failed creating a connection query as requested, see inner exception", inner);
            }
        }

        /// <summary>
        /// returns: return "server=localhost; uid=root; pwd=; " + addons;
        /// </summary>
        public string DefaultQuery(bool includeOthers = false) {
            try {
                return "server=localhost; uid=root; pwd=;" + IfNotNull(charset) + IfNotEmptyOrNull(Constants) + IfTrueToInsertOthers(includeOthers);
            } catch (Exception inner) {
                throw new MySqlConnectionQueryException("Failed creating a connection query as requested, see inner exception", inner);
            }
        }

        /// <summary>
        /// returns: return "server=localhost; uid=root; pwd=******; " + addons;
        /// </summary>
        public string UserSafeQuery(bool includeOthers = true) {
            try {
                return "" + host + IfNullThrow(database) + IfNotNull(port) + userName + hidePassword(password) + IfNotNull(charset) + IfNotEmptyOrNull(Constants) + IfTrueToInsertOthers(includeOthers);
            } catch (Exception inner) {
                throw new MySqlConnectionQueryException("Failed creating a connection query as requested, see inner exception", inner);
            }
        }

        /// <summary>
        /// adds all possible data to the query that was received.
        /// </summary>
        /// <param name="includeOthers"></param>
        /// <returns></returns>
        public string AllAvailable(bool includeOthers = true) {
            try {
                return host + IfNotNull(database) + IfNotNull(port) + userName + password + IfNotNull(charset) + IfNotEmptyOrNull(Constants) + IfTrueToInsertOthers(includeOthers);
            } catch (Exception inner) {
                throw new MySqlConnectionQueryException("Failed creating a connection query as requested, see inner exception", inner);   
            }
        }


        #region Design Methods
        

        private string IfNullThrow(MySqlConnectionStringNameValuePair nv, [CallerMemberName] string memberName = "") {
            if (nv == null)
                throw new InvalidOperationException("Could not return string inside method "+memberName+" because the tested NameValue is null");
            return nv.ToString();
        }

        private string IfNotNull(MySqlConnectionStringNameValuePair nv) {
            if (nv == null)
                return "";
            return nv.ToString();
        }

        private string stringfy(IEnumerable<MySqlConnectionStringNameValuePair> nvs) {
            if (nvs == null || !nvs.Any()) return "";
            return string.Join("", nvs.Select(nv => nv.ToString()));
        }

        private string hidePassword(MySqlConnectionStringNameValuePair nv_pass) {
            var new_nv = new MySqlConnectionStringNameValuePair(nv_pass.Key, nv_pass.Value);
            var s = new_nv.Value.ToCharArray();
            if (s.Length > 1) { //yes above 1, means 2 or more.
                for (int i = 2; i < s.Length; i++) {
                    s[i] = '*';
                }
                new_nv.Value = new string(s);
                return new_nv.ToString();
            }
            new_nv.Value = "******";
            return new_nv.ToString();
        }

        private string IfTrueToInsertOthers(bool boolean) {
            return boolean ? stringfy(Others) : "";
        }

        private string IfNotEmptyOrNull(List<MySqlConnectionStringNameValuePair> list) {
            if (list != null && list.Count > 0)
                return stringfy(list);
            return "";
        }

        #endregion

        #endregion

        #region Connection Creator
        public MySqlConnection ToLoginConnection(bool includeOthers = true) {
            return new MySqlConnection(LoginQuery(includeOthers));
        }

        public MySqlConnection ToDatabaseConnection(bool includeOthers = true) {
            return new MySqlConnection(ConnectionQuery(includeOthers));
        }

        public MySqlConnection ToDefaultConnection() {
            return new MySqlConnection(GetDefaultQuery());
        }

        public MySqlConnection ToAllAvailableConnection(bool includeOthers) {
            var a = AllAvailable(includeOthers);
            return new MySqlConnection(a);
        }

        public MySqlConnectionAuto ToAutoLoginConnection(bool includeOthers = true) {
            return new MySqlConnectionAuto(LoginQuery(includeOthers));
        }

        public MySqlConnectionAuto ToAutoDatabaseConnection(bool includeOthers = true) {
            return new MySqlConnectionAuto(ConnectionQuery(includeOthers));
        }

        public MySqlConnectionAuto ToAutoDefaultConnection() {
            return new MySqlConnectionAuto(GetDefaultQuery());
        }

        public MySqlConnectionAuto ToAutoAllAvailableConnection(bool includeOthers) {
            return new MySqlConnectionAuto(AllAvailable(includeOthers));
        }

        #endregion

        #region Static
        /// <summary>
        /// returns: return "server=localhost; uid=root; pwd=;"
        /// </summary>
        public static string GetDefaultQuery() {
            return "server=localhost; uid=root; pwd=;";
        }
        #endregion

        public override string ToString() {
            return AllAvailable(true);
        }

    }

    public sealed class MySqlConnectionStringNameValuePair {
        public string Key { get; set; }
        public string Value { get; set; }
        public MySqlConnectionStringNameValuePair(string key, string val) {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be empty or null", "key");
            if (val == null)
                throw new ArgumentException("Value cannot be null", "val");
            Key = key;
            Value = val;
        }

        public MySqlConnectionStringNameValuePair(KeyValuePair<string, string> kv) : this(kv.Key, kv.Value) {}

        public static MySqlConnectionStringNameValuePair Create(KeyValuePair<string, string> kv) {
            return Create(kv.Key, kv.Value);
        }

        public static MySqlConnectionStringNameValuePair Create(string key, string val) {
            if (key == null || val == null)
                return null;
            return new MySqlConnectionStringNameValuePair(key, val);
        }

        public static implicit operator MySqlConnectionStringNameValuePair(KeyValuePair<string, string> kv) {
            return kv.Key == null ? null : new MySqlConnectionStringNameValuePair(kv);
        }

        public override string ToString() {
            return Key + "=" + Value + ";";
        }

        public override bool Equals(object obj) {
            if (obj is MySqlConnectionStringNameValuePair) {
                return (obj as MySqlConnectionStringNameValuePair).Key.ToLowerInvariant().Equals(this.Key.ToLowerInvariant());
            }
            return false;
        }
    }

    [DebuggerStepThrough]
    public class MySqlConnectionQueryException : Exception {
        public MySqlConnectionQueryException(string message) : base(message) {}
        public MySqlConnectionQueryException(string message, Exception innerException) : base(message, innerException) { }
        public MySqlConnectionQueryException() { }
    }
}
