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

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        BindRepeater();
    }

    private void BindRepeater()
    {
        Repeater rptFriends = FindControl("rptFriends") as Repeater;
        string constr = ConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;
        MySqlConnection _con = new MySqlConnection(constr);
        MySqlDataAdapter da = new MySqlDataAdapter("Select * From tblFriends", _con);
        DataSet ds = new DataSet();
        da.Fill(ds);
        rptFriends.DataSource = ds.Tables[0];
        rptFriends.DataBind();
        
    }

}