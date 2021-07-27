using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

using Interfaces.Models;


namespace LaunchServer.Controllers
{
    public class DatabaseService
    {
        public DatabaseService(string login, string password, string server = "localhost", int port = 3306)
        {
            connection = new MySqlConnection("server=" + server + ";user=" + login + ";port=" + port + ";database=planetwars;password=" + password + ";");
            connection.Open();
        }

        ~DatabaseService()
        {
            Close();
        }

        public void Open()
        {
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

        public int CreateNewSession(Session session)
        {
            string strSQL = "INSERT INTO `session_list` (`server_id`, `name`, `description`, `size`, `planet_count`, `player_count`) VALUES (@id, @name, @description, @size, @planetCount, @playerCount);";
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                cmd.Parameters.Add("id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = session.ServerId;
                cmd.Parameters.Add("name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = session.Parameters.Name;
                cmd.Parameters.Add("description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = session.Parameters.Description;
                cmd.Parameters.Add("size", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = session.Parameters.Size;
                cmd.Parameters.Add("planetCount", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = session.Parameters.PlanetCount;
                cmd.Parameters.Add("playerCount", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = session.Players.Count;

                if (cmd.ExecuteNonQuery() >= 0)
                {
                    strSQL = "SELECT LAST_INSERT_ID() AS ID";
                    cmd.CommandText = strSQL;
                    int ID = 0;
                    int.TryParse(cmd.ExecuteScalar().ToString(), out ID);

                    return ID;
                }
            }

            throw new Exception("Failed create session");
        }

        public void StartSession(int id)
        {
            string strSQL = "UPDATE `session_list` SET `start_time`=CURRENT_TIMESTAMP";
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public Dictionary<string, GameServer> ServerList()
        {
            string strSQL = "SELECT * FROM `server_list`";
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    Dictionary<string, GameServer> res = new Dictionary<string, GameServer>();
                    while (dr.Read())
                    {
                        GameServer serverModel = new GameServer();
                        serverModel.ActiveSessions = 0;
                        serverModel.SessionLimit = dr.GetInt64("session_limit");
                        res.Add(dr.GetString("address").ToString(), serverModel);
                    }
                    return res;
                }
            }
        }

        private MySqlConnection connection;
    }
}
