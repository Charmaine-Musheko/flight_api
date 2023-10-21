using System;
using flight_api.Models;
using Microsoft.EntityFrameworkCore;

namespace flight_api
{
	public class flightDbContext : DbContext
	{
		public flightDbContext(DbContextOptions<flightDbContext> options) : base(options)
        {

		}
        public virtual DbSet<User> user { get; set; }
        public virtual DbSet<Flight> flights { get; set; }
    }
}

