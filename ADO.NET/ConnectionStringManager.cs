using System.Diagnostics;
using System.Linq;
namespace nucs.ADO.NET {
    //[DebuggerStepThrough]
   /* public class ConnectionStringManager {
        //TODO make a manager to prepare query for need, if there is no database, send query w/o.. and so on.
        //TODO have public strings of each part in query
        public string InputtedConnString;
        private string tmpSave;
        private string host      { get; set; }
        private string database  { get; set; }
        private string userName  { get; set; }
        private string password  { get; set; }

        internal string Host     { get { return host;     } set { host     = value; Save();} }
        internal string Database { get { return database; } set { database = value; Save();} }
        internal string UserName { get { return userName; } set { userName = value; Save();} }
        internal string Password { get { return password; } set { password = value; Save();} }

        internal const string PernamentAddons = "Charset=utf8; Allow Zero Datetime=True;";
        /* design backups
        internal static string Host     { get { return Host;     } set { Host     = value; Save(); } }
        internal static string Database { get { return Database; } set { Database = value; Save(); } }
        internal static string UserName { get { return UserName; } set { UserName = value; Save(); } }
        internal static string Password { get { return Password; } set { Password = value; Save(); } }
        internal static string Addons   { get { return Addons;   } set { Addons   = value; Save(); } }
        #1#

        //server=localhost; database=techni; uid=root; pwd=;
        /// <summary>
        /// host, database, username, password, addons
        /// </summary>
        internal string ConnectionQuery      { get { return "server=" + host + "; " + (!string.IsNullOrEmpty(database) ? ("database=" + database + "; ") : "") + "uid=" + userName + "; pwd=" + password + "; "+ PernamentAddons; } }
        /// <summary>
        /// host, username, password, addons
        /// </summary>
        internal string LoginQuery { get { return "server=" + host + "; uid=" + userName + "; pwd=" + password + "; " + PernamentAddons; } }
        /// <summary>
        /// returns: return "server=localhost; uid=root; pwd=; " + addons;
        /// </summary>
        internal string DefaultQuery { get { return "server=localhost; uid=root; pwd=; " + PernamentAddons; } }
        /// <summary>
        /// returns: return "server=localhost; uid=root; pwd=******; " + addons;
        /// </summary>
        internal string UserSafeQuery { get { return "server=" + host + "; uid=" + userName + "; pwd=******; " + PernamentAddons; } }

        //server=localhost; uid=root; pwd=; Charset=utf8; Allow Zero Datetime=True;
        [DebuggerStepThrough]
        public ConnectionStringManager(string connStr) {
            if (connStr.Contains(';')) {
                var a =
                    connStr.Split(';').ToList().Select(
                        item => {
                            try {
                                if (item[0] == ' ') 
                                    return item.Substring(1, item.Length - 1);} catch {}
                                return item;
                        }).ToList();
                host = a.First(j => j.Contains("server")).Split('=')[1];
                try { database = a.First(j => j.Contains("database")).Split('=')[1]; } catch{}
                
                userName = a.First(j => j.Contains("uid")).Split('=')[1];
                password = a.First(j => j.Contains("pwd")).Split('=')[1];
                Save();
            }
            Initialize(connStr);
        }

        private void Initialize(string connStr) {
            if (string.IsNullOrEmpty(connStr)) {
                tmpSave = "localhost" + "." + "" + "." + "root" + "." + "" + "." + PernamentAddons;
            }

            var str = tmpSave.Split('.').ToList();
            host = str[0];
            database = str[1];
            userName = str[2];
            password = str[3];
            Save();
        }

        internal void Update(string _host, string _database, string _username, string _password, string _addons) {
            host = _host;
            database = _database;
            userName = _username;
            password = _password;
            Save();
        }

        internal bool IsDefaultQuery() {
            return (host == "localhost" && userName == "root" && string.IsNullOrEmpty(database) && string.IsNullOrEmpty(password));
        }

        private void Save() {
            tmpSave = host + "." + database + "." + userName + "." + password;
        }

    }*/
}