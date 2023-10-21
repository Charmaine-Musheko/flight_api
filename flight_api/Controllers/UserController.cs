using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using flight_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace flight_api.Controllers
{
    [Route("api/User")]

    public class UserController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly flightDbContext _dbcontext;
        private readonly IConfiguration _configuration;

        public UserController(flightDbContext dbConext, IConfiguration configuration)
        {
            _dbcontext = dbConext;
            _configuration = configuration;
        }

        [HttpGet("getMe")]
        public async Task<IActionResult> Get()
        {
            var userResults = new List<User>(); // Create a list to store user data.
            using (var connection = new SqlConnection(_configuration.GetConnectionString("sqlconnect")))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("ReadFlightUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters if necessary, e.g., command.Parameters.AddWithValue("@ParamName", paramValue);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var user = new User
                            {
                                user_id = (int)reader["user_id"], 
                                first_name = (string)reader["first_name"], 
                                last_name = (string)reader["last_name"], 
                                email = (string)reader["email"], 
                                password = (string)reader["password"], 
                                address = (string)reader["address"] 
                            };

                            userResults.Add(user);
                        }
                    }
                }
            }

            return Ok(userResults);
        }

        [HttpGet("getMe/{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var connectionString = _configuration.GetConnectionString("sqlconnect");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("GetUserByID", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@user_id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var user = new User
                            {
                                user_id = (int)reader["user_id"],
                                first_name = (string)reader["first_name"],
                                last_name = (string)reader["last_name"],
                                email = (string)reader["email"],
                                password = (string)reader["password"],
                                address = (string)reader["address"]
                            };

                            return user;
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
        }
                




                // POST api/values

                [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("sqlconnect")))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("CreateUser", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@first_name", user.first_name);
                command.Parameters.AddWithValue("@last_name", user.last_name);
                command.Parameters.AddWithValue("@email", user.email);
                command.Parameters.AddWithValue("@password", user.password);
                command.Parameters.AddWithValue("@address", user.address);

                await command.ExecuteNonQueryAsync();

                return CreatedAtAction("GetUserById", new { id = user.user_id }, user);
            }
        }



        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

   
}

