using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Day18
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
            string query = "SELECT * FROM tmember";
            string str = "<table>";
            DataTable dt = new DataTable();
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandText = query;
            commandDatabase.CommandTimeout = 60;
            try{ 
                commandDatabase.Connection = databaseConnection;
                using (MySqlDataAdapter MySqla = new MySqlDataAdapter(commandDatabase))
                {
                    MySqla.Fill(dt);
                }
                for (int i=0; i < dt.Rows.Count;i++)
                {
                    str += "<tr><td style='color:red'>";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        str += "<td style='color:red'>" + dt.Rows[i][j].ToString() + "</td>";
                    }
                    str+= "</td></tr>";
                }
                str += "</table>";
                //<div id="mainDiv" runat="server">
                mainDiv.InnerHtml = str;
                //databaseConnection.Close();
                Label1.Text = "success";
                commandDatabase.Connection.Close();
                commandDatabase.Connection.Dispose();
            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;
            }
        }
    }
}