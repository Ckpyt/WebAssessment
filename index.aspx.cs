using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        private static string ConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;

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
            if (!IsPostBack)
            {
                FillAdsCategoryList();
                BindAdsRepeater();
            }
        }
        void BindAdsRepeater(int id = 0)
        {

            SqlConnection conn = new SqlConnection(ConnString);
            SqlDataAdapter da = new SqlDataAdapter("select tblAdverts.AdsName, tblCategory.Name, tblAdverts.AdsText from tblAdverts, tblCategory" +
                " where(tblCategory.ID=tblAdverts.CategoryID" + (id > 0 ? " and tblCategory.ID=" + id: "") + ")" , conn);

            DataSet ds = new DataSet();
            da.Fill(ds);
            rptAds.DataSource = ds.Tables[0];
            rptAds.DataBind();
        }
        void FillAdsCategoryList()
        {

            DataTable subjects = new DataTable();

            using (SqlConnection con = new SqlConnection(ConnString))
            {

                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT ID, Name FROM tblCategory", con);
                    adapter.Fill(subjects);

                    AdsCategory.DataSource = subjects;
                    AdsCategory.DataTextField = "Name";
                    AdsCategory.DataValueField = "ID";
                    AdsCategory.DataBind();
                }
                catch (Exception ex)
                {
                    // Handle the error
                    MySite.ShowAlert(this, "Error happen:" + ex.Message);
                }
                AdsCategory.Items.Insert(0, new ListItem("All categories", "0"));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int id = AdsCategory.SelectedIndex;
            if (searchText.Text.Length == 0)
                BindAdsRepeater(id);
            else {
                string key = searchText.Text;
                SqlConnection conn = new SqlConnection(ConnString);
                SqlDataAdapter da = new SqlDataAdapter("select tblAdverts.AdsName, tblCategory.Name, tblAdverts.AdsText from tblAdverts, tblCategory" +
                    " where(tblCategory.ID=tblAdverts.CategoryID" + (id > 0 ? " and tblCategory.ID=" + id : "") + ")", conn);

                DataSet ds = new DataSet();
                da.Fill(ds);

                DataSet dssearch = new DataSet();
                var tbl = ds.Tables[0];
                for(int i =0; i < tbl.Rows.Count && tbl.Rows.Count > 0; i++)
                {
                    DataRow row = tbl.Rows[i];
                    string name = Convert.ToString(row.ItemArray[0]);
                    if (name.Contains(key))
                    {
                        //dssearch.Tables[0].ImportRow(row);
                    }
                    else
                    {
                        string text = Convert.ToString(row.ItemArray[2]);
                        if (text.Contains(key))
                        {
                            //dssearch.Tables[0].ImportRow(row);
                        }
                        else
                        {
                            tbl.Rows.Remove(row);
                            i--;
                        }
                    }
                }
                rptAds.DataSource = ds.Tables[0];
                rptAds.DataBind();
            }


        }
    }
}