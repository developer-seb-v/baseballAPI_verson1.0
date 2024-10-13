using baseballAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace baseballAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        [HttpGet]
        public List<Player> GetPlayers()
        {
            // created join query to get foreign table data
            string connection = "server=localhost;user=root;database=baseball_db;port=3308;password=password123";
            MySqlConnection mySqlConnection = new MySqlConnection(connection);
            try
            {   
                  List<Player> listOfPlayers = new List<Player>();
                  mySqlConnection.Open();
                  string get = "SELECT player.player_number, player.first_name, player.last_name, position.pos_name, country.country_name  FROM ((player INNER JOIN position ON  player.pos_id = position.pos_id) INNER JOIN country ON player.country_id = country.country_id)";
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
                    Player P = new Player();
                    P.PlayerNumber = Convert.ToInt32(reader.GetValue(0));
                    P.FirstName = reader.GetValue(1).ToString();
                    P.LastName = reader.GetValue(2).ToString();
                    P.Position = reader.GetValue(3).ToString(); 
                    P.Country = reader.GetValue(4).ToString();
                    listOfPlayers.Add(P);
                 }
                mySqlConnection.Close();
                return listOfPlayers;
            }
            catch (System.Exception)
            {    
                throw;
            }
        }

        // http post
        

        // http delete
    

        // http put 
    }
}