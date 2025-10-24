using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline.Domain.Items
{
    /// <summary>
    /// The class for flight description and information about it.
    /// </summary>
    public class Flight
    {
        /// <summary>
        /// Unique flight's ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Flight's code.
        /// </summary>
        public required string FlightCode { get; set; }

        /// <summary>
        /// The place of departure.
        /// </summary>
        public required string DepartureCity { get; set; }

        /// <summary>
        /// The place of arrival.
        /// </summary>
        public required string ArrivalCity { get; set; }

        /// <summary>
        /// Date of the departure.
        /// </summary>
        public DateOnly? DepartureDate { get; set; }

        /// <summary>
        /// Date of the arrival.
        /// </summary>
        public DateOnly? ArrivalDate { get; set; }

        /// <summary>
        /// Flight's eparture time.
        /// </summary>
        public TimeSpan? DepartureTime { get; set; }

        /// <summary>
        /// Flight's travel time.
        /// </summary>
        public TimeSpan? TravelTime { get; set; }

        /// <summary>
        /// The model of plane.
        /// </summary>
        public required PlaneModel Model { get; set; }
    }
}
