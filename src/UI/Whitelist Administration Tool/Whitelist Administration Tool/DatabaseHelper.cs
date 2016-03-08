using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using Newtonsoft.Json;
using Whitelist_Administration_Tool.Properties;

namespace Whitelist_Administration_Tool
{
    public class DatabaseHelper
    {
        internal static string Database = Settings.Default.DatabasePath;
        public const string AdminsTable = "adminsTable";
        public const string UsersTable = "usersTable";
        public const string ApplicationsTable = "applicationsTable";

        internal static bool IsLoginValid(string user, string password)
        {
            using (SQLiteConnection dbConn = OpenDatabase())
            {
                if (dbConn == null)
                {
                    return false;
                }

                string sql = String.Format("SELECT * FROM {0} WHERE username = '{1}'", AdminsTable, user);
                using (SQLiteCommand command = new SQLiteCommand(sql, dbConn))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["username"].ToString() == user && reader["password"].ToString() == password)
                            {
                                // Valid login
                                return true;
                            }
                        }

                        // Invalid login
                        return false;
                    }
                }
            }
        }

        protected internal static bool IsUidExists(string uniqueKey, string table)
        {
            using (SQLiteConnection dbConn = OpenDatabase())
            {
                if (uniqueKey == null || dbConn == null)
                {
                    return false;
                }

                string sql = string.Format("SELECT 1 FROM {0} WHERE uid = '{1}'",
                    table ,uniqueKey);

                using (SQLiteCommand command = new SQLiteCommand(sql, dbConn))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // The table exists
                            return true;
                        }

                        // The table does not exist
                        return false;
                    }
                }
            }
        }

        protected internal static bool IsTableExists(string tableName)
        {
            using (SQLiteConnection dbConn = OpenDatabase())
            {
                if (tableName == null || dbConn == null)
                {
                    return false;
                }

                string sql = string.Format("SELECT 1 FROM sqlite_master WHERE type = 'table' AND name = '{0}'",
                    tableName);

                using (SQLiteCommand command = new SQLiteCommand(sql, dbConn))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // The table exists
                            return true;
                        }

                        // The table does not exist
                        return false;
                    }
                }
            }
        }

        public static SQLiteConnection OpenDatabase()
        {
            SQLiteConnection sqLiteConnection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Database));
            sqLiteConnection.Open();
            return sqLiteConnection;
        }

        public static void ExectuteNonQuery(string sql)
        {
            using (SQLiteConnection c = OpenDatabase())
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Get the a sql data reader to read the whitelist out of the database.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="orderBy">Optional orderBy column from dataset.  'name' is default</param>
        /// <returns></returns>
        public static SQLiteDataReader GetTableReader(SQLiteConnection dbConn, string table, string orderBy = "name")
        {
            SQLiteDataReader reader;
            
            if (dbConn == null)
            {
                return null;
            }

            string sql = string.Format("SELECT * FROM {0} ORDER BY {1} COLLATE NOCASE ASC", table, orderBy);

                
            using (SQLiteCommand command = new SQLiteCommand(sql, dbConn))
            {
                reader = command.ExecuteReader();
            }

            return reader;
        }

        public static string FetchUuidFishbans(string username)
        {
            string uuid = null;

            // Build the url with the username to the api
            string url = string.Format("http://api.fishbans.com/bans/{0}", username);

            // Create the web request.
            var request = WebRequest.Create(url);
            request.Timeout = 5000;

            // Get the response
            var response = (HttpWebResponse)request.GetResponse();

            // Read out the json
            string json;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                json = sr.ReadToEnd();
            }

            // Get our data from the json if it is not null
            if (!string.IsNullOrEmpty(json))
            {
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                if (jsonObj != null)
                {
                    // If the api reports a successful request
                    if ((bool)jsonObj.success)
                    {
                        // then get the goods
                        uuid = jsonObj.bans.uuid;

                        // Since the uuid doesn't come properly formatted as a guid...
                        Guid guid = new Guid(uuid);
                        uuid = guid.ToString();
                    }
                }
            }

            return uuid;
        }

        /// <summary>
        /// Insert a new row into the UserTable.
        /// </summary>
        /// <param name="name">The Screen Name of the user.</param>
        /// <param name="uuid">The UUID. (GUID)</param>
        /// <param name="banned">Is the user banned?</param>
        /// <param name="email">The user's email address.</param>
        /// <param name="approvedBy">What admin approved this user?</param>
        public static void InsertUser(string name, string uuid, int banned, string email, string approvedBy)
        {
            if (name.Contains(";") || uuid.Contains(";") || email.Contains(";") || approvedBy.Contains(";"))
            {
                return;
            }

            string sql = string.Format("INSERT INTO {0} (name, uid, banned, email, approvedBy) values ('{1}', '{2}', {3}, '{4}', '{5}')", UsersTable, name, uuid, banned, email, approvedBy);
            ExectuteNonQuery(sql);
        }

        public static void InsertApplication(string userName, string uid, string email, int banned)
        {
            if (userName.Contains(";") || uid.Contains(";") || email.Contains(";"))
            {
                return;
            }

            banned = int.Parse(FetchBannedFishbans(userName));

            string sql = string.Format("INSERT INTO {0} (name, uid, email, banned) values ('{1}', '{2}', '{3}', {4})",
                        ApplicationsTable, userName, uid, email, banned);
            ExectuteNonQuery(sql);
        }

        public static string FetchBannedFishbans(string userName)
        {
            // Build the url with the username to the api
            string url = string.Format("http://api.fishbans.com/bans/{0}", userName);

            // Create the web request.
            var request = WebRequest.Create(url);

            // Get the response
            var response = (HttpWebResponse)request.GetResponse();

            // Read out the json
            string json;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                json = sr.ReadToEnd();
            }

            // Get our data from the json if it is not null
            if (!string.IsNullOrEmpty(json))
            {
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                if (!bool.Parse(jsonObj.success.ToString()))
                {
                    return "3";
                }

                dynamic services = jsonObj.bans.service;
                foreach (var obj in services)
                {
                    if (int.Parse(obj.Value.bans.ToString()) > 0)
                    {
                        return "1";
                    }
                }
                
            }

            return "0";
        }

        public static void CreateApplicationTable()
        {
            string sql = string.Format("CREATE TABLE if not exists {0} (name VARCHAR(40), uid VARCHAR(40) PRIMARY KEY, email VARCHAR(80), banned INT)", ApplicationsTable);
            ExectuteNonQuery(sql);
        }

        public static void CreateUsersTable()
        {
            string sql = string.Format("CREATE TABLE if not exists {0} (name VARCHAR(40), uid VARCHAR(40) PRIMARY KEY, banned INT, email VARCHAR(80), approvedBy VARCHAR(40))", UsersTable);
            ExectuteNonQuery(sql);
        }

        public static void CreateAdminsTable()
        {
            string sql = string.Format("CREATE TABLE if not exists {0} (username VARCHAR(40), password VARCHAR(40), powerlevel INT)", AdminsTable);
            ExectuteNonQuery(sql);
        }

        public static void InsertAdmin(string admin, string password, int powerlevel)
        {
            string sql = string.Format("INSERT INTO {0} (username, password, powerlevel) values ('{1}', '{2}', {3})", AdminsTable, admin, password, powerlevel);
            ExectuteNonQuery(sql);
        }

        public static bool IsUserNameExists(string newUser, string adminsTable)
        {
            using (SQLiteConnection dbConn = OpenDatabase())
            {
                if (newUser == null || dbConn == null)
                {
                    return false;
                }

                string sql = string.Format("SELECT 1 FROM {0} WHERE username = '{1}'",
                    adminsTable, newUser);

                using (SQLiteCommand command = new SQLiteCommand(sql, dbConn))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // The table exists
                            return true;
                        }

                        // The table does not exist
                        return false;
                    }
                }
            }
        }
    }
}