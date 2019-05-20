using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAssessment
{

    public partial class Site : System.Web.UI.MasterPage
    {
        private string ConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;
        private SqlConnection conn;
        private SqlCommand comm;
        private Int64 id = 0;
        private bool IsAutorised = false;
        private string Name = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Register.OnClientClick = "return RegisterBtnClc()";
            LoginBtn.OnClientClick = "return LoginBtnClc()";
            SignBtn.OnClientClick = "return DisplayModal()";

            if (!IsPostBack)
            {
                if (Context.User.Identity.IsAuthenticated)
                {
                    //StatusText.Text = string.Format("Hello {0}!!", User.Identity.GetUserName());
                    //LoginStatus.Visible = true;
                    //LogoutButton.Visible = true;
                }
                else
                {
                    //LoginForm.Visible = true;
                }
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
                comm = new SqlCommand("CREATE TABLE tblUsers ( [Id] INT NOT NULL, [Name] VARCHAR(50)  primary key NOT NULL, [Pass] VARCHAR(50) NOT NULL, [Email] VARCHAR(50) NOT NULL,  [Rights] INT NULL);insert into tblUsers values(1,'Ckpyt', 'ghjwtccjh','ckpyt@bk.ru', 255);", conn);
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

        protected bool RegisterUser()
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var user = new IdentityUser() { UserName = Login.Text, Email = Email.Text };
            IdentityResult result1 = manager.Create(user, Password.Text);
            if (result1.Succeeded)
            {
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                var userIdentity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                //Response.Redirect("~/Login.aspx");


                ShowAlert(this, "user " + Login.Text + " successful register");
            }
            else
            {
                string err = "Error happens:";
                foreach (var er in result1.Errors)
                    err += er + " ";
                ShowAlert(this, err);
            }
            return result1.Succeeded;
        }

        protected Boolean RegisterBtn(object sender, EventArgs e)
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
            return RegisterUser();
        }

        protected void SignIn(object sender, EventArgs e)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.Find(UserName.Text, UserPassword.Text);

            if (user != null)
            {
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                LoginNameRes.Value = UserName.Text;
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);
                SignBtn.Visible = false;
                Logout.Visible = true;
                Logout.Text = "Sign out, " + UserName.Text;
                //Response.Redirect("~/Login.aspx");
            }
            else
            {
                ShowAlert(this, "Invalid username or password.");
                //StatusText.Text = "Invalid username or password.";
                //LoginStatus.Visible = true;
            }
        }

        protected void SignOut(object sender, EventArgs e)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            //Response.Redirect("~/Login.aspx");

        }



        protected void LoginBtn_Click(object sender, EventArgs e)
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);

            SignIn(sender, e);

            /*
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
                        int rights = Convert.ToInt32(result[4]);
                        IsAutorised = true;
                        Name = UserName.Text;
                        Login.Text = Name;
                        Password.Text = DateTime.Now.ToString();
                        //ShowAlert(this, Name + " welcome on my site!");
                    }
                    else
                    {
                        Login.Text = "false";
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
            */
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            RegisterBtn(sender,e);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            SignOut(sender, e);
            Logout.Visible = false;
            SignBtn.Visible = true;
        }

        protected void sigin_Click(object sender, EventArgs e)
        {

        }
    }
}