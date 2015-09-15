#if NET_4_5

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace nucs.ADO.NET.MySql {
    public static class MySqlConnectionTools {
        public static MySqlConnectionTestResults ConfirmConnection(this MySqlConnectionString str) {
            try {
                using (var connn = new MySqlConnection(str.LoginQuery(true))) {
                    try {
                        connn.Open();
                        connn.Close();
                        //Successfully passed first connection test
                    } catch (MySqlException eex) {
                        return new MySqlConnectionTestResults(false, null, eex);
                    }
                }

                using (var conn = str.ToAllAvailableConnection(true)) {
                    try {
                        conn.Open();
                        conn.Close();
                        //Successfully passed second connection test
                    } catch (MySqlException ex) {
                        return new MySqlConnectionTestResults(false, null, ex);
                    }
                }
            } catch (MySqlException e) {
                return new MySqlConnectionTestResults(false, null, e);
            }
            return new MySqlConnectionTestResults(true, str, null);
        }

        public static MySqlConnectionTestResults ConfirmConnection(string str) {
            try {
                using (var connn = new MySqlConnection(str)) {
                    try {
                        connn.Open();
                        connn.Close();
                        //Successfully passed first connection test
                    }
                    catch (MySqlException eex) {
                        return new MySqlConnectionTestResults(false, null, eex);
                    }
                }
            } catch (MySqlException e) {
                return new MySqlConnectionTestResults(false, null, e);
            }
            return new MySqlConnectionTestResults(true, new MySqlConnectionString(str), null);
        }

        public static string TranslateErrorCode(MySqlException err) {
            var errortxt = "";
            switch (err.Number) {
#region ErrorSwitching
                case 175:
                    errortxt = "The specific provider could not be found";
                    break;
                case 1042: //couldn't find mysql service.
                    errortxt = "Unable to find hostname! try 'localhost' as host if the db is stored locally or turn on WAMP Server";
                    break;
                case 1049: // Invalid Database 
                    errortxt ="Wrong Database details, please check the settings";
                    break;
                case 18456: // Login Failed 
                    errortxt = "Login failed, please contact the administrator or see if 'WAMP Server' is running correctly";
                    break;
                case 1044:
                    errortxt = "Wrong Username or password details, please check them again";
                    break;
                case 1045:
                    errortxt = "Wrong Username or password, please check them again";
                    break;
                default:
                    errortxt = err.Message;
                    break;
                #endregion
            }
            errortxt += " ("+err.ErrorCode+")";
            return errortxt;
        }
        
    }

    public struct MySqlConnectionTestResults {
            public bool Connected;
            public int ErrorCode { get { return Exception.Number; } }
            public MySqlException Exception;
            public string Description;
            public MySqlConnectionString SuccessfulQuery;

            public MySqlConnectionTestResults(bool connectionState, MySqlConnectionString query/* = null */, MySqlException ex/* = null*/) {
                if ((Connected = connectionState) == true) {
                    Exception = null;
                    Description = "Connection was established successfully";
                    SuccessfulQuery = query;
                    return;
                }
                if (ex == null)
                    throw new InvalidOperationException("Connection State was passed as false, but exception is null!");
                Exception = ex;
                Description = MySqlConnectionTools.TranslateErrorCode(ex);
                SuccessfulQuery = null;
            }
        }
}
#endif