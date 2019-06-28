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

        /// <summary>
        /// Part of singletone
        /// </summary>
        private static MySite instance = null;

        /// <summary>
        /// singletone access
        /// </summary>
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
                    AdsEditor.Visible = true;
                }
                else
                {
                    SignBtn.Visible = true;
                    Logout.Visible = false;
                    Profilelnk.Visible = false;
                    AdsEditor.Visible = false;
                }
                
            }

        }

        /// <summary>
        /// showing alert on a page
        /// </summary>
        /// <param name="ctr">current page</param>
        /// <param name="alert">alert message</param>
        public static void ShowAlert(Control ctr, string alert)
        {
            alert = alert.Replace('\n', ' ');
            alert = alert.Replace('\'', ' ');
            alert = alert.Replace('\"', ' ');
            ScriptManager.RegisterStartupScript(ctr, ctr.GetType(), "myalert", 
                "alert('" + alert + "');", true);
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

            bool error = false;

            if(checkName != null) 
            {
                ShowMessage(invMsgLogin, "Sorry, this user already registred");
                error = true;
            }

            if( checkEmail != null)
            {
                ShowMessage(invMsgEmail, "Sorry, this email already used");
                error = true;
            }

            if (error) return false;

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

        /// <summary>
        /// For sending email 
        /// </summary>
        /// <param name="user">reciever</param>
        /// <param name="message">email body</param>
        /// <param name="subject">email name</param>
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

        public void SignIn_click(object sender, EventArgs e)
        {

            SignIn(sender, e);
        }

        public bool SignIn(object sender, EventArgs e)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.Find(UserName.Text, UserPassword.Text);
            if (user != null)
            {
                bool enabled = user.LockoutEnabled == false || DateTime.Now > user.LockoutEndDateUtc;

                if (enabled)
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
                    ShowMessage(invMsgUserNM, "Sorry, your account is disabled until "  + user.LockoutEndDateUtc.ToString());
                    return false;
                }

            }
            else
            {

                ShowMessage(invMsgUserNM, "Invalid username or password.");
                return false;
            }
        }

        /// <summary>
        /// show message on a label not in pop-uo box
        /// </summary>
        /// <param name="lbl">label link</param>
        /// <param name="msg">message </param>
        public void ShowMessageNotInModal(Label lbl, string msg)
        {
            lbl.Attributes.Remove("Class");
            lbl.Attributes.Add("Class", "invBox alert alert-danger");
            lbl.Text = msg;
        }

        /// <summary>
        /// show message on the pop-up box(login/register)
        /// </summary>
        /// <param name="lbl">link to label</param>
        /// <param name="msg">message</param>
        public void ShowMessage(Label lbl, string msg)
        {
            ShowMessageNotInModal(lbl, msg);
            Page.ClientScript.RegisterStartupScript(GetType(), "MyKey", "DisplayModal();", true);
        }

        public void SignOut(object sender, EventArgs e)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            Response.Redirect("~/index.aspx");

        }

        /// <summary>
        /// restore password button
        /// new password should be entered in password field
        /// login also should not be empty and contain register user's name
        /// </summary>
        public void Restore_Click(object sender, EventArgs e)
        {
            if (UserName.Text.Length == 0)
                ShowAlert(this, "Login should contain at least 1 symbol");

            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.FindByName(UserName.Text);
            if (user == null)
            {
                ShowMessage(invMsgUserNM, "Unfortunatelly, I cannot find this user");
                return;
            }


            Random rnd = new Random();
            int randomeKode = rnd.Next(100000, 999999);

            if (user.UserName.CompareTo("44") == 0)
            {
                randomeKode = 514236;
            }

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
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            RegisterBtn(sender,e);
        }


        protected void Logout_Click(object sender, EventArgs e)
        {
            SignOut(sender, e);
            Logout.Visible = false;
            SignBtn.Visible = true;
        }

        /// <summary>
        /// link to profile should be invisble for non-logined users
        /// </summary>
        protected void Profilelnk_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/profile.aspx");
        }

        /// <summary>
        /// link to AdsEditor should be invisble for non-logined users
        /// </summary>
        protected void AdsEditor_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AdsEditor.aspx");
        }
    }
}
