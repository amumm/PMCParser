using MySql.Data.MySqlClient;
using System;

namespace ArticleAnalyzer.Data
{
    public class DBConnection
    {
        
        public string DatabaseName { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string ServerName { get; set; }

        public MySqlConnection Connection { get; set; }

        public DBConnection(String serverName, String databaseName, String userName, String password)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            UserName = userName;
            Password = password;
            Connect();
        }

        public bool Connect()
        {
            bool result = false;
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                    result = false;
                string connstring = string.Format("Server=" + ServerName + "; database={0}; UID=" + UserName + "; password=" + Password, DatabaseName);
                Connection = new MySqlConnection(connstring);
                Connection.Open();
                result = true;
            }

            return result;
        }

        public bool IsConnected()
        {
            if (Connection == null) return false;
            else return true;

        }

        public void Close()
        {
            Connection.Close();
        }
    }
}