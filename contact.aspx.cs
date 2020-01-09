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
    public partial class contact : System.Web.UI.Page
    {
        private string ConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;
        private SqlConnection conn;
        private SqlCommand comm;

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
            SqlConnection conn = new SqlConnection(ConnString);
            conn.Open();
            SqlCommand comm = new SqlCommand("insert into tblMessages values(@name, @email, @subject, @body);", conn);
            comm.Parameters.Add(new SqlParameter("@name", name.Text));
            comm.Parameters.Add(new SqlParameter("@email", email.Text));
            comm.Parameters.Add(new SqlParameter("@subject", subject.Text));
            comm.Parameters.Add(new SqlParameter("@body", message.Text));

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