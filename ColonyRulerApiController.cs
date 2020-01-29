using MySql.Data.MySqlClient;
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
    /// <summary>
    /// The game API: saving saves and settings
    /// </summary>
    public class ColonyRulerApiController : ApiController
    {
        /// <summary> Connection to database string </summary>
        private static string ConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;

        /// <summary>
        /// Get user application settings
        /// </summary>
        /// <param name="name"> user name </param>
        /// <returns> settings in json </returns>
        string GetSettings(string name)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            MySqlCommand comm;

            conn.Open();
            comm = new MySqlCommand("select settingsJson from tblSettings where(Name=@name)", conn);
            comm.Parameters.Add(new MySqlParameter("@name", name));

            MySqlDataReader result = null;
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

        /// <summary>
        /// Get names of all user's saves
        /// </summary>
        /// <param name="name"> user name </param>
        /// <returns> names, divided by comma </returns>
        string GetSaveNames(string name)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            MySqlCommand comm;
            string saveNames = "";
            conn.Open();
            comm = new MySqlCommand("select SaveName from tblSaves where(UserName=@name)", conn);
            comm.Parameters.Add(new MySqlParameter("@name", name));

            MySqlDataReader result = null;
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

        /// <summary>
        /// Get save from user name and save name
        /// </summary>
        /// <param name="name"> user name </param>
        /// <returns> save in json </returns>
        string GetSave(string name, string saveName)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            MySqlCommand comm;

            conn.Open();
            comm = new MySqlCommand("select SaveData from tblSaves where(UserName=@name and SaveName=@savename)", conn);
            comm.Parameters.Add(new MySqlParameter("@name", name));
            comm.Parameters.Add(new MySqlParameter("@savename", saveName));

            MySqlDataReader result = null;
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

            MySqlConnection conn = new MySqlConnection(ConnString);
            MySqlCommand comm;

            conn.Open();
            comm = settings != null && settings.Length > 0 ?
                new MySqlCommand("update tblSettings set settingsJson=@value where(Name=@name)", conn):
                new MySqlCommand("insert into tblSettings values(@nane, @value)" , conn);
            comm.Parameters.Add(new MySqlParameter("@name", name));
            comm.Parameters.Add(new MySqlParameter("@value", value));
            MySqlDataReader result = null;
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
            MySqlConnection conn = new MySqlConnection(ConnString);
            MySqlCommand comm;

            conn.Open();
            comm = new MySqlCommand("delete from tblSaves where(Name=@name, SaveName = @value)", conn);
            comm.Parameters.Add(new MySqlParameter("@name", name));
            comm.Parameters.Add(new MySqlParameter("@value", value));

            MySqlDataReader result = null;
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

                MySqlConnection conn = new MySqlConnection(ConnString);
                MySqlCommand comm;

                conn.Open();
                if(curSave != null && curSave.Length > 0)
                {
                    comm = new MySqlCommand("update tblSaves set SaveData=@save where(UserName=@name and SaveName=@saveName)", conn);
                    comm.Parameters.Add(new MySqlParameter("@name", name));
                    comm.Parameters.Add(new MySqlParameter("@saveName", save));
                    comm.Parameters.Add(new MySqlParameter("@save", converted));
                }
                else
                {
                    comm = new MySqlCommand("insert into tblSaves values(@saveName, @name, @save)", conn);

                    comm.Parameters.Add(new MySqlParameter("@name", name));
                    comm.Parameters.Add(new MySqlParameter("@saveName", save));
                    comm.Parameters.Add(new MySqlParameter("@save", converted));
                }

                MySqlDataReader result = comm.ExecuteReader();

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