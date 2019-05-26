using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAssessment
{
    
    public partial class profile : System.Web.UI.Page
    {
        private string ConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;


        void FillTheTable()
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            
            var user = userManager.FindByName(this.User.Identity.Name);
            Login.Text = user.UserName;
            if(UserEmail.Text.Length == 0)
                UserEmail.Text = user.Email;

            

        }

        public void Page_Load(object sender, EventArgs e)
        {
            FillTheTable();
            if (User.IsInRole("Administrator"))
            {
                //Button1.Visible = true;
            }
            else
            {
                //Button1.Visible = false;
            }
            ChangeDetails.OnClientClick = "return ChangeDetailsBtnClc()";
            RemoveUser.OnClientClick = "return DeleteConfirm()";
        }

        public void DeleteAccount_Click(object sender, EventArgs e)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.FindByName(this.User.Identity.Name);
            userManager.Delete(user);
            var pg = Page.Master as MySite;
            pg.SignOut(sender, e);
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
                user.Email = UserEmail.Text;
                userManager.SetEmail(user.Id, UserEmail.Text);
            }

            if (Password.Text.Length > 0)
            {
                var result = userManager.ChangePassword(user.Id, Password.Text, newPassword.Text);
                if (!result.Succeeded)
                {
                    MySite.ShowAlert(this, "Error:" + result.Errors.ElementAt(0));
                }
                else
                {
                    MySite.ShowAlert(this, "Password changed");
                }
            }

            //Response.Redirect("~/profile.aspx");
        }
    }
}