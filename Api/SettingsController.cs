using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAssessment.Api
{
    public class SettingsController : ApiController
    {
        string connString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;

        /// <summary>
        /// Not safe method. Should be rewritten to session id
        /// </summary>
        public void Post(string login)
        {
            var data = Request.Content.ReadAsStringAsync();
            data.Wait();
            string answ = data.Result;
            string settings = GetSettings(login);

            MySqlConnection conn = new MySqlConnection(connString);
            MySqlCommand comm;

            conn.Open();
            comm = !string.IsNullOrEmpty(settings) ?
                new MySqlCommand("update tblSettings set settingsJson=@value where(Name=@name)", conn) :
                new MySqlCommand("insert into tblSettings values(@name, @value)", conn);
            comm.Parameters.Add(new MySqlParameter("@name", login));
            comm.Parameters.Add(new MySqlParameter("@value", answ));

            try
            {
                comm.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ColonyRulerApi: SaveSettings error:" + ex.Message);
            }
            conn.Close();

            Request.CreateResponse(HttpStatusCode.OK);
        }


        /// <summary>
        /// Get user application settings
        /// </summary>
        /// <param name="name"> user name </param>
        /// <returns> settings in json </returns>
        string GetSettings(string name)
        {
            MySqlConnection conn = new MySqlConnection(connString);

            conn.Open();
            MySqlCommand comm = new MySqlCommand("select settingsJson from tblSettings where(Name=@name)", conn);
            comm.Parameters.Add(new MySqlParameter("@name", name));

            try
            {
                MySqlDataReader result = comm.ExecuteReader();
                if (result.HasRows && result.Read())
                {
                    var scs = result[0];
                    var settings = Convert.ToString(scs);
                    conn.Close();
                    return settings;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ColonyRulerApi: GetSettings error:" + ex.Message);
            }
            conn.Close();
            return "";
        }

        public string Get(string login)
        {
            return GetSettings(login);
        }
    }
}
