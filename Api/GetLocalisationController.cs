using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAssessment.Api
{
    public class GetLocalisationController : ApiController
    {
        public string Get(int id=1, string language="English")
        {
            MySqlConnection conn = new MySqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString);
            string local = "";

            conn.Open();
            MySqlCommand comm;
            switch (id)
            {
                default:
                case 1:
                    comm = new MySqlCommand("select UILocalization from tblLocalization where(FullName=@name)", conn);
                    break;
                case 2:
                    comm = new MySqlCommand("select Items from tblLocalization where(FullName=@name)", conn);
                    break;
                case 3:
                    comm = new MySqlCommand("select History from tblLocalization where(FullName=@name)", conn);
                    break;
            }
            comm.Parameters.Add(new MySqlParameter("@name", language));


            try
            {
                MySqlDataReader result = comm.ExecuteReader();
                if (result.HasRows && result.Read())
                {
                    var scs = result[0];
                    local = Convert.ToString(scs);
                    conn.Close();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ColonyRulerApi: GetLocalization error:" + ex.Message);
            }
            conn.Close();
            return local;
        }
    }
}
