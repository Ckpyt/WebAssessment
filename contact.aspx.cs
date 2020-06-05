using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.MySqlClient;
using System;

namespace WebAssessment
{
    public partial class contact : System.Web.UI.Page
    {
        private string ConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<IdentityUser>();
                var userManager = new UserManager<IdentityUser>(userStore);
                var user = userManager.FindByName(this.User.Identity.Name);

                name.Text = user.UserName;
                email.Text = user.Email;

            }
        }

        protected void SendMessage(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            MySqlCommand comm = new MySqlCommand("insert into tblMessages (`UserName`, `email`, `subject`, `message`) values(@name, @email, @subject, @body);", conn);
            comm.Parameters.Add(new MySqlParameter("@name", name.Text));
            comm.Parameters.Add(new MySqlParameter("@email", email.Text));
            comm.Parameters.Add(new MySqlParameter("@subject", subject.Text));
            comm.Parameters.Add(new MySqlParameter("@body", message.Text));

            try
            {
                comm.ExecuteReader();
            }
            catch (Exception ex)
            {
                MySite.ShowAlert(this, "Error happens:" + ex.Message);
            }
        }
    }
}