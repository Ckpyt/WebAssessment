using System;
using System.Web;

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