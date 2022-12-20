using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using MySql.Data.MySqlClient;

namespace Day18.App_Code
{
    
    public class dbCommand
    {
        public dbCommand()
        {
            //
            //  在此加入建構函式的程式碼
            //
        }
        public static MySqlConnection conn()
        {
            return new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);
        }
        public static bool ExecuteSQL(MySqlCommand command)
        {
            bool state = false;
            if (command.ToString() != "" && command.ToString() != null)
            {
                using (MySqlConnection sqlconnection = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                {
                    try
                    {
                        command.Connection = sqlconnection;
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        state = true;
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                    command.Connection.Close();
                    command.Connection.Dispose();
                }
            }
            return state;
        }
        public static void ExecuteSQL(string queryString)
        {
            if (queryString != "" && queryString != null)
            {
                using (MySqlConnection sqlconnection = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(queryString, sqlconnection))
                    {
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();
                        command.Connection.Dispose();
                    }
                }
            }
        }
        public static DataTable GetTable(MySqlCommand command)
        {
            DataTable data = new DataTable();
            using (MySqlConnection sqlconnection = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
            {
                command.Connection = sqlconnection;
                using (MySqlDataAdapter MySqla = new MySqlDataAdapter(command))
                {
                    MySqla.Fill(data);
                }
                command.Connection.Close();
                command.Connection.Dispose();
            }

            return data;
        }
        public static DataTable GetTable(string sql)
        {
            DataTable data = new DataTable();
            if (sql != "" && sql != null)
            {
                using (MySqlConnection sqlconnection = new MySqlConnection(WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString))
                {
                    using (MySqlDataAdapter MySqla = new MySqlDataAdapter(sql, sqlconnection))
                    {
                        MySqla.Fill(data);
                    }
                    sqlconnection.Close();
                    sqlconnection.Dispose();
                }
            }
            return data;
        }
    }
}