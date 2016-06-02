using System;
using FaceRecognitionService.Models;
using Npgsql;
using System.Web.Configuration;

namespace FaceRecognitionService.Logic.SessionManager.Postgres
{
    public class RecognitionSessionManager : IRecognitionSessionManager
    {
        public RecognitionSession createSession()
        {
            var connection = createConnection();
            connection.Open();
            RecognitionSession session = new RecognitionSession();
            session.mirrorID = -1;
            int rows;
            session.sessionID = Guid.NewGuid().ToString();
            NpgsqlCommand command = new NpgsqlCommand(string.Format("INSERT INTO RecognitionSessions (session_id,mirror_id,state,recognized_user) VALUES ('{0}','{1}','{2}','{3}');",
            session.sessionID, session.mirrorID,(int)session.state,session.recognizedUser), connection);
            rows = command.ExecuteNonQuery();
            connection.Close();
            if (rows == 0) return null;
            return session;

        }

        public RecognitionSession getSession(string sessionID)
        {
            var connection = createConnection();
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(string.Format("SELECT session_id,mirror_id,state,recognized_user FROM RecognitionSessions WHERE session_id='{0}';",
            sessionID), connection);
            RecognitionSession session = null;
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                session = new RecognitionSession();
                session.sessionID = reader["session_id"].ToString();
                session.mirrorID = int.Parse(reader["mirror_id"].ToString());
                session.state = int.Parse(reader["state"].ToString())==0?RecognitionSession.RecognitionState.PRE_RECOGNITION:RecognitionSession.RecognitionState.POST_RECOGNITION;
                session.recognizedUser = int.Parse(reader["recognized_user"].ToString());
            }
            connection.Close();
            return session;
        }

        public int removeSession(string sessionID)
        {
            var connection = createConnection();
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(string.Format("DELETE FROM RecognitionSessions WHERE session_id='{0}';",
            sessionID), connection);
            int rows = command.ExecuteNonQuery();
            connection.Close();
            return rows;
        }

        public bool saveSession(RecognitionSession session)
        {
            var connection = createConnection();
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(string.Format("UPDATE RecognitionSessions SET session_id='{0}', mirror_id='{1}', state='{2}', recognized_user='{3}' WHERE session_id='{0}';",
            session.sessionID, session.mirrorID,(int)session.state,session.recognizedUser), connection);
            int rows = command.ExecuteNonQuery();
            connection.Close();
            if (rows != 0) return true;
            else return false;
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
            var connectionString = string.Format("Server={0};Database={1};User Id={2};Password={3};Port={4}" + (ssl?"; SSL Mode = Require; Use SSL Stream = true":""),
            host, db, user, pass, port);
            var con = new NpgsqlConnection(connectionString);
            if (ssl) con.UserCertificateValidationCallback = delegate { return true; };
            return con;
        }
    }
}