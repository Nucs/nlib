#if NET_4_5
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using nucs.Utils;

namespace nucs.ADO.NET.MySql {
    public static class MySqlTools {
        public static List<string> GetTablesNames(string loginquery) {
            var result = new List<string>();
            using (var conn = new MySqlConnection(loginquery)) {
                using (var comm = new MySqlCommand("SELECT * FROM sys.tables;", conn)) {
                    try {
                        conn.Open();
                        var r = comm.ExecuteReader();
                        while (r.Read()) {
                            result.Add((string) r["name"]);
                        }
                    }
                    catch (MySqlException e) {
                        Console.WriteLine(e.ToString());
                    }
                    finally {
                        conn.Close();
                    }
                }
            }
            return result;
        }

        public static List<string> GetSchemas(string loginquery) {
            var result = new List<string>();
            using (var conn = new MySqlConnection(loginquery))
            {
                using (var comm = new MySqlCommand("SHOW DATABASES;", conn)) {
                    try {
                        conn.Open();
                        var r = comm.ExecuteReader();
                        while (r.Read()) {
                            result.Add((string) r[0]);
                        }
                    }
                    catch (MySqlException e) {
                        Console.WriteLine(e.ToString());
                    }
                    finally {
                        conn.Close();
                    }
                }
            }
            return
                result.Where(s => s != "performance_schema" && s != "information_schema" && s != "mysql" && s != "test")
                    .ToList();
        }

        public static int GetRowsCountByQuery(string loginquery, string query) {
            using (var conn = new MySqlConnection(loginquery)) {
                using (var comm = new MySqlCommand(query, conn)) {
                    try {
                        conn.Open();
                        return Convert.ToInt32(comm.ExecuteScalar());
                    }
                    catch (MySqlException e) {
                        Console.WriteLine(e.ToString());
                    }
                    finally {
                        conn.Close();
                    }
                }
            }
            return -1;
        }


        public static int GetRowsCountByTableName(string loginquery, string tableName) {
            using (var conn = new MySqlConnection(loginquery)) {
                using (var comm = new MySqlCommand("SELECT Count(*) FROM " + tableName + ";", conn)) {
                    try {
                        conn.Open();
                        return Convert.ToInt32(comm.ExecuteNonQuery());
                    }
                    catch (MySqlException e) {
                        Console.WriteLine(e.ToString());
                    }
                    finally {
                        conn.Close();
                    }
                }
            }
            return -1;
        }

        public static bool DropSchema(string adminLognQuery, string schema) {
        using (var conn = new MySqlConnectionAuto(adminLognQuery)) 
            using (var comm = new MySqlCommand("DROP SCHEMA " + schema + ";", conn.Connection)) {
                    try {
                        comm.ExecuteNonQuery();
                    }
                    catch (MySqlException e) {
                        if (e.Number == 1008)
                            return false;
                        Console.WriteLine("Failed executing: ("+e.Number+") "+comm.CommandText);
                    }
            }
            return true;
        }

        public static bool ExecuteScript(string adminLognQuery, string path, string TableName) {
            #region Reading and Filtering
            TableName = TableName.ToLower();
            var builder = new StringBuilder();

            try {
                var file = Tools.GetResourceStream(path);
                using (var reader = new StreamReader(file)) {
                    string line;
                    var query = "";
                    while ((line = reader.ReadLine()) != null) {
                        if (line.Contains("--") || string.IsNullOrEmpty(line))
                            continue;
                        line = line.Replace("techni", TableName).Replace("    ", "");

                        if (!line.Contains(';'))
                            query += line + " ";
                        else {
                            query += line + "\n";
                            builder.Append(query);
                            query = "";
                        }
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            var result = builder.ToString();

            #endregion

            #region Transaction
            try {
                using (var dsad = new MySqlTransaction(adminLognQuery)) {
                    var conn = new MySqlConnection();
                    try {
                        conn.ConnectionString = adminLognQuery;
                        conn.Open();
                    } catch (MySqlConnectionException) {
                        MessageBox.Show("Failed connecting to server, please check the login details!", "Connection Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    var trans = conn.BeginTransaction();
                    var comm = new MySqlCommand(result, conn, trans);
                    try {
                        comm.ExecuteNonQuery();
                        trans.Commit();
                    }
                    catch (Exception) {
                        Console.WriteLine("Error executing the script: " + path + " under table name: " + TableName);
                        trans.Rollback();
                        return false;
                    }
                    finally {
                        conn.Close();
                        conn.Dispose();
                        trans.Dispose();
                    }
                }
            } catch (MySqlException e) {
                Console.WriteLine("Error executing the script: "+e);
                return false;
            }
            return true;
        }
            #endregion
    }
    


    //exe.AddGet("SELECT name FROM SQLITE_MASTER where type='table' order by name", ref tables);

        public sealed class MySqlQueryException : Exception {
            public MySqlQueryException(string message) : base(message) {
            }
        }

        public sealed class MySqlConnectionException : Exception {
            public MySqlConnectionException(string message) : base(message) {
            }
        }
    }

/*using System;using System.Data;using System.Data.SqlClient;  public class clsSqlCommandUpdate{    //Create Connection    SqlConnection thisConnection = new SqlConnection("server=(local)\\SQLEXPRESS;" + "integrated security=sspi;database=Northwind");        public void Main()    {         OpenConnection();         //Insert Rows to make sure they exist        Console.WriteLine("\n");        Console.WriteLine("***Insert Rows to make sure they exist***");        InsertRows();         //Display Rows Before Update        Console.WriteLine("\n");        Console.WriteLine("***Display Rows Before Update***");        SelectRows();         //Update Rows        Console.WriteLine("\n");        Console.WriteLine("***Perform Update***");        UpdateRows();         //Display Rows after update        Console.WriteLine("\n");        Console.WriteLine("***Display Rows After Update***");        SelectRows();         //Clean up with delete of all inserted rows        Console.WriteLine("\n");        Console.WriteLine("***Clean Up By Deleting Inserted Rows***");        DeleteRows();         // Close Connection        thisConnection.Close();        Console.WriteLine("Connection Closed");         Console.ReadLine();    }    void OpenConnection()    {        try        {            // Open Connection            thisConnection.Open();            Console.WriteLine("Connection Opened");        }        catch (SqlException ex)        {            // Display error            Console.WriteLine("Error: " + ex.ToString());        }    }    void SelectRows()    {         try        {            // Sql Select Query             string sql = "SELECT * FROM Employees";            SqlCommand cmd = new SqlCommand(sql, thisConnection);             SqlDataReader dr;            dr = cmd.ExecuteReader();            string strEmployeeID = "EmployeeID";            string strFirstName = "FirstName";            string strLastName = "LastName";             Console.WriteLine("{0} | {1} | {2}", strEmployeeID.PadRight(10), strFirstName.PadRight(10), strLastName);            Console.WriteLine("==========================================");            while (dr.Read())            {                //reading from the datareader                Console.WriteLine("{0} | {1} | {2}",                     dr["EmployeeID"].ToString().PadRight(10),                     dr["FirstName"].ToString().PadRight(10),                     dr["LastName"]);            }            dr.Close();            Console.WriteLine("==========================================");        }         catch (SqlException ex)        {            // Display error            Console.WriteLine("Error: " + ex.ToString());        }     }     void InsertRows()    {          //Insert Rows to make sure row exists before updating        //Create Command object        SqlCommand nonqueryCommand = thisConnection.CreateCommand();         try        {             // Create INSERT statement with named parameters            nonqueryCommand.CommandText = "INSERT  INTO Employees (FirstName, LastName) VALUES (@FirstName, @LastName)";             // Add Parameters to Command Parameters collection            nonqueryCommand.Parameters.Add("@FirstName", SqlDbType.VarChar, 10);            nonqueryCommand.Parameters.Add("@LastName", SqlDbType.VarChar, 20);             // Prepare command for repeated execution            nonqueryCommand.Prepare();             // Data to be inserted            string[] names = { "Wade", "David", "Charlie" };            for (int i = 0; i < = 2; i++)            {                nonqueryCommand.Parameters["@FirstName"].Value = names[i];                nonqueryCommand.Parameters["@LastName"].Value = names[i];                 Console.WriteLine("Executing {0}", nonqueryCommand.CommandText);                Console.WriteLine("Number of rows affected : {0}", nonqueryCommand.ExecuteNonQuery());            }        }        catch (SqlException ex)        {            // Display error            Console.WriteLine("Error: " + ex.ToString());        }        finally        {          }     }     void UpdateRows()    {         try        {            // 1. Create Command            // Sql Update Statement            string updateSql = "UPDATE Employees " + "SET LastName = @LastName " + "WHERE FirstName = @FirstName";            SqlCommand UpdateCmd = new SqlCommand(updateSql, thisConnection);             // 2. Map Parameters             UpdateCmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 10, "FirstName");             UpdateCmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 20, "LastName");             UpdateCmd.Parameters["@FirstName"].Value = "Wade";            UpdateCmd.Parameters["@LastName"].Value = "Harvey";             UpdateCmd.ExecuteNonQuery();        }         catch (SqlException ex)        {            // Display error            Console.WriteLine("Error: " + ex.ToString());        }     }     void DeleteRows()    {         try        {            //Create Command objects            SqlCommand scalarCommand = new SqlCommand("SELECT COUNT(*) FROM Employees", thisConnection);             // Execute Scalar Query            Console.WriteLine("Before Delete, Number of Employees = {0}", scalarCommand.ExecuteScalar());              // Set up and execute DELETE Command            //Create Command object            SqlCommand nonqueryCommand = thisConnection.CreateCommand();            nonqueryCommand.CommandText = "DELETE FROM Employees WHERE " + "Firstname='Wade'  or " + "Firstname='Charlie' AND Lastname='Charlie' or " + "Firstname='David' AND Lastname='David' ";            Console.WriteLine("Executing {0}", nonqueryCommand.CommandText);            Console.WriteLine("Number of rows affected : {0}", nonqueryCommand.ExecuteNonQuery());             // Execute Scalar Query            Console.WriteLine("After Delete, Number of Employee = {0}", scalarCommand.ExecuteScalar());        }         catch (SqlException ex)        {            // Display error            Console.WriteLine("Error: " + ex.ToString());        }     } } */#endif