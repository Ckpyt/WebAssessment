using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAssessment
{
    public partial class admin : System.Web.UI.Page
    {
        private static string ConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            BindRepeater();
            BindRepeaterCategory();
        }

        /// <summary>
        /// bind all the users data
        /// </summary>
        private void BindRepeater()
        {
            string constr = ConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;
            MySqlConnection _con = new MySqlConnection(constr);
            MySqlDataAdapter da = new MySqlDataAdapter("Select AspNetUsers.Id, AspNetUsers.UserName, AspNetUsers.Email, tblDescription.Description From AspNetUsers, tblDescription where(tblDescription.Id=AspNetUsers.Id);", _con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            rptUsers.DataSource = ds.Tables[0];
            rptUsers.DataBind();

        }

        /// <summary>
        /// bind all the categories data
        /// </summary>
        private void BindRepeaterCategory()
        {
            string constr = ConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;
            MySqlConnection _con = new MySqlConnection(constr);
            MySqlDataAdapter da = new MySqlDataAdapter("Select * From tblCategory;", _con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            rptCategory.DataSource = ds.Tables[0];
            rptCategory.DataBind();
        }

        /// <summary>
        /// confirm to delete user
        /// </summary>
        public void ConfirmButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string id = btn.CommandArgument;
            profile.DeleteAccount(id, this);
            Response.Redirect("~/admin.aspx");
        }

        /// <summary>
        /// fill selected category data to edit category table
        /// </summary>
        public void EditCategoryButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string id = btn.CommandArgument;
            string constr = ConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;
            MySqlConnection _con = new MySqlConnection(constr);
            _con.Open();
            MySqlCommand comm = new MySqlCommand("Select * From tblCategory where(id=" + id + ");", _con);
            try
            {
                var result = comm.ExecuteReader();
                if(result.HasRows && result.Read())
                {
                    CatId.Text = Convert.ToString(result[0]);
                    Category.Text = Convert.ToString(result[1]);

                    CatId.Text = CatId.Text.Split(' ')[0];
                   
                    EditCat.Text = "Edit";
                }

            }catch(Exception ex)
            {
                MySite.ShowAlert(this, "error happens:" + ex.ToString());
            }

        }

        /// <summary>
        /// fill user by id to user's edit table
        /// </summary>
        /// <param name="id">user's id</param>
        public void ShowUser(string id)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.FindById(id);
            Login.Text = user.UserName;
            //save e-mail
            UserEmail.Text = user.Email;
            DisableAccount.Checked = user.LockoutEnabled;
            if (user.LockoutEndDateUtc != null)
                Time.Text = user.LockoutEndDateUtc.Value.Date.ToString("yyyy-MM-dd");
            else
                Time.Text = "";

            Page.ClientScript.RegisterStartupScript(GetType(), "MyKey", "SetText();", true);

            MySqlConnection conn = new MySqlConnection(ConnString);
            MySqlCommand comm;

            conn.Open();
            comm = new MySqlCommand("select Description from tblDescription where(id='" + user.Id + "')", conn);

            MySqlDataReader result = null;
            try
            {
                result = comm.ExecuteReader();
                if (result.HasRows && result.Read())
                {
                    var desc = result[0];
                    string descr = Convert.ToString(desc);

                    Description.Text = descr;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MySite.ShowAlert(this, "error happens:" + ex.ToString());
            }
        }


        public void ShowUser(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string id = btn.CommandArgument;

            ShowUser(id);
        }

        /// <summary>
        /// save user's data
        /// </summary>
        protected void UpdateAccount_Click(object sender, EventArgs e)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var usr = userManager.FindByName(Login.Text);
            if (usr == null) return;

            if(usr.Email.CompareTo(UserEmail.Text) != 0)
                userManager.SetEmail(usr.Id, UserEmail.Text);

            if (usr.LockoutEnabled != DisableAccount.Checked)
            {
                userManager.SetLockoutEnabled(usr.Id, DisableAccount.Checked);
            }

            if (DisableAccount.Checked)
            {
                if (Time.Text.Length > 0)
                {
                    DateTime dtm = DateTime.Parse(Time.Text + " 23:00:00");
                    userManager.SetLockoutEndDate(usr.Id, dtm);
                }
                else
                {
                    userManager.SetLockoutEnabled(usr.Id, false);
                }

            }
            
            MySqlConnection conn = new MySqlConnection(ConnString);
            MySqlCommand comm;

            conn.Open();

            comm = new MySqlCommand("update tblDescription set Description='" + Description.Text
                + "' where(Id='" + usr.Id.ToString() + "');", conn);

            try
            {
                comm.ExecuteReader();
            }
            catch (Exception ex)
            {
                MySite.ShowAlert(this, "Error happens:" + ex.Message);
            }
            conn.Close();
            ShowUser(usr.Id);
        }

        /// <summary>
        /// save category's data
        /// </summary>
        protected void EditCat_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            MySqlCommand comm;

            conn.Open();
            if (CatId.Text.Length == 0)
            {
                comm = new MySqlCommand("select max(id) as max_id from tblCategory", conn);
                int id = 1;
                MySqlDataReader result = null;
                try
                {
                    result = comm.ExecuteReader();
                    if (result.HasRows && result.Read())
                    {
                        id = Convert.ToInt32(result[0]) + 1;
                    }
                    conn.Close();
                    conn.Open();
                }
                catch (Exception ex)
                {
                    MySite.ShowAlert(this, "error happens:" + ex.ToString());
                    
                }

                result.Close();
                comm = new MySqlCommand("insert tblCategory values(" + id.ToString() + ",'" + Category.Text + "');", conn);
            }
            else
            {
                comm = new MySqlCommand("update tblCategory set Name='" + Category.Text + "' where(ID=" + CatId.Text + ");", conn);
            }
            try
            {
                comm.ExecuteReader();
            }
            catch (Exception ex)
            {
                MySite.ShowAlert(this, "Error happens:" + ex.Message);
            }
            conn.Close();
            BindRepeaterCategory();
        }
    }
}