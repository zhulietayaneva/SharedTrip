using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static SharedTrip.Data.DataConstants;

namespace SharedTrip.Data.Models
{
    public class Trip
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        public string StartPoint { get; set; }
        [Required]
        public string EndPoint { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        public int Seats { get; set; }
        [Required]
        [MaxLength(DescriptionMaxLenght)]
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public IEnumerable<UserTrip> UserTrips { get; set; } = new List<UserTrip>();

    }
}