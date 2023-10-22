using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using flight_api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace flight_api.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
        public class FlightsController : ControllerBase
        {
        private readonly IConfiguration _configuration;
        private readonly flightDbContext _dbcontext;


        private readonly string _connectionString;

            public FlightsController(flightDbContext dbConext, IConfiguration configuration)
            {
            _dbcontext = dbConext;
            _configuration = configuration;
        }

        [HttpGet("getFlights")]
        public async Task<IActionResult> GetFlights()
        {
            List<Flight> flights = new List<Flight>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("sqlconnect")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("GetFlights", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Flight flight = new Flight();

                            flight.FlightId = (int)reader["flight_id"];
                          
                            flight.AirportName = (string)reader["airport_name"];
                            flight.DepartureLocation = (string)reader["departure_location"];
                            flight.Destination = (string)reader["destination"];
                            flight.DepartureTime = reader["departure_time"].ToString();
                            flight.ArrivalTime = reader["arrival_time"].ToString();
                            flight.FlightDate = DateTime.ParseExact(reader["flight_date"].ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture).ToString();
                            flight.DepartureDate = DateTime.ParseExact(reader["departure_date"].ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture).ToString();
                            flight.ArrivalDate = DateTime.ParseExact(reader["arrival_date"].ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture).ToString();
                            flight.TicketNumber = (string)reader["ticket_number"];

                            flights.Add(flight);
                        }
                    }
                }
            }

            return Ok(flights);
        }

        [HttpGet("getFlight/{id}")]
        public async Task<IActionResult> GetFlightById(int id)
        {
            Flight flight = null;

            using (var connection = new SqlConnection(_configuration.GetConnectionString("sqlconnect")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("GetFlightByID", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@flight_id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            flight = new Flight();

                            flight.FlightId = (int)reader["flight_id"];
                            flight.AirportName = (string)reader["airport_name"];
                            flight.DepartureLocation = (string)reader["departure_location"];
                            flight.Destination = (string)reader["destination"];
                            flight.DepartureTime = reader["departure_time"].ToString();
                            flight.ArrivalTime = reader["arrival_time"].ToString();
                            flight.FlightDate = DateTime.ParseExact(reader["flight_date"].ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture).ToString();
                            flight.DepartureDate = DateTime.ParseExact(reader["departure_date"].ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture).ToString();
                            flight.ArrivalDate = DateTime.ParseExact(reader["arrival_date"].ToString(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture).ToString();
                            flight.TicketNumber = (string)reader["ticket_number"];

                            // Set the airport name based on the departure location and destination
                           
                        }
                    }
                }
            }

            if (flight is not null)
            {
                return Ok(flight);
            }

            return NotFound();
        }

        [HttpPost("AddFlight")]
        public async Task<IActionResult> AddFlight([FromBody] Flight flight)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("sqlconnect")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("CreateFlight", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@airport_name", flight.AirportName);
                    command.Parameters.AddWithValue("@departure_location", flight.DepartureLocation);
                    command.Parameters.AddWithValue("@destination", flight.Destination);
                    command.Parameters.AddWithValue("@departure_time", flight.DepartureTime);
                    command.Parameters.AddWithValue("@arrival_time", flight.ArrivalTime);
                    command.Parameters.AddWithValue("@flight_date", flight.FlightDate);
                    command.Parameters.AddWithValue("@departure_date", flight.DepartureDate);
                    command.Parameters.AddWithValue("@arrival_date", flight.ArrivalDate);
                    command.Parameters.AddWithValue("@ticket_number", flight.TicketNumber);
                 
                    await command.ExecuteNonQueryAsync();
                }
            }

            return CreatedAtAction("GetFlightById", new { id = flight.FlightId }, flight);
        }

        [HttpPut("editFlight/{id}")]
        public async Task<IActionResult> UpdateFlight([FromBody]Flight flight)

        {

            using (var connection = new SqlConnection(_configuration.GetConnectionString("sqlconnect")))
            {
                await connection.OpenAsync();


                using (SqlCommand command = new SqlCommand("UpdateFlight", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                 
                   
                    command.Parameters.AddWithValue("@airport_name", flight.AirportName);
                    command.Parameters.AddWithValue("@departure_location", flight.DepartureLocation);
                    command.Parameters.AddWithValue("@destination", flight.Destination);
                    command.Parameters.AddWithValue("@departure_time", flight.DepartureTime);
                    command.Parameters.AddWithValue("@arrival_time", flight.ArrivalTime);
                    command.Parameters.AddWithValue("@flight_date", flight.FlightDate);
                    command.Parameters.AddWithValue("@departure_date", flight.DepartureDate);
                    command.Parameters.AddWithValue("@arrival_date", flight.ArrivalDate);
                    command.Parameters.AddWithValue("@ticket_number", flight.TicketNumber);
                    // All parameters

                    await command.ExecuteNonQueryAsync();
                }
            }

            return CreatedAtAction("GetFlightById", new { id = flight.FlightId }, flight);
        }

        [HttpDelete("deleteFlight/{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("sqlconnect")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("DeleteFlight", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@flight_id", id);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return NoContent();
        }

    }
}