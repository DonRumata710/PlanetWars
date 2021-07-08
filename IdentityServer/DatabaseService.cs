﻿using System;
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
            User user = null;
            string strSQL = "SELECT `user_id`, `username`, `password`, `email`, `create_time` FROM `user_list` WHERE `user_id`=@user_id";
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                ulong userID = 0;
                string username = null;
                string password = String.Empty;
                string email = String.Empty;
                DateTime? registerDate = null;
                cmd.Parameters.Add("user_id", MySqlDbType.Int32).Value = ID;
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        userID = dr.GetUInt64("user_id");
                        username = dr.GetString("username").ToString();
                        password = dr.GetString("password").ToString(); ;
                        if (!dr.IsDBNull(dr.GetOrdinal("email"))) email = dr.GetString("email").ToString();
                        if (!dr.IsDBNull(dr.GetOrdinal("create_time"))) registerDate = dr.GetDateTime("create_time");
                    }
                    if (userID > 0)
                        user = new User(userID, username, email, password, registerDate);
                }
            }
            return user;
        }

        public User FetchByUsername(string Name)
        {
            string strSQL = "SELECT `user_id`, `username`, `password`, `email`, `create_time` FROM `user_list` WHERE `username`=@username";
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                cmd.Parameters.Add("username", MySqlDbType.String).Value = Name;
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new User(
                            dr.GetUInt64("user_id"),
                            dr.GetString("username"),
                            dr.IsDBNull(dr.GetOrdinal("email")) ? dr.GetString("email") : "",
                            dr.GetString("password"),
                            dr.IsDBNull(dr.GetOrdinal("create_time")) ? dr.GetDateTime("create_time") : null);
                    }
                }
            }
            return null;
        }

        // Стандартная и очень привлекательная практика для ASP.NET WebForms, поскольку позволяет в компонентах для отображения данных напрямую обращаться к данным в ObjectDataSource без образования специальной типизированной модели
        // Но так писать не надо, потому что в представлении мы вынуждены будем использовать "магические строки" для получения доступа к значениям в строке данных
        //public IEnumerable<DataRow> List()
        //{
        //    using (MySqlConnection objConnect = new MySqlConnection(Base.strConnect))
        //    {
        //        string strSQL = "select * from users";
        //        using (MySqlCommand objCommand = new MySqlCommand(strSQL, objConnect))
        //        {
        //            objConnect.Open();
        //            using (MySqlDataAdapter da = new MySqlDataAdapter(objCommand))
        //            {
        //                DataTable dt = new DataTable();
        //                da.Fill(dt);
        //                return dt.AsEnumerable();
        //            }
        //        }
        //    }
        //}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Проверка запросов SQL на уязвимости безопасности")]
        public IList<User> List(string sortOrder, bool descending, int page, int pagesize, out int count)
        {
            List<User> users = new List<User>();
            // добавляем в запрос сортировку
            string sort = " ORDER BY ";
            // это плохая практика, потому что запрос может быть взломан при удачном встраивании в него некоей текстовой строки (inject)
            // но, к сожалению, MySQL не дает возможности использовать параметры для сортировки
            // поэтому надо экранировать кавычками, но перед этим обеспечить сначала проверку входного значения (чтобы тех же кавычек в нём не было)
            // в нашем проекте проверка значения идет в контроллере, перед простроением модели
            if (sortOrder != null && sortOrder != String.Empty)
            {
                sort += "`" + sortOrder + "`";
                if (descending) sort += " DESC";
                sort += ",";
            }
            sort += "`user_id`"; // по умолчанию
            // добавляем в запрос отображение только части записей (отображение страницами)
            string limit = "";
            if (pagesize > 0)
            {
                int start = (page - 1) * pagesize;
                limit = string.Concat(" LIMIT ", start.ToString(), ", ", pagesize.ToString());
            }
            string strSQL = "SELECT SQL_CALC_FOUND_ROWS u.`user_id`, u.`username`, u.`email`, u.`create_time` FROM `user_list` " + sort + limit;
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                //cmd.Parameters.Add("page", MySqlDbType.Int32).Value = page;
                //cmd.Parameters.Add("pagesize", MySqlDbType.Int32).Value = pagesize;
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        users.Add(new User(
                            dr.GetUInt64("user_id"),
                            dr.GetString("username"),
                            dr.IsDBNull(dr.GetOrdinal("email")) ? dr.GetString("email") : "",
                            dr.GetString("password"),
                            dr.IsDBNull(dr.GetOrdinal("create_time")) ? dr.GetDateTime("create_time") : null
                        ));
                    }
                }
            }
            // получаем общее количество пользователей
            using (MySqlCommand cmdrows = new MySqlCommand("SELECT FOUND_ROWS()", connection))
            {
                int.TryParse(cmdrows.ExecuteScalar().ToString(), out count);
            }
            return users;
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
                    {
                        return CompareCredentials(password, dr.GetString("password"));
                    }
                }
            }
            return false;
        }

        private MySqlConnection connection;
        private bool isOpen = false;
    }
}
