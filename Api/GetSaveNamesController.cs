using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace WebAssessment.Api
{
    public class GetSaveNamesController : ApiController
    {
        public string Get(string login)
        {
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
                    return JsonConvert.SerializeObject(answ);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ColonyRulerApi: GetSaveNames error:" + ex.Message);
            }
            conn.Close();
            return "";
        }
    }
}
