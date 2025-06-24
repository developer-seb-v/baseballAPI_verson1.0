using Microsoft.AspNetCore.Mvc;
using baseballAPI.Models;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authorization;

namespace baseballAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly string _connection ="server=localhost;user=root;database=baseball;port=3308;password=password123";
        
        [HttpGet("country")]
        public List<Country> GetCountries()
        {
            MySqlConnection mySqlConnection = new MySqlConnection(_connection);
            try
            {
                List<Country> countries = new List<Country>();
                mySqlConnection.Open();
                // created join query to get foreign table data
                string get = "SELECT country.country_id, country.country_name FROM country";
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
                    Country c = new Country
                    {
                        CountryId = Convert.ToInt32(reader.GetValue(0)),
                        CountryName = reader.GetValue(1).ToString(),
                    };
                    countries.Add(c);
                }

                mySqlConnection.Close();
                return countries;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("addcountry")]
        public async Task<IActionResult> AddCountry([FromBody] Country country)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(_connection);
                string query = $"INSERT INTO `country` " + "(country_name)" + "VALUES (@name)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", country.CountryName);

                //cmd.Connection.Open();
                await cmd.Connection.OpenAsync();
                cmd.ExecuteNonQuery();
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex);
            }

            return Ok(country);
            // int rowsAffected = await cmd.ExecuteNonQueryAsync();
            //
            // if (rowsAffected > 0)
            // {
            //     return Ok("Customer inserted successfully.");
            // }
            //
            // return StatusCode(500, "An error occurred while inserting the customer.");
        }
    }


}
