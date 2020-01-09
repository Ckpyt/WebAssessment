using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace WebAssessment
{
    public class ColonyRulerApiController : ApiController
    {
        private static string ConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;

        string GetSettings(string name)
        {
            SqlConnection conn = new SqlConnection(ConnString);
            SqlCommand comm;

            conn.Open();
            comm = new SqlCommand("select settingsJson from tblSettings where(Name='" + name + "')", conn);

            SqlDataReader result = null;
            try
            {
                result = comm.ExecuteReader();
                if (result.HasRows && result.Read())
                {
                    var scs = result[0];
                    string settings = Convert.ToString(scs);
                    conn.Close();
                    return settings;
                }

            }catch(Exception ex)
            {

            }
            conn.Close();
            return "";
        }

        string GetSaveNames(string name)
        {
            SqlConnection conn = new SqlConnection(ConnString);
            SqlCommand comm;
            string saveNames = "";
            conn.Open();
            comm = new SqlCommand("select SaveName from tblSaves where(UserName='" + name + "')", conn);

            SqlDataReader result = null;
            try
            {
                result = comm.ExecuteReader();
                if (result.HasRows )
                {
                    while (result.Read())
                    {
                        var scs = result[0];
                        saveNames += Convert.ToString(scs) + ",";
                        
                    }
                    conn.Close();
                    return saveNames;
                }

            }
            catch (Exception ex)
            {

            }
            conn.Close();
            return "";
        }

        string GetSave(string name, string saveName)
        {
            SqlConnection conn = new SqlConnection(ConnString);
            SqlCommand comm;

            conn.Open();
            comm = new SqlCommand("select SaveData from tblSaves where(UserName='" + name + "' and SaveName='" + saveName + "')", conn);

            SqlDataReader result = null;
            try
            {
                result = comm.ExecuteReader();
                if (result.HasRows && result.Read())
                {
                    var scs = result[0];
                    string save = Convert.ToString(scs);
                    conn.Close();
                    return save;
                }

            }
            catch (Exception ex)
            {

            }
            conn.Close();
            return "";
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public string Get(string name, string save)
        {
            return GetSave(name, save);
        }

        // GET api/<controller>/5
        public string Get(int id, string name)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            switch(id)
            {
                case 1:
                    return ( GetSettings(name));
                case 2:
                    return (GetSaveNames(name));
            };

            return ("invalid request");
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
            string txt = value;
        }

        void SaveSettings(string name, string value)
        {
            string settings = GetSettings(name);

            SqlConnection conn = new SqlConnection(ConnString);
            SqlCommand comm;

            conn.Open();
            comm = settings != null && settings.Length > 0 ?
                new SqlCommand("update tblSettings set settingsJson='" + value + 
                "' where(Name='" + name + "')", conn):
                new SqlCommand("insert into tblSettings values('" + name + 
                "','" + value + "')" , conn);

            SqlDataReader result = null;
            try
            {
                result = comm.ExecuteReader();
            }
            catch (Exception ex)
            {

            }
            conn.Close();
        }

        void DeleteSave(string name, string value)
        {
            SqlConnection conn = new SqlConnection(ConnString);
            SqlCommand comm;

            conn.Open();
            comm = new SqlCommand("delete from tblSaves where(Name='" + name + "', SaveName = '" + value + "')", conn);

            SqlDataReader result = null;
            try
            {
                result = comm.ExecuteReader();
                if (result.HasRows && result.Read())
                {
                    var scs = result[0];
                    string settings = Convert.ToString(scs);
                    
                }

            }
            catch (Exception ex)
            {

            }
            conn.Close();
        }

        [HttpPost]
        public string PlainStringBody([FromBody] string content)
        {
            return content;
        }

        public async void Post(string save, string name)
        {
            var httpContext = (HttpContextWrapper)Request.Properties["MS_HttpContext"];

            string converted = " ";

            // Read the form data and return an async task

            try
            {
                // Read the form data.
                string req = await Request.Content.ReadAsStringAsync();
                string[] reqStrs = req.Split('\n');
                // This illustrates how to get the file names.
                foreach (string file in reqStrs)
                {
                    if (file.Length > 0 && file[0] == '{')
                        converted = file;
                }

                string curSave = GetSave(name, save);

                SqlConnection conn = new SqlConnection(ConnString);
                SqlCommand comm;

                conn.Open();
                if(curSave != null && curSave.Length > 0)
                {
                    comm = new SqlCommand("update tblSaves set SaveData=@save where(UserName=@name and SaveName=@saveName)", conn);
                    comm.Parameters.Add(new SqlParameter("@name", name));
                    comm.Parameters.Add(new SqlParameter("@saveName", save));
                    comm.Parameters.Add(new SqlParameter("@save", converted));
                }
                else
                {
                    comm = new SqlCommand("insert into tblSaves values(@saveName, @name, @save)", conn);

                    comm.Parameters.Add(new SqlParameter("@name", name));
                    comm.Parameters.Add(new SqlParameter("@saveName", save));
                    comm.Parameters.Add(new SqlParameter("@save", converted));
                }

                SqlDataReader result = comm.ExecuteReader();

            }
            catch (System.Exception e)
            {
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }


        }

        public async void Post(int id, string name)
        {
            var httpContext = (HttpContextWrapper)Request.Properties["MS_HttpContext"];

            string converted = " ";

            // Read the form data and return an async task.

            try
            {
                // Read the form data.
                string req = await Request.Content.ReadAsStringAsync();
                string[] reqStrs = req.Split('\n');
                // This illustrates how to get the file names.
                foreach (string file in reqStrs)
                {
                    if (file.Length > 0 && file[0] == '{')
                        converted = file;

                }

            }
            catch (System.Exception e)
            {
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }

            switch (id)
            {
                case 1:
                    SaveSettings(name, converted);
                    break;
                case 2:
                    DeleteSave(name, converted);
                    break;

            }
            Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}