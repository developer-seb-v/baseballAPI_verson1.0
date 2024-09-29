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
            string connection = "server=localhost;user=root;database=baseball;port=3307;password=password123";
            MySqlConnection mySqlConnection = new MySqlConnection(connection);
            try
            {   
                  List<Player> listOfPlayers = new List<Player>();
                  mySqlConnection.Open();
                  string get = "SELECT player.PlayerNumber, player.FirstName, player.LastName, position.PositionName, country.CountryName  FROM ((player INNER JOIN position ON  player.PositionId = position.PositionId) INNER JOIN country ON player.CountryId = country.CountryId)";
                  MySqlCommand cmd = new MySqlCommand(get, mySqlConnection);
                  MySqlDataReader reader = cmd.ExecuteReader();
                  while (reader.Read())
                  {
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