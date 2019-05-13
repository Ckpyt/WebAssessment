using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAssessment
{
    public partial class index : System.Web.UI.Page
    {


        public static void ShowAlert(Control ctr, string alert)
        {
            alert = alert.Replace('\n', ' ');
            alert = alert.Replace('\'', ' ');
            alert = alert.Replace('\"', ' ');
            ScriptManager.RegisterStartupScript(ctr, ctr.GetType(), "myalert",
                "alert('" + alert + "');", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


    }
}