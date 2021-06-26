using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Data.Models
{
    public class UserTrip
    {
        [Key]
        public string UserId { get; set; }
        public User User { get; set; }
        [Key]
        public string TripId { get; set; }
        public Trip Trip { get; set; }

    }
}