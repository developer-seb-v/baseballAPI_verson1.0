using baseballAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace baseballAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly string _connection =
            "server=localhost;user=root;database=baseball;port=3308;password=password123";

       // [Authorize]
        [HttpGet]
        public List<Player> GetPlayers()
        {
            MySqlConnection mySqlConnection = new MySqlConnection(_connection);
            try
            {
                List<Player> listOfPlayers = new List<Player>();
                mySqlConnection.Open();
                // created join query to get foreign table data
                string get =
                    "SELECT player.player_number, player.first_name, player.last_name, position.pos_name, country.country_name  FROM ((player INNER JOIN position ON  player.pos_id = position.pos_id) INNER JOIN country ON player.country_id = country.country_id)";
                MySqlCommand cmd = new MySqlCommand(get, mySqlConnection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    // mapping the DB entities to C# class
                    // The player model mirrors the DB entity
                    // So just as DB entity has PlayerNumber, so does the class
                    // and so on
                    // the columns are accessed as a 0-indexed array
                    // 0=PlayerNumber, 1=FirstName, 2=LastName, 3=PositionId, 4=CountryId
                    Player p = new Player
                    {
                        PlayerNumber = Convert.ToInt32(reader.GetValue(0)),
                        FirstName = reader.GetValue(1).ToString(),
                        LastName = reader.GetValue(2).ToString(),
                        Position = reader.GetValue(3).ToString(),
                        Country = reader.GetValue(4).ToString(),
                    };
                    listOfPlayers.Add(p);
                }
                mySqlConnection.Close();
                return listOfPlayers;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        // http post - Add player using parameters
        [HttpPost]
        public void AddPlayer(string pnum, string fn, string ln, string posid, string cid)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(_connection);

                string query =
                    $"INSERT INTO `player` "
                    + "(player_number, first_name, last_name, pos_id, country_id)"
                    + "VALUES (@num, @fn, @ln, @pos, @cid)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@num", pnum);
                cmd.Parameters.AddWithValue("@fn", fn);
                cmd.Parameters.AddWithValue("@ln", ln);
                cmd.Parameters.AddWithValue("@pos", posid);
                cmd.Parameters.AddWithValue("@cid", cid);

                cmd.Connection.Open();

                cmd.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex);
            }
        }

        // http delete
        [HttpDelete]
        public int DeletePlayer(int id)
        {
            MySqlConnection conn = new MySqlConnection(_connection);

            string query = $" DELETE FROM `player` " + "WHERE player_number = @num";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@num", id);

            cmd.Connection.Open();

            cmd.ExecuteNonQuery();

            return id;
        }

        // http put
        // not sure how to implement this one, which values to update
        [HttpPut]
        public void UpdatePlayer(string pnum, string fn, string ln, string posid, string cid)
        {
            MySqlConnection conn = new MySqlConnection(_connection);

            string query =
                "UPDATE `player` "
                + "SET first_name = @fn, last_name = @ln, pos_id= @pos, country_id = @cid "
                + "WHERE player_number = @num";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@fn", fn);
            cmd.Parameters.AddWithValue("@ln", ln);
            cmd.Parameters.AddWithValue("@pos", posid);
            cmd.Parameters.AddWithValue("@cid", cid);
            cmd.Parameters.AddWithValue("@num", pnum);

            cmd.Connection.Open();

            cmd.ExecuteNonQuery();
        }
    }
}
