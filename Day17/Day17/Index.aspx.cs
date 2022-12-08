using System;
using Day17.Models;

namespace Day17
{
    public partial class Index : System.Web.UI.Page
    {
        dbmemberEntities1 db = new dbmemberEntities1();

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Member member = new Member();
          
            member.Account = TextBox1.Text;
            member.Password = Password1.Value;

            db.Member.Add(member);
            db.SaveChanges();

            Response.Redirect("Index.aspx");
        }
    }
}

/*
SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\USER\Desktop\ASPMVC\Day17\Day17\App_Data\dbmember.mdf;Integrated Security=True");
using (SqlCommand Cmd = new SqlCommand())
{
    Cmd.Connection = connection;
    connection.Open();
    Cmd.CommandText = @"INSERT INTO [Member]
                            (
                            [Account]
                            ,[Password]) 
                        VALUES (
                            @account
                            ,@password)";
    Cmd.Parameters.AddWithValue("@account", TextBox1.Text );
    Cmd.Parameters.AddWithValue("@password", TextBox2.Text );
                
    Cmd.ExecuteNonQuery();
    // 關閉資料庫連線
    connection.Close();
               
}
*/