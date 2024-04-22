using System.ComponentModel.DataAnnotations;

namespace BookingAPI.Model
{
    public class BookingRequest
    {
        [Required]
        [RegularExpression(@"^(0[9-9]|1[0-6]):[0-5][0-9]$", ErrorMessage = "Booking time format is invalid.")]
        public string BookingTime { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must not be none or empty.")]
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
