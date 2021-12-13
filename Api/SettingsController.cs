using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebAssessment.Api
{
    public class SettingsController : AbstractController
    {
        string connString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;

        [ResponseType(typeof(void))]
        public HttpResponseMessage Post(string login, int sessionID)
        {
            if (CheckSessionId(login, sessionID) == false)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Error: wrong session id");
            }

            var data = Request.Content.ReadAsStringAsync();
            data.Wait();
            string answ = data.Result;
            var settings = GetSettings(login);

            MySqlConnection conn = new MySqlConnection(connString);
            MySqlCommand comm;

            conn.Open();
            comm = settings.IsSuccessStatusCode ?
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
                conn.Close();
                Console.WriteLine("ColonyRulerApi: SaveSettings error:" + ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            conn.Close();
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        /// <summary>
        /// Get user application settings
        /// </summary>
        /// <param name="name"> user name </param>
        /// <returns> settings in json </returns>
        [ResponseType(typeof(string))]
        HttpResponseMessage GetSettings(string name)
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
                    return Request.CreateResponse(settings);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Login not found");
                }

            }
            catch (Exception ex)
            {
                conn.Close();
                Console.WriteLine("ColonyRulerApi: GetSettings error:" + ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [ResponseType(typeof(string))]
        public HttpResponseMessage Get(string login, int sessionID)
        {
            if (CheckSessionId(login, sessionID) == false)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Error: wrong session id");
            }
            return GetSettings(login);
        }
    }
}
