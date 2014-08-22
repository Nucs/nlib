using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace nucs.ADO.NET.MySql {
    public class MySqlConnectionAuto : IDisposable {
        public readonly MySqlConnection Connection;
        public readonly MySqlCommand Command;

        public MySqlConnectionAuto(string connectionString) {
            string error;
            if ((error = TestConnection(connectionString)) != "true")
                // //maybe change it to /writeline?
                throw new MySqlConnectionException(error); //maybe change it to /writeline?
            Connection = new MySqlConnection(connectionString);
            Command = new MySqlCommand {Connection = Connection};
            Open();
        }

        public MySqlConnectionAuto(string connectionString, string command, bool AutoExecute = false, bool AutoDispose = false) {
            string error;
            if ((error = TestConnection(connectionString)) != "true")
                throw new MySqlConnectionException(error); //maybe change it to /writeline?
            Connection = new MySqlConnection(connectionString); 
            Command = new MySqlCommand(command, Connection);

            Open();
            if (AutoExecute) {
                Command.ExecuteNonQuery();
                if (AutoDispose) {
                    Dispose();
                }
            }
        }

        private void Open() {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
        }

        private void Close() {
            if (Connection.State == ConnectionState.Open)
                Connection.Close();
        }

        /// <summary>
        /// tests the connection based on connectionString parameter.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>if succeeds returns a string saying "true", otherwise returns the error string.</returns>
        public string TestConnection(string connectionString) {
            try {
                var connMgr = new MySqlConnectionString(connectionString);
                string result;
                using (var conn = new MySqlConnection(connectionString)) {
                    try {
                        conn.Open();
                        conn.Close();
                        result = "true";
                    } catch (MySqlException ex) {

                        switch (ex.Number) {
                            case 175:
                                result = "The specific provider could not be found. (175)";
                                break;
                            case 1042: //couldn't find mysql service.
                                var s = "Unable to find hostname: '" +
                                        connMgr.Host + "'";
                                if (connMgr.Host.ToLowerInvariant() !=
                                    "localhost") s += "try 'localhost', else";
                                s += ", contact the administrator." + " (" + ex.Number + ")";
                                result = s;
                                break;
                            case 1049: // Invalid Database 
                                result =
                                    "Wrong Database details, please contact the administrator or select one by yourself (Only if you know what you are doing!)." +
                                    " (" + ex.Number + ")";
                                break;
                            case 18456: // Login Failed 
                                result =
                                    "Login failed, please contact the administrator or see if 'WAMP Server' is running correctly" +
                                    " (" + ex.Number + ")";
                                break;
                            case 1044:
                                result = "Wrong Username or password details, please check them again" + " (" +
                                         ex.Number + ")";
                                break;
                            case 1045:
                                result = "Wrong Username or password, please check them again" + " (" + ex.Number + ")";
                                break;
                            default:
                                result =
                                    "Could not connect to the MySql server, please see if 'WAMP Server' is running correctly or contact the administrator." +
                                    "(" + ex.Number + ")";
                                break;
                        }
                    }
                }
                return result;
            } catch (ArgumentException) {
                return "Error Initializing MySqlConnection object, please contact administrator.";
            }
        }

        public static implicit operator MySqlConnection(MySqlConnectionAuto auto) {
            return auto.Connection;
        }

        public static implicit operator MySqlCommand(MySqlConnectionAuto auto) {
            return auto.Command;
        }

        public void Dispose() {
            Close();
            Command.Dispose();
            Connection.Dispose();
        }

        public static MySqlConnectionAuto TryCreate(string connectionString) {
            try {
                return new MySqlConnectionAuto(connectionString);
            } catch {return null;}
        }

        public static MySqlConnectionAuto TryCreate(string connectionString, string command, bool AutoExecute = false, bool AutoDispose = false) {
            try {
              return new MySqlConnectionAuto(connectionString, command, AutoExecute, AutoDispose);  
            } catch {return null;}
        }


    }


}