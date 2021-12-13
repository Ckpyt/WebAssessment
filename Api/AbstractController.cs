using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAssessment.Api
{
    public abstract class AbstractController : ApiController
    {
        protected MySqlConnection connection = new MySqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString);

        protected bool CheckSessionId(string login, int id)
        {
            if (login == null || login.Length == 0) return false;

            connection.Open();
            MySqlCommand comm = new MySqlCommand("select * from tblUsers where(Name=@login and SessionID=@sessionID)", connection);
            comm.Parameters.Add(new MySqlParameter("@login", login));
            comm.Parameters.Add(new MySqlParameter("@sessionID", id));

            try
            {
                var answ = comm.ExecuteReader();
                bool ret = answ.HasRows;
                answ.Close();
                return ret;
            }catch(Exception ex)
            {
                Console.WriteLine("ColonyRulerApi: GetLocalization error:" + ex.Message);
            }
            return false;
        }
    }
}
