using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

using Interfaces.Models;
using System.Security.Cryptography.X509Certificates;

namespace GameServer
{
    public class DatabaseService
    {
        public class NotFoundException : Exception
        {
            public NotFoundException(string message) : base(message)
            {}
        }

        public DatabaseService(int _gameServerId, string _login, string _password, string _server = "localhost", int _port = 3306)
        {
            gameServerId = _gameServerId;
            login = _login;
            password = _password;
            server = _server;
            port = _port;
        }

        public Session GetSession(int id)
        {
            string strSQL = "SELECT * FROM `session_list` WHERE `id`=@id;";
            using (MySqlConnection connection = CreateConnection())
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                connection.Open();
                cmd.Parameters.Add("id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = id;
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        SessionStartParameters parameters = GetSessionParameters(dr);
                        return new Session
                        {
                            Parameters = parameters,
                            Players = GetSessionPlayers(id)
                        };
                    }
                    dr.Close();
                }
            }

            throw new NotFoundException("Failed get session information");
        }

        private List<int> GetSessionPlayers(int sessionId)
        {
            List<int> res = new List<int>();

            string strSQL = "SELECT `user_id` FROM `session_members` WHERE `session_id`=@session_id;";
            using (MySqlConnection connection = CreateConnection())
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                connection.Open();
                cmd.Parameters.Add("session_id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = sessionId;
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        res.Add(dr.GetInt32("user_id"));
                    dr.Close();
                }
                return res;
            }
        }

        private SessionStartParameters GetSessionParameters(MySqlDataReader dr)
        {
            return new SessionStartParameters
            {
                Name = dr.GetString("name"),
                Description = dr.GetString("description"),
                Size = dr.GetInt32("size"),
                PlanetCount = dr.GetInt32("planet_count"),
                PlayerLimit = dr.GetInt32("player_count")
            };
        }

        public void SetServerStatus(bool status)
        {
            string strSQL = "UPDATE `server_list` SET `status`=@status WHERE server_id=@server_id";
            using (MySqlConnection connection = CreateConnection())
            using (MySqlCommand cmd = new MySqlCommand(strSQL, connection))
            {
                connection.Open();
                cmd.Parameters.Add("server_id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = gameServerId;
                cmd.Parameters.Add("status", MySql.Data.MySqlClient.MySqlDbType.Byte).Value = status;

                if(cmd.ExecuteNonQuery() != 1)
                {
                    throw new Exception("Failure while update server state in the database");
                }
            }
        }

        private MySqlConnection CreateConnection()
        {
            return new MySqlConnection("server=" + server + ";user=" + login + ";port=" + port + ";database=planetwars;password=" + password + ";");
        }


        private int gameServerId = -1;

        private string server;
        private string login;
        private int port;
        private string password;
    }
}
