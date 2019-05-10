using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAssessment
{
    public partial class Site : System.Web.UI.MasterPage, IPostBackEventHandler
    {
        private string ConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;
        private SqlConnection conn;
        private SqlCommand comm;
        private Int64 id = 0;
        private bool IsAutorised = false;
        private string Name = "";
        protected void Page_Load(object sender, EventArgs e)
        {
 
        }

        public void RaisePostBackEvent(string eventArgument) {
            switch (eventArgument)
            {
                case "Register":
                    RegisterBtn();
                    break;
            }
        }

        public static void ShowAlert(Control ctr, string alert)
        {
            alert = alert.Replace('\n', ' ');
            alert = alert.Replace('\'', ' ');
            alert = alert.Replace('\"', ' ');
            ScriptManager.RegisterStartupScript(ctr, ctr.GetType(), "myalert", 
                "alert('" + alert + "');", true);
        }

        private void CheckTableUsers()
        {
            conn = new SqlConnection(ConnString);
            conn.Open();
            comm = new SqlCommand("select max(id) as max_id from tblUsers", conn);

            SqlDataReader result = null;
            try
            {
                result = comm.ExecuteReader();
            }
            catch (Exception ex)
            {
                ShowAlert(this, "error happens:" + ex.ToString());
            }
            if (result == null)
            {
                comm = new SqlCommand("CREATE TABLE tblUsers ( [Id] INT NOT NULL, [Name] VARCHAR(50)  primary key NOT NULL, [Pass] VARCHAR(50) NOT NULL,  [Rights] INT NULL);insert into tblUsers values(1,'Ckpyt', 'ghjwtccjh', 255);", conn);
                comm.ExecuteNonQuery();
                id = 2;
            }
            else
            {
                try
                {
                    if (result != null && result.Read())
                    {
                        var res = result[0];
                        id = Convert.ToInt64(res) + 1;

                    }
                }
                catch (Exception ex)
                {
                    id = 1;
                    ShowAlert(this, "error happens:" + ex.ToString());
                }
            }

            conn.Close();
        }

        protected void RegisterUser()
        {
            conn = new SqlConnection(ConnString);
            conn.Open();
            string s_quarry = "insert into tblUsers values(" + id.ToString()
                            + "," + Login.Text + "," + Password.Text + ",1);";
            comm = new SqlCommand(s_quarry, conn);

            try
            {
                SqlDataReader result = comm.ExecuteReader();
            }
            catch (Exception ex)
            {
                ShowAlert(this, "error happens:" + ex.Message);
                return;
            }

            ShowAlert(this, "user " + Login.Text + " successful register");
            conn.Close();
        }

        [WebMethod]
        protected Boolean RegisterBtn()
        {
            if (Password.Text.CompareTo(Confirm.Text) != 0)
            {
                ShowAlert(this, "password and confirm password do not match");
                return false;
            }
            if(Login.Text.Length == 0 || Password.Text.Length == 0)
            {
                ShowAlert(this, "Login and password should to contain at least 1 symbol");
                return false; ;
            }

            CheckTableUsers();

            conn = new SqlConnection(ConnString);
            conn.Open();
            
            string S_quarry = "select * from tblUsers where(Name='" + Login.Text
                + "');";
            SqlCommand comm = new SqlCommand(S_quarry, conn);

            SqlDataReader result = null;
            try
            {
                result = comm.ExecuteReader();
            }
            catch (Exception ex)
            {
                ShowAlert(this, "error happens:" + ex.Message);
                conn.Close();
                return false;
            }
            if (result != null)
            {
                try
                {
                    if (result != null && result.Read())
                    {
                        id = Convert.ToInt64(result[0]);
                        string name = Convert.ToString(result[1]);
                        string pass = Convert.ToString(result[2]);
                        int rights = Convert.ToInt32(result[3]);
                        if (name.CompareTo(Login.Text) == 0 && pass.CompareTo(Password.Text) == 0)
                        {
                            IsAutorised = true;
                            Name = UserName.Text;
                            ShowAlert(this, Name + " welcome on my site!");
                            conn.Close();
                            return true;
                        }
                        else
                        {
                            ShowAlert(this, "user " + Login.Text + " already register");
                            conn.Close();
                            return false;
                        }
                    }
                    else
                    {
                        RegisterUser();
                    }
                }
                catch (Exception ex)
                {
                    id = 1;
                    ShowAlert(this, "error happens:" + ex.Message);
                }
            }
            else
            {
                RegisterUser();
            }
            conn.Close();
            return true;
        }
    

        protected void LoginBtn_Click(object sender, EventArgs e)
        {
            CheckTableUsers();
            conn = new SqlConnection(ConnString);
            conn.Open();
            string S_quarry = "select * from tblUsers where(Name='" + UserName.Text
                + "' and Pass='" + UserPassword.Text + "');";
            comm = new SqlCommand(S_quarry, conn);

            SqlDataReader result = null;
            try
            {
                result = comm.ExecuteReader();
            }
            catch (Exception ex)
            {
                ShowAlert(this, "error happens:" + ex.Message);
                return;
            }
            if (result == null)
            {
                ShowAlert(this, "this login and password not used in this site");
                
                return;
            }
            else
            {
                try
                {
                    if (result != null && result.Read())
                    {
                        id = Convert.ToInt64(result[0]);
                        int rights = Convert.ToInt32(result[3]);
                        IsAutorised = true;
                        Name = UserName.Text;
                        ShowAlert(this, Name + " welcome on my site!");
                        
                    }
                    else
                    {
                        ShowAlert(this, "this login and password not used in this site");
                        
                    }
                }
                catch (Exception ex)
                {
                    id = 1;
                    ShowAlert(this, "error happens:" + ex.Message);
                }
            }

            conn.Close();
        }
    }
}