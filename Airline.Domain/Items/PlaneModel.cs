using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline.Domain.Items
{

    /// <summary>
    /// The class for information about a plane model.
    /// </summary>
    public class PlaneModel
    {
        /// <summary>
        /// Unique plane model's ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The name of plane model.
        /// </summary>
        public required string ModelName { get; set; }

        /// <summary>
        /// The model family of the plane.
        /// </summary>
        public required ModelFamily PlaneFamily { get; set; }

        /// <summary>
        /// The max flight range of the plane model.
        /// </summary>
        public required double MaxRange { get; set; }

        /// <summary>
        /// The passenger capacity of the plane model (tons).
        /// </summary>
        public required double PassengerCapacity { get; set; }

        /// <summary>
        /// The cargo capacity of the plane model (tons).
        /// </summary>
        public required double CargoCapacity { get; set; }
    }

}
