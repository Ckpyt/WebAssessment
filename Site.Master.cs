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
using System.Net.Mail;

namespace WebAssessment
{

    public partial class MySite : System.Web.UI.MasterPage
    {
        private string ConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;
        private SqlConnection conn;
        private SqlCommand comm;
        private Int64 id = 0;
        private bool IsAutorised = false;
        private string Name = "";

        private static MySite instance = null;
        public static MySite Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MySite();
                }
                return instance;
            }
        }

        public MySite()
        {
            instance = this;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Register.OnClientClick = "return RegisterBtnClc()";
            LoginBtn.OnClientClick = "return LoginBtnClc()";
            SignBtn.OnClientClick = "return DisplayModal()";
            RestoreBtn.OnClientClick = "return LoginBtnClc()";

            if (!IsPostBack)
            {
                Logout.Visible = Context.User.Identity.IsAuthenticated;
                if (Context.User.Identity.IsAuthenticated)
                {
                    SignBtn.Visible = false;
                    Logout.Visible = true;
                    Logout.Text = "Sign out, " + Context.User.Identity.Name;
                    Profilelnk.Visible = true;
                }
                else
                {
                    SignBtn.Visible = true;
                    Logout.Visible = false;
                    Profilelnk.Visible = false;
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
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var checkName = manager.FindByName(Login.Text);
            var checkEmail = manager.FindByEmail(Email.Text);
            if(checkName != null || checkEmail != null)
            {
                ShowAlert(this, "Name and e-mail should be unique");
                return false;
            }

            var user = new IdentityUser() { UserName = Login.Text, Email = Email.Text };
            IdentityResult result1 = manager.Create(user, Password.Text);
            if (result1.Succeeded)
            {
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                var userIdentity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                //Response.Redirect("~/Login.aspx");
                manager.AddToRole(user.Id, "User");

                UserName.Text = Login.Text;
                UserPassword.Text = Password.Text;
                SendEmail(user, "Hello, " + user.UserName + "<br>Congratilation!<br> You was registred on my site!<br> I'll never send you any junk mails, only security confirmations.<br><br>Cheers, Dmitriy Shabalin",
                    "Registration complite");
                LoginBtn_Click(null, null);

                //ShowAlert(this, "user " + Login.Text + " successful register");
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

        public void SendEmail(IdentityUser user, string message, string subject)
        {

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

            mail.To.Add(user.Email); //What address you send your email
            mail.From = new MailAddress("ckpyt.site@gmail.com", "Ckpyt's site", System.Text.Encoding.UTF8);
            mail.Subject = subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = message;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("ckpyt.site@gmail.com", "cfvjktnbrb_987"); //Your initial email and password which you use as a Credential
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
                ShowAlert(this, "Error happens:" + errorMessage);
            }
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

        protected bool SignIn(object sender, EventArgs e)
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

                var ReturnParam = Request.Params.Get("ReturnUrl");
                if (ReturnParam != null)
                    Response.Redirect(ReturnParam);
                return true;
                //Response.Redirect("~/Login.aspx");
            }
            else
            {

                invMsgUserNM.Attributes.Remove("Class");
                invMsgUserNM.Attributes.Add("Class", "alert alert-danger");
                invMsgUserNM.Text = "Invalid username or password.";
                Page.ClientScript.RegisterStartupScript(GetType(), "MyKey", "DisplayModal();", true);

                //ShowAlert(this, "Invalid username or password.");
                //StatusText.Text = "Invalid username or password.";
                //LoginStatus.Visible = true;
                return false;
            }
        }

        public void SignOut(object sender, EventArgs e)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            Response.Redirect("~/index.aspx");

        }

        public void Restore_Click(object sender, EventArgs e)
        {
            if (UserName.Text.Length == 0)
                ShowAlert(this, "Login should contain at least 1 symbol");

            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.FindByName(UserName.Text);
            if (user == null)
            {
                invMsgUserNM.Attributes.Remove("Class");
                invMsgUserNM.Attributes.Add("Class", "alert alert-danger");
                invMsgUserNM.Text = "Unfortunatelly, I cannot find this user";
                Page.ClientScript.RegisterStartupScript( GetType(), "MyKey", "DisplayModal();", true);
                return;
            }


            Random rnd = new Random();
            int randomeKode = rnd.Next(100000, 999999);

            var pg = Page.Master as MySite;
            pg.SendEmail(user, "Hello, " + user.UserName + "<br>Somebody want to change your password on my web-site <br>If it was your action, please, follow the link and type this code:" + randomeKode.ToString() + ". <br> <a href=\"http://localhost:62817/passwordChangeConfirm.aspx \"> Confirm page </a><br>Your new password will be the same as it was typed in the password field  <br> Cheers, Dmitriy Shabalin",
                "Password changing request");

            SqlConnection conn = new SqlConnection(ConnString);
            conn.Open();
            SqlCommand comm = new SqlCommand("insert into tblPassConfirm values('" + user.Id +
                "'," + randomeKode.ToString() + ",'" + DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss") +
                "','" + UserPassword.Text +"','1', 'true' );", conn);
                
            try
            {
                comm.ExecuteReader();
            }
            catch (Exception ex)
            {
                MySite.ShowAlert(this, "Error happens:" + ex.Message);
            }

            MySite.ShowAlert(this, "Password notification was send into your email.\n Please, follow instructions.");
        }


        protected void LoginBtn_Click(object sender, EventArgs e)
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);

            if(SignIn(sender, e))
                Response.Redirect("~/profile.aspx");
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

        protected void Profilelnk_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/profile.aspx");
        }
    }
}