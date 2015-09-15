#if NET_4_5
using System;
using MySql.Data.MySqlClient;

namespace nucs.ADO.NET.MySql {
    public class MySqlTransaction : IDisposable {
        private readonly MySqlConnection connection;
        public MySqlTransaction(string connQuery) {
            connection = new MySqlConnection(connQuery);
            var comm = new MySqlCommand("SET autocommit = 0;", connection);
            connection.Open();
            comm.ExecuteNonQuery();
            connection.Close();
            comm.Dispose();
        }


        public void Close() {
            Dispose();
        }

        public void Dispose() {
            var comm = new MySqlCommand("SET autocommit = 1;", connection);
            connection.Open();
            comm.ExecuteNonQuery();
            connection.Close();
            comm.Dispose();
            connection.Dispose();
        }

    }
}
#endif