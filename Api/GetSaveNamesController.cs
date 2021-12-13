using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web.Http.Description;

namespace WebAssessment.Api
{
    public class GetSaveNamesController : AbstractController
    {

        [ResponseType(typeof(string))]
        public HttpResponseMessage Get(string login, int sessionID)
        {
            if (CheckSessionId(login, sessionID) == false)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Error: wrong session id");
            }
            var conn = new MySqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString);
            conn.Open();
            var comm = new MySqlCommand("select SaveName from tblSaves where(UserName=@name)", conn);
            comm.Parameters.Add(new MySqlParameter("@name", login));
            List<string> answ = new List<string>();
            try
            {
                MySqlDataReader result = comm.ExecuteReader();
                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        var scs = result[0];
                        answ.Add(Convert.ToString(scs));

                    }
                    conn.Close();
                    return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(answ));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Login not found");
                }


            }
            catch (Exception ex)
            {
                conn.Close();
                Console.WriteLine("ColonyRulerApi: GetSaveNames error:" + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
