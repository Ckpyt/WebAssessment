using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.MySqlClient;
using System;
using System.Web.UI;

namespace WebAssessment
{

    public partial class profile : System.Web.UI.Page
    {
        private static string ConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;


        void FillTheTable()
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var m_currDescr = Description.Text;
            var user = userManager.FindByName(this.User.Identity.Name);
            Login.Text = user.UserName;
            //save e-mail
            if (UserEmail.Text.Length == 0)
                UserEmail.Text = user.Email;

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
                else
                {
                    conn.Close();
                    Description.Text = "It is your description";
                    conn.Open();
                    comm = new MySqlCommand("insert into tblDescription values('" + user.Id + "','" + Description.Text + "')", conn);
                    result = comm.ExecuteReader();
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                MySite.ShowAlert(this, "error happens:" + ex.ToString());
            }

            //save current description
            if (m_currDescr.Length > 0 && m_currDescr.CompareTo(Description.Text) != 0)
            {
                conn.Open();

                comm = new MySqlCommand("update tblDescription set Description='" + m_currDescr
                    + "' where(Id='" + user.Id.ToString() + "');", conn);

                try
                {
                    comm.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MySite.ShowAlert(this, "Error happens:" + ex.Message);
                }
                conn.Close();

                Description.Text = m_currDescr;
            }

        }

        public void Page_Load(object sender, EventArgs e)
        {
            FillTheTable();
            if (User.IsInRole("Administrator"))
            {
                AdminMode.Visible = true;
            }
            else
            {
                AdminMode.Visible = false;
            }
            ChangeDetails.OnClientClick = "return ChangeDetailsBtnClc()";
            RemoveUser.OnClientClick = "return DeleteConfirm()";
        }

        public static void DeleteAccount(string id, Control th)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.FindById(id);
            userManager.Delete(user);

            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            MySqlCommand comm = new MySqlCommand("delete from tblDescription where id='" + user.Id + "';", conn);
            try
            {
                comm.ExecuteReader();
            }
            catch (Exception ex)
            {
                MySite.ShowAlert(th, "Error happens:" + ex.Message);
            }

        }

        public void DeleteAccount_Click(object sender, EventArgs e)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.FindById(User.Identity.GetUserId());

            DeleteAccount(User.Identity.GetUserId(), this);

            var pg = Page.Master as MySite;
            pg.SendEmail(user, "Hello, " + user.UserName + "<br>Unfortunatelly, your account was deleted.<br>If it was not your action, please, register again. <br><br> Cheers, Dmitriy Shabalin",
                "Account was deleted");
            pg.SignOut(null, null);

        }

        public void Profilelnk_Click(object sender, EventArgs e)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var user = userManager.FindByName(this.User.Identity.Name);

            if (UserEmail.Text.Length > 5 && UserEmail.Text != user.Email)
            {
                var checkEmail = userManager.FindByEmail(UserEmail.Text);
                if (checkEmail != null && checkEmail.Id != user.Id)
                {
                    MySite mst = Page.Master as MySite;
                    mst.ShowMessageNotInModal(invMsgEmail2, "sorry, this email already used");
                }
                else
                {
                    user.Email = UserEmail.Text;
                    userManager.SetEmail(user.Id, UserEmail.Text);
                }
            }

            if (Password.Text.Length > 0)
            {
                Random rnd = new Random();
                int randomeKode = rnd.Next(100000, 999999);
                //just for testing
                if (user.UserName.CompareTo("44") == 0)
                {
                    randomeKode = 514236;
                }

                var pg = Page.Master as MySite;
                pg.SendEmail(user, "Hello, " + user.UserName + "<br>Somebody want to change your password on my web-site <br>If it was your action, please, follow the link and type this code:" + randomeKode.ToString() + ". <br> <a href=\"http://localhost:62817/passwordChangeConfirm.aspx \"> Confirm page </a><br> Cheers, Dmitriy Shabalin",
                    "Password changing request");

                MySqlConnection conn = new MySqlConnection(ConnString);
                conn.Open();
                MySqlCommand comm = new MySqlCommand("insert into tblPassConfirm values('" + user.Id +
                    "'," + randomeKode.ToString() + ",'" + DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss") +
                    "','" + Password.Text + "','" + newPassword.Text + "','false');", conn);

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
        }

        protected void AdminMode_Click(object sender, EventArgs e)
        {
            if (User.IsInRole("Administrator"))
            {
                Response.Redirect("~/admin.aspx");
            }
            else
            {
                MySite.ShowAlert(this, "Sorry, this page is not allowed for you");
            }
        }
    }
}
