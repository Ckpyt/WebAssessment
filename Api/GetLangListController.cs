using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAssessment.Api
{
    public class GetLangListController : ApiController
    {
        public class LanguagesList
        {
            public List<string> m_languages = new List<string>();
        }

        public string Get()
        {
            MySqlConnection conn = new MySqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString);
            string local = "";

            conn.Open();
            MySqlCommand comm = new MySqlCommand("select FullName from tblLocalization", conn);

            try
            {
                LanguagesList lang = new LanguagesList();

                MySqlDataReader result = comm.ExecuteReader();

                foreach (System.Data.Common.DbDataRecord res in result)
                {
                    lang.m_languages.Add(res.GetValue(0).ToString());
                }

                conn.Close();
                local = JsonConvert.SerializeObject(lang);
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
