using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAssessment.Api
{
    public class SaveController : ApiController
    {
        string connString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;

        public string Get(string login, string saveName)
        {
            MySqlConnection conn = new MySqlConnection(connString);

            conn.Open();
            MySqlCommand comm = new MySqlCommand("select SaveData from tblSaves where(UserName=@name and SaveName=@savename)", conn);
            comm.Parameters.Add(new MySqlParameter("@name", login));
            comm.Parameters.Add(new MySqlParameter("@savename", saveName));

            try
            {
                MySqlDataReader result = comm.ExecuteReader();
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
                Console.WriteLine("ColonyRulerApi: GetSave error:" + ex.Message);
            }
            conn.Close();
            return "";
        }

        public void Post(string login, string saveName)
        {
            try
            {
                string converted = "";
                // Read the form data.
                var data = Request.Content.ReadAsStringAsync();
                data.Wait();
                string req = data.Result;
                string[] reqStrs = req.Split('\n');
                // This illustrates how to get the file names.
                foreach (string file in reqStrs)
                {
                    if (file.Length > 0 && file[0] == '{')
                        converted = file;
                }

                string curSave = Get(login, saveName);

                MySqlConnection conn = new MySqlConnection(connString);
                MySqlCommand comm;

                conn.Open();
                if (!string.IsNullOrEmpty(curSave))
                {
                    comm = new MySqlCommand("update tblSaves set SaveData=@save where(UserName=@name and SaveName=@saveName)", conn);
                    comm.Parameters.Add(new MySqlParameter("@name", login));
                    comm.Parameters.Add(new MySqlParameter("@saveName", saveName));
                    comm.Parameters.Add(new MySqlParameter("@save", converted));
                }
                else
                {
                    comm = new MySqlCommand("insert into tblSaves values(@saveName, @name, @save)", conn);

                    comm.Parameters.Add(new MySqlParameter("@name", login));
                    comm.Parameters.Add(new MySqlParameter("@saveName", saveName));
                    comm.Parameters.Add(new MySqlParameter("@save", converted));
                }

                MySqlDataReader result = comm.ExecuteReader();

            }
            catch (System.Exception e)
            {
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        public void Delete(string login, string saveName)
        {
            var conn = new MySqlConnection(connString);

            conn.Open();
            var comm = new MySqlCommand("delete from tblSaves where(UserName=@name and SaveName=@saveName)", conn);
            comm.Parameters.Add(new MySqlParameter("@name", login));
            comm.Parameters.Add(new MySqlParameter("@saveName", saveName));

            try
            {
                comm.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ColonyRulerApi: DeleteSave error:" + ex.Message);
            }
            conn.Close();
        }
    }
}
