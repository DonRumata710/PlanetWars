using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace IdentityServer
{
    public class DatabaseService
    {
        public DatabaseService(string login, string password, string server = "localhost", int port = 3306)
        {
            connection = new MySqlConnection("server=" + server + ";user=" + login + ";port=" + port + ";database=planetwars;password=" + password + ";");
        }

        ~DatabaseService()
        {
            Close();
        }

        public void Open()
        {
            if (!isOpen)
            {
                connection.Open();
                isOpen = true;
            }
        }

        public void Close()
        {
            if (isOpen)
            {
                connection.Close();
                isOpen = false;
            }
        }


        public bool AddUser(User user)
        {
            user.userId = AddUser(name: user.name, email: user.email);
            return user.userId > 0;
        }

        public ulong AddUser(string name, string email)
        {
            ulong ID = 0;
            string sql = "INSERT INTO `user_list` (`username`, `email`) VALUES (@username, @email)";
            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.Add("username", MySqlDbType.String).Value = name;
                cmd.Parameters.Add("email", MySqlDbType.String).Value = email;
                if (cmd.ExecuteNonQuery() >= 0)
                {
                    sql = "SELECT LAST_INSERT_ID() AS ID";
                    cmd.CommandText = sql;
                    ulong.TryParse(cmd.ExecuteScalar().ToString(), out ID);
                }
            }
            return ID;
        }

        public bool ChangeUser(User user)
        {
            return ChangeUser(ID: user.userId, name: user.name, email: user.email, password: user.password);
        }

        public bool ChangeUser(ulong ID, string name, string email, string password)
        {
            bool result = false;
            if (ID > 0)
            {
                string sql = "UPDATE `user_list` SET `username`=@username, `email`=@email WHERE user_id=@user_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("user_id", MySqlDbType.UInt64).Value = ID;
                    cmd.Parameters.Add("username", MySqlDbType.String).Value = name;
                    cmd.Parameters.Add("email", MySqlDbType.String).Value = email;
                    cmd.Parameters.Add("password", MySqlDbType.String).Value = password;
                    result = cmd.ExecuteNonQuery() >= 0;
                }
            }
            return result;
        }

        public bool RemoveUser(User user)
        {
            return RemoveUser(user.userId);
        }

        public bool RemoveUser(ulong ID)
        {
            string sql = "DELETE FROM `user_list` WHERE `user_id`=@user_id";
            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.Add("user_id", MySqlDbType.UInt64).Value = ID;
                return cmd.ExecuteNonQuery() >= 0;
            }
        }

        public User FetchByID(int ID)
        {
            string strSQL = "SELECT * FROM `user_list` WHERE `user_id`=@user_id";
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                cmd.Parameters.Add("user_id", MySqlDbType.Int32).Value = ID;
                return FetchUser(cmd);
            }
        }

        public User FetchByUsername(string Name)
        {
            string strSQL = "SELECT * FROM `user_list` WHERE `username`=@username";
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                cmd.Parameters.Add("username", MySqlDbType.String).Value = Name;
                return FetchUser(cmd);
            }
            return null;
        }

        public User FetchUser(MySqlCommand cmd)
        {
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                    return ReadUserInfo(dr);
            }

            return null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Проверка запросов SQL на уязвимости безопасности")]
        public IList<User> List(string sortOrder, bool descending, int page, int pagesize, out int count)
        {
            List<User> users = new List<User>();
            string sort = " ORDER BY ";
            if (sortOrder != null && sortOrder != String.Empty)
            {
                sort += "`" + sortOrder + "`";
                if (descending) sort += " DESC";
                sort += ",";
            }
            sort += "`user_id`";
            string limit = "";
            if (pagesize > 0)
            {
                int start = (page - 1) * pagesize;
                limit = string.Concat(" LIMIT ", start.ToString(), ", ", pagesize.ToString());
            }
            string strSQL = "SELECT SQL_CALC_FOUND_ROWS * FROM `user_list` " + sort + limit;
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        users.Add(ReadUserInfo(dr));
                }
            }
            using (MySqlCommand cmdrows = new MySqlCommand("SELECT FOUND_ROWS()", connection))
            {
                int.TryParse(cmdrows.ExecuteScalar().ToString(), out count);
            }
            return users;
        }

        public User ReadUserInfo(MySqlDataReader dr)
        {
            return new User(
                dr.GetUInt64("user_id"),
                dr.GetString("username"),
                !dr.IsDBNull(dr.GetOrdinal("email")) ? dr.GetString("email") : "",
                dr.GetString("password"),
                !dr.IsDBNull(dr.GetOrdinal("create_time")) ? dr.GetDateTime("create_time") : null,
                dr.GetBoolean("active_user"));
        }

        public bool CompareCredentials(string password, string hash)
        {
            return hash == crypto.Security.ComputeHash(password, "0123456789ABCDEF");
        }

        public bool ValidateCredentials(string user, string password)
        {
            string strSQL = "SELECT `password` FROM `user_list` WHERE `username`=@username;";
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                cmd.Parameters.Add("username", MySqlDbType.String).Value = user;
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                        return CompareCredentials(password, dr.GetString("password"));
                }
            }
            return false;
        }

        private MySqlConnection connection;
        private bool isOpen = false;
    }
}
