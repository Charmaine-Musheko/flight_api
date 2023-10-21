using System;
using System.ComponentModel.DataAnnotations;

namespace flight_api.Models

{
    using System;

    public class Flight
    {
        [Key]
        public int FlightId { get; set; }
        public string AirportName { get; set; }
        public string DepartureLocation { get; set; }
        public string Destination { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string FlightDate { get; set; }
        public string DepartureDate { get; set; }
        public string ArrivalDate { get; set; }

        public string TicketNumber { get; set; }

        //// You can add navigation property for the associated airline here if needed
        //public Airline Airline { get; set; }
    }

}

