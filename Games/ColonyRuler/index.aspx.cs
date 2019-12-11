using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAssessment.Games.ColonyRuler
{
    public partial class index : System.Web.UI.Page
    {
        public string UserName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            if (authenticationManager.User.Identity.IsAuthenticated)
            {
                UserName = authenticationManager.User.Identity.Name;
            }
            else
            {
                UserName = "Not authorized";
            }
        }
    }
}