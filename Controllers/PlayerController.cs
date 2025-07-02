using System.ComponentModel;
using baseballAPI.Models;
using baseballAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Mysqlx;

namespace baseballAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly string _connection = "server=localhost;user=root;database=baseball;port=3308;password=password123";

        // [Authorize] if you want to protect this endpoint from unauthorized users uncomment this attribute
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

       
            [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDTO>> GetProduct(int id)
        {
            PlayerDTO player = new PlayerDTO();

            using (MySqlConnection connection = new MySqlConnection(_connection))
            {
                await connection.OpenAsync();
                string query = "SELECT player.first_name, player.last_name, position.pos_name, country.country_name  FROM ((player INNER JOIN position ON  player.pos_id = position.pos_id) INNER JOIN country ON player.country_id = country.country_id) WHERE player_number =  @Id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            player = new PlayerDTO
                            {

                                FirstName = reader["first_name"].ToString(),
                                LastName = reader["last_name"].ToString(),
                                Position = reader["pos_name"].ToString(),
                                Country = reader["country_name"].ToString()

                            };
                        }
                    }
                }
            }

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        [AllowAnonymous]
        [HttpPost("add")]
        public async Task<IActionResult> AddPlayerObject([FromBody] Player player)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(_connection);
                string query =
                    $"INSERT INTO `player` "
                    + "(player_number, first_name, last_name, pos_id, country_id)"
                    + "VALUES (@num, @fn, @ln, @pos, @cid)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@num", player.PlayerNumber);
                cmd.Parameters.AddWithValue("@fn", player.FirstName);
                cmd.Parameters.AddWithValue("@ln", player.LastName);
                cmd.Parameters.AddWithValue("@pos", player.Position);
                cmd.Parameters.AddWithValue("@cid", player.Country);

                //cmd.Connection.Open();
                await cmd.Connection.OpenAsync();
                cmd.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex);
            }

            return Ok(player);
            // int rowsAffected = await cmd.ExecuteNonQueryAsync();
            //
            // if (rowsAffected > 0)
            // {
            //     return Ok("Customer inserted successfully.");
            // }
            //
            // return StatusCode(500, "An error occurred while inserting the customer.");
        }

        // http delete
        [HttpDelete("{id}")]
        public ActionResult DeletePlayer(int id)
        {
            MySqlConnection conn = new MySqlConnection(_connection);
            string query = $" DELETE FROM `player` " + "WHERE player_number = @num";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@num", id);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            return Ok(id);
        }

        // // http put
        // // not sure how to implement this one, which values to update
        // [HttpPut]
        // public void UpdatePlayer(Player player)
        // {
        //     MySqlConnection conn = new MySqlConnection(_connection);
        //     string query =
        //         "UPDATE `player` "
        //         + "SET first_name = @fn, last_name = @ln, pos_id= @pos, country_id = @cid "
        //         + "WHERE player_number = @num";
        //     MySqlCommand cmd = new MySqlCommand(query, conn);
        //     cmd.Parameters.AddWithValue("@fn", player.FirstName);
        //     cmd.Parameters.AddWithValue("@ln", player.LastName);
        //     cmd.Parameters.AddWithValue("@pos", player.Position);
        //     cmd.Parameters.AddWithValue("@cid", player.Country);
        //     cmd.Parameters.AddWithValue("@num", player.PlayerNumber);
        //     cmd.Connection.Open();
        //     cmd.ExecuteNonQuery();
        // }

        [HttpPut("{playerNumber}")]
        public ActionResult UpdatePlayer(int playerNumber, [FromBody] Player player)
        {
            // Validate the request: Ensure playerNumber matches the player's PlayerNumber
            if (playerNumber != player.PlayerNumber)
            {
                return BadRequest("Player number in URL does not match the player number in the body.");
            }

            // SQL UPDATE query
            string query = @"
            UPDATE player
            SET first_name = @FirstName, last_name = @LastName, pos_id = @Position, country_id = @Country
            WHERE player_number = @PlayerNumber";

            // Create MySQL connection
            using (MySqlConnection conn = new MySqlConnection(_connection))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Add parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@FirstName", player.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", player.LastName);
                        cmd.Parameters.AddWithValue("@Position", player.Position);
                        cmd.Parameters.AddWithValue("@Country", player.Country);
                        cmd.Parameters.AddWithValue("@PlayerNumber", player.PlayerNumber);

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // If no rows were affected, the player was not found
                        if (rowsAffected == 0)
                        {
                            return NotFound("Player not found.");
                        }

                        // Return 200 OK if the update is successful
                        return Ok("Player updated successfully.");
                    }
                }
                catch (MySqlException ex)
                {
                    // Handle exceptions and return internal server error
                    return StatusCode(500, $"Error updating player: {ex.Message}");
                }
            }
        }

    }
}
