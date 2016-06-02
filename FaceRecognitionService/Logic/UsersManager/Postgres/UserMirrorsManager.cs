using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace FaceRecognitionService.Logic.UsersManager.Postgres
{
    public class UserMirrorsManager : IUserMirrorsManager
    {
        public void addUsersMirror(int userID, int mirrorID)
        {
            var connection = createConnection();
            connection.Open();
            int rows;
            NpgsqlCommand command = new NpgsqlCommand(string.Format("INSERT INTO UsersMirrors (user_id,mirror_id) VALUES ('{0}','{1}');",
            userID,mirrorID), connection);
            try
            {
                rows = command.ExecuteNonQuery();
            }
            catch(NpgsqlException)
            {
            }
            connection.Close();
        }

        public void deleteUsersMirrors(int userID)
        {
            var connection = createConnection();
            connection.Open();
            int rows;
            NpgsqlCommand command = new NpgsqlCommand(string.Format("DELETE FROM UsersMirrors WHERE user_id='{0}';",
            userID), connection);
            rows = command.ExecuteNonQuery();
            connection.Close();
        }

        public void deleteUsersMirror(int userID, int mirrorID)
        {
            var connection = createConnection();
            connection.Open();
            int rows;
            NpgsqlCommand command = new NpgsqlCommand(string.Format("DELETE FROM UsersMirrors WHERE user_id='{0}' AND mirror_id='{1}';",
            userID, mirrorID), connection);
            rows = command.ExecuteNonQuery();
            connection.Close();
        }

        public int[] getUsersMirrors(int userID)
        {
            var connection = createConnection();
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(string.Format("SELECT mirror_id FROM UsersMirrors WHERE user_id='{0}';",
            userID), connection);
            List<int> mirrors = new List<int>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                mirrors.Add(int.Parse(reader["mirror_id"].ToString()));
            }
            connection.Close();
            return mirrors.ToArray();
        }

        public NpgsqlConnection createConnection()
        {
            string host = WebConfigurationManager.AppSettings["DB_SERVER"] ?? "";
            string db = WebConfigurationManager.AppSettings["DB_NAME"] ?? "";
            string user = WebConfigurationManager.AppSettings["DB_USER"] ?? "";
            string pass = WebConfigurationManager.AppSettings["DB_PASS"] ?? "";
            string port = WebConfigurationManager.AppSettings["DB_PORT"] ?? "";
            bool ssl = false;
            bool.TryParse(WebConfigurationManager.AppSettings["DB_SSL"] ?? "", out ssl);
            var connectionString = string.Format("Server={0};Database={1};User Id={2};Password={3};Port={4}" + (ssl ? "; SSL Mode = Require; Use SSL Stream = true" : ""),
            host, db, user, pass, port);
            var con = new NpgsqlConnection(connectionString);
            if (ssl) con.UserCertificateValidationCallback = delegate { return true; };
            return con;
        }
    }
}