﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAssessment
{
    public partial class AdsEditor : System.Web.UI.Page
    {
        private static string ConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillAdsCategoryList();
                FillAllAds();
            }
            
        }

        void FillAllAds()
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            IdentityUser user = userManager.FindByName(this.User.Identity.Name);

            SqlConnection conn = new SqlConnection(ConnString);
            SqlDataAdapter da = new SqlDataAdapter("select id, AdsName from tblAdverts where(userId='" 
                + user.Id +"')", conn);

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

            }
        }

        protected void EditAds_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string id = btn.CommandArgument;
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            IdentityUser user = userManager.FindByName(this.User.Identity.Name);

            SqlConnection conn = new SqlConnection(ConnString);
            SqlCommand comm = new SqlCommand("select * from tblAdverts where(id=" +
               id + " and UserID='" + user.Id + "')", conn);

            conn.Open();
            try
            {
                var reader = comm.ExecuteReader();
                if(reader.HasRows && reader.Read())
                {
                    AdvId.Text = Convert.ToString(reader[0]);
                    AdsCategory.SelectedValue = Convert.ToString(reader[2]);
                    AdsName.Text = Convert.ToString(reader[3]);
                    AdsText.Text = Convert.ToString(reader[4]);
                }
            }
            catch (Exception ex)
            {
                MySite.ShowAlert(this, "Error:" + ex.Message);
            }
            EditAds.Text = "Save advert";
        }

        protected void DeleteAds_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string id = btn.CommandArgument;
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            IdentityUser user = userManager.FindByName(this.User.Identity.Name);

            SqlConnection conn = new SqlConnection(ConnString);
            SqlCommand comm = new SqlCommand("delete from tblAdverts where(id=" + 
               id + " and UserID='" + user.Id + "')" ,conn);

            conn.Open();
            try
            {
                comm.ExecuteReader();
            }catch(Exception ex)
            {
                MySite.ShowAlert(this, "Error:" + ex.Message);
            }
            FillAllAds();

        }
        protected void EditAdsBtn_Click(object sender, EventArgs e)
        {
            
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            IdentityUser user = userManager.FindByName(this.User.Identity.Name);

            SqlConnection conn = new SqlConnection(ConnString);
            SqlCommand comm;

            conn.Open();
            if (AdvId.Text.Length == 0)
            {
                comm = new SqlCommand("select max(id) as max_id from tblAdverts", conn);
                int id = 1;
                SqlDataReader result = null;
                try
                {
                    result = comm.ExecuteReader();
                    if (result.HasRows && result.Read())
                    {
                        id = Convert.ToInt32(result[0]) + 1;
                    }
                    conn.Close();
                    conn.Open();
                }
                catch (Exception ex)
                {
                    MySite.ShowAlert(this, "error happens:" + ex.ToString());

                }

                result.Close();
                comm = new SqlCommand("insert tblAdverts values(" + id.ToString() + ",'" +
                    user.Id+ "'," + (AdsCategory.SelectedValue) + ",'"+ AdsName.Text + "','" + 
                    AdsText.Text + "', 'true' );", conn);
            }
            else
            {
                comm = new SqlCommand("update tblAdverts set AdsName='" + AdsName.Text +
                   "', CategoryID=" + (AdsCategory.SelectedValue) + ", AdsText='" + AdsText.Text +
                   "' where(ID=" + AdvId.Text + " and UserID='" + user.Id + "');", conn);
            }
            try
            {
                comm.ExecuteReader();
            }
            catch (Exception ex)
            {
                MySite.ShowAlert(this, "Error happens:" + ex.Message);
            }
            conn.Close();

            FillAllAds();
        }
    }
}