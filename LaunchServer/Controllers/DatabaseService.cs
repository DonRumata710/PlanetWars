using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

using Interfaces.Models;
using LaunchServer.Models;

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
            connection.Close();
        }

        public int CreateNewSession(SessionStartParameters session)
        {
            string strSQL = "create_session(@session_id, @name, @description, @size, @planetCount, @playerCount);";
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                cmd.Parameters.Add("name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = session.Name;
                cmd.Parameters.Add("description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = session.Description;
                cmd.Parameters.Add("size", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = session.Size;
                cmd.Parameters.Add("planetCount", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = session.PlanetCount;
                cmd.Parameters.Add("playerCount", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = session.PlayerLimit;
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    return dr.GetInt32("session_id");
                }
            }

            throw new Exception("Failed create session");
        }

        public void RefreshSession(int id, SessionStartParameters session)
        {
            string strSQL = "UPDATE `session_list` SET `name`=@name, `description`=@description, `size`=@size, `planet_count`=@planet_count, `player_count`=@player_count;";
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                cmd.Parameters.Add("name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = session.Name;
                cmd.Parameters.Add("description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = session.Description;
                cmd.Parameters.Add("size", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = session.Size;
                cmd.Parameters.Add("planet_count", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = session.PlanetCount;
                cmd.Parameters.Add("player_count", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = session.PlayerLimit;
                if (cmd.ExecuteNonQuery() != 1)
                {
                    throw new Exception("Failure while update session");
                }
            }
        }

        public void StartSession(int session, List<int> players, int server)
        {
            foreach (int player in players)
                AddPlayer(player, session);

            string strSQL = "start_session(@sessionId, @serverId);";
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                cmd.Parameters.Add("sessionId", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = session;
                cmd.Parameters.Add("serverId", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = server;
                cmd.ExecuteNonQuery();
            }
        }

        private void AddPlayer(int playerId, int sessionId)
        {
            string strSQL = "add_player_to_session(@sessionId, @playerId);";
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                cmd.Parameters.Add("sessionId", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = sessionId;
                cmd.Parameters.Add("playerId", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = playerId;
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

        public UserInfo GetUserInfo(string name)
        {
            string strSQL = "SELECT * FROM `user_list` WHERE `username`=@username";
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    cmd.Parameters.Add("username", MySqlDbType.String).Value = name;
                    return GetUserInfo(cmd);
                }
            }
        }

        public UserInfo GetUserInfo(int id)
        {
            string strSQL = "SELECT * FROM `user_list` WHERE `user_id`=@user_id";
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                cmd.Parameters.Add("user_id", MySqlDbType.Int32).Value = id;
                return GetUserInfo(cmd);
            }
        }

        private UserInfo GetUserInfo(MySqlCommand cmd)
        {
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                UserInfo info = new UserInfo();
                if (dr.Read())
                {
                    info.email = !dr.IsDBNull(dr.GetOrdinal("email")) ? dr.GetString("email") : "";
                    info.userId = dr.GetUInt64("user_id");
                    info.name = dr.GetString("username");
                    info.registerTime = !dr.IsDBNull(dr.GetOrdinal("create_time")) ? dr.GetDateTime("create_time") : null;
                    info.isActive = dr.GetBoolean("active_user");
                }
                return info;
            }
        }

        private MySqlConnection connection;
    }
}
