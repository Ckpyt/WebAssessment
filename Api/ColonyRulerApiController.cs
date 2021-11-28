using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace WebAssessment
{
    /// <summary>
    /// The game API: saving saves and settings
    /// </summary>
    public class ColonyRulerApiController : ApiController
    {
        public class LanguagesList
        {
            public List<string> m_languages = new List<string>();
        }

        /// <summary> Connection to database string </summary>
        public static readonly string ConnString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ModalConnectionString"].ConnectionString;

        /// <summary> current salt </summary>
        private static int m_hashSalt = 0;


        string GetHashSalt()
        {
            DateTime tm = DateTime.Now;
            Random rnd = new Random((int)tm.Ticks + 1154);
            m_hashSalt = rnd.Next();
            return m_hashSalt.ToString();
        }

        /// <summary>
        /// Get user application settings
        /// </summary>
        /// <param name="name"> user name </param>
        /// <returns> settings in json </returns>
        string GetSettings(string name)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);

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

        /// <summary>
        /// Get names of all user's saves
        /// </summary>
        /// <param name="name"> user name </param>
        /// <returns> names, divided by comma </returns>
        string GetSaveNames(string name)
        {
            var conn = new MySqlConnection(ConnString);
            var saveNames = "";
            conn.Open();
            var comm = new MySqlCommand("select SaveName from tblSaves where(UserName=@name)", conn);
            comm.Parameters.Add(new MySqlParameter("@name", name));

            try
            {
                MySqlDataReader result = comm.ExecuteReader();
                if (result.HasRows)
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
                Console.WriteLine("ColonyRulerApi: GetSaveNames error:" + ex.Message);
            }
            conn.Close();
            return "";
        }

        /// <summary>
        /// Get save from user name and save name
        /// </summary>
        /// <param name="name"> user name </param>
        /// <param name="saveName"> requested save's name </param>
        /// <returns> save in json </returns>
        string GetSave(string name, string saveName)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);

            conn.Open();
            MySqlCommand comm = new MySqlCommand("select SaveData from tblSaves where(UserName=@name and SaveName=@savename)", conn);
            comm.Parameters.Add(new MySqlParameter("@name", name));
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

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        /// <summary>
        /// Get save from server
        /// </summary>
        /// <param name="name">user name</param>
        /// <param name="save">save name</param>
        /// <returns></returns>
        public string Get(string name, string save)
        {
            return GetSave(name, save);
        }

        /// <summary>
        /// Get string from server
        /// </summary>
        /// <param name="id">request id. Could 1 or 2</param>
        /// <param name="name">user name</param>
        /// <returns></returns>
        public string Get(int id, string name)
        {
            //var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            switch (id)
            {
                case 1: return GetSettings(name);
                case 2: return GetSaveNames(name);
                case 3: return GetHashSalt();
                case 4:
                case 5:
                case 6: return GetLocalization(name, id - 3);
                case 7: return GetLocalizationNames();
            }

            return ("invalid request");
        }

        /// <summary>
        /// Post request. Not used.
        /// </summary>
        /// <param name="value"></param>
        public void Post([FromBody]string value)
        {
            
        }

        /// <summary>
        /// Save settings to the database
        /// </summary>
        /// <param name="name">username</param>
        /// <param name="value">settings in json string</param>
        private void SaveSettings(string name, string value)
        {
            string settings = GetSettings(name);

            MySqlConnection conn = new MySqlConnection(ConnString);
            MySqlCommand comm;

            conn.Open();
            comm = !string.IsNullOrEmpty(settings) ?
                new MySqlCommand("update tblSettings set settingsJson=@value where(Name=@name)", conn) :
                new MySqlCommand("insert into tblSettings values(@name, @value)", conn);
            comm.Parameters.Add(new MySqlParameter("@name", name));
            comm.Parameters.Add(new MySqlParameter("@value", value));
            
            try
            {
                comm.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ColonyRulerApi: SaveSettings error:" + ex.Message);
            }
            conn.Close();
        }

        /// <summary>
        /// delete save from database
        /// </summary>
        /// <param name="name">username</param>
        /// <param name="value">savename</param>
        void DeleteSave(string name, string value)
        {
            var conn = new MySqlConnection(ConnString);

            conn.Open();
            var comm = new MySqlCommand("delete from tblSaves where(Name=@name, SaveName = @value)", conn);
            comm.Parameters.Add(new MySqlParameter("@name", name));
            comm.Parameters.Add(new MySqlParameter("@value", value));

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

        [HttpPost]
        public string PlainStringBody([FromBody] string content)
        {
            return content;
        }

        /// <summary>
        /// save savegame to database
        /// </summary>
        /// <param name="save"> save name </param>
        /// <param name="name"> user name </param>
        public async void Post(string save, string name)
        {
            //var httpContext = (HttpContextWrapper)Request.Properties["MS_HttpContext"];

            // Read the form data and return an async task

            try
            {
                string converted = "";
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
                if (!string.IsNullOrEmpty(curSave))
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

        public async Task<string> ReadData()
        {
            string converted = "";
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
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }

            return converted;
        }

        /// <summary>
        /// Save settings or delete save
        /// </summary>
        /// <param name="id">request id</param>
        /// <param name="name">username</param>
        public async void Post(int id, string name)
        {
            //var httpContext = (HttpContextWrapper)Request.Properties["MS_HttpContext"];
            // Read the form data and return an async task.
            string converted = await ReadData();

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

        string GetLocalizationNames()
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
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

        string GetLocalization(string name, int id = 1)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            string local = "";

            conn.Open();
            MySqlCommand comm;
            switch (id)
            {
                case 2:
                    comm = new MySqlCommand("select Items from tblLocalization where(FullName=@name)", conn);
                    break;
                case 3:
                    comm = new MySqlCommand("select History from tblLocalization where(FullName=@name)", conn);
                    break;
                case 4:
                    comm = new MySqlCommand("select UILocalization from tblLocalization where(Name=@name)", conn);
                    break;
                default:
                    comm = new MySqlCommand("select UILocalization from tblLocalization where(FullName=@name)", conn);
                    break;
            }
            comm.Parameters.Add(new MySqlParameter("@name", name));


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

        public async Task<HttpResponseMessage> Post(int id, string name, int hash)
        {
            HttpResponseMessage mess = new HttpResponseMessage();

            
            int Hash = -1406264422;
            int passHash = Hash ^ m_hashSalt;
            if (passHash != hash)
            {
                mess.StatusCode = HttpStatusCode.Forbidden;
                mess.ReasonPhrase = "incorrect password";
                return mess;
            }

            var value = await Request.Content.ReadAsStringAsync();


            var loc = GetLocalization(name, 4);

            var conn = new MySqlConnection(ConnString);

            conn.Open();

            MySqlCommand comm;
            if (string.IsNullOrEmpty(loc))
            {
                comm = new MySqlCommand("insert into tblLocalization values(@name, @value, '', '')", conn);
            }
            else
            {
                switch (id)
                {
                    case 1:
                        comm = new MySqlCommand("update tblLocalization set UILocalization=@value where(Name=@name)", conn);
                        break;
                    case 2:
                        comm = new MySqlCommand("update tblLocalization set Items=@value where(Name=@name)", conn);
                        break;
                    case 3:
                        comm = new MySqlCommand("update tblLocalization set History=@value where(Name=@name)", conn);
                        break;
                    default:
                        mess.StatusCode = HttpStatusCode.Accepted;
                        return mess;
                }
            }

            comm.Parameters.Add(new MySqlParameter("@name", name));
            comm.Parameters.Add(new MySqlParameter("@value", value));
            MySqlDataReader result;
            try
            {
                result = comm.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ColonyRulerApi: SaveSettings error:" + ex.Message);
            }
            conn.Close();
            mess.StatusCode = HttpStatusCode.OK;
            return mess;
        }

        // PUT api/<controller>/5
        // not used
        public void Put(int id, [FromBody]string value)
        {
            
        }

        // DELETE api/<controller>/5
        // not used
        public void Delete(int id)
        {
        }
    }
}