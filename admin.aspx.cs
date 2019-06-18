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
        protected void Page_Load(object sender, EventArgs e)
        {
            BindRepeater();
        }

        private void BindRepeater()
        {

            string constr = ConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;
            SqlConnection _con = new SqlConnection(constr);
            SqlDataAdapter da = new SqlDataAdapter("Select AspNetUsers.Id, AspNetUsers.UserName, AspNetUsers.Email, tblDescription.Description From AspNetUsers, tblDescription where(tblDescription.Id=AspNetUsers.Id);", _con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            rptUsers.DataSource = ds.Tables[0];
            rptUsers.DataBind();

        }

        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String commArg = e.CommandArgument.ToString();
        }

        public void ConfirmButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string id = btn.CommandArgument;
            profile.DeleteAccount(id, this);
            Response.Redirect("~/admin.aspx");
        }
    }
}