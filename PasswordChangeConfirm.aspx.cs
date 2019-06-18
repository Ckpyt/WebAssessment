using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAssessment
{
    public partial class PasswordChangeConfirm : System.Web.UI.Page
    {
        private string ConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        void DeleteRequest(string code)
        {
            try
            {
                MySite.ShowAlert(this, "Error happens: nothing password requests was founded");
                SqlConnection conn2 = new SqlConnection(ConnString);
                conn2.Open();
                SqlCommand comm2 = new SqlCommand("delete from tblPassConfirm where(keyz=" + code.ToString() + ");", conn2);
                comm2.ExecuteReader();
                conn2.Close();
            }catch(Exception ex)
            {
                MySite.ShowAlert(this, "Error happens:" + ex.Message);
            }
        }


        public async void ConfirmButton_ClickAsync(object sender, EventArgs e)
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

            IdentityUser user = null;
            if(User.Identity.IsAuthenticated)
                user = userManager.FindByName(this.User.Identity.Name);
            string Password= "", newPassword= "";

            SqlConnection conn = new SqlConnection(ConnString);
            conn.Open();
            SqlCommand comm = new SqlCommand("select * from tblPassConfirm where keyz=" + ConfirmBox.Text + ";", conn);

            try
            {
                var readed = comm.ExecuteReader();

                if (readed.HasRows)
                    while (readed.Read())
                    {
                        string userID = Convert.ToString(readed[0]);
                        if (user != null && userID != user.Id)
                        {
                            MySite.ShowAlert(this, "Error happens: nothing password requests was founded");
                            return;
                        }
                        else
                        {
                            user = userManager.FindById(userID);
                            if (user == null)
                            {
                                MySite.ShowAlert(this, "Error happens: nothing password requests was founded");
                                return;
                            }
                        }
                        DateTime tm = Convert.ToDateTime(readed[2]);
                        if (tm.Day + 1 < DateTime.Now.Day)
                        {
                            DeleteRequest(ConfirmBox.Text);
                            return;
                        }
                        string pass = Convert.ToString(readed[3]);
                        string newPass = Convert.ToString(readed[4]);
                        bool IsItReset = Convert.ToBoolean(readed[5]);

                        pass = pass.Split(' ')[0];
                        newPass = newPass.Split(' ')[0];

                        if (IsItReset)
                        {

                            String hashedNewPassword = userManager.PasswordHasher.HashPassword(pass);
                            await userStore.SetPasswordHashAsync(user, hashedNewPassword);
                            await userStore.UpdateAsync(user);
                        }
                        else
                        {
                            var resultPass = userManager.ChangePassword(user.Id, pass, newPass);
                            if (!resultPass.Succeeded)
                            {
                                MySite.ShowAlert(this, "Error:" + resultPass.Errors.ElementAt(0));
                                return;
                            }
                        }



                        //MySite.ShowAlert(this, "Password changed");
                        var pg1 = Page.Master as MySite;
                        pg1.SendEmail(user, "Hello, " + user.UserName + "<br>Somebody had changed your password on my web-site <br>If it was not your action, please, register again. <br><br> Cheers, Dmitriy Shabalin",
                            "Password was changed");
                        MySite.ShowAlert(this,"Password was changed successful");
                        DeleteRequest(ConfirmBox.Text);
                        
                    }
                else
                {
                    MySite.ShowAlert(this, "Error happens: nothing password requests was founded");
                    return;
                }

            }
            catch (Exception ex)
            {
                MySite.ShowAlert(this, "Error happens:" + ex.Message);
            }

            var result = userManager.ChangePassword(user.Id, Password, newPassword);
            if (!result.Succeeded)
            {
                MySite.ShowAlert(this, "Error:" + result.Errors.ElementAt(0));
            }
            else
            {
                //MySite.ShowAlert(this, "Password changed");
                var pg1 = Page.Master as MySite;
                pg1.SendEmail(user, "Hello, " + user.UserName + "<br>Somebody had changed your password on my web-site <br>If it was not your action, please, register again. <br><br> Cheers, Dmitriy Shabalin",
                    "Password was changed");
            }
        }
    }
}