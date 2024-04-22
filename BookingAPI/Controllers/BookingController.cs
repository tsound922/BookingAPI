using BookingAPI.Model;
using BookingAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        }

        [HttpPost]
        public async Task<IActionResult> BookTimeSlot(BookingRequest requestBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_bookingRepository.IsInBusinessHour(requestBody.BookingTime))
            {
                return BadRequest("Out of business hours");
            }
            if (_bookingRepository.IsTimeSlotsAvailable(requestBody.BookingTime) == false)
            {
                return Conflict("Booking time slots are full");
            }
            if (string.IsNullOrEmpty(requestBody.Name) || string.IsNullOrWhiteSpace(requestBody.Name))
            {
                return BadRequest("Name must be provided");
            }
            if (!_bookingRepository.IsValidTime(requestBody.BookingTime))
            {
                return BadRequest("Invalid booking time format");
            }
            return Ok("\"" + "bookingId: " + "\"" + "\"" + await _bookingRepository.BookTimeSlotAsync(Guid.NewGuid().ToString(), requestBody) + "\"");
        }

        [HttpPost("bookingSlot", Name = "ProcessBookingSlots")]
        public async Task<IActionResult> ProcessBookingSlots() 
        {
            bool result = await _bookingRepository.ProcessBookingSlotsAsync();
            if (result == true)
            {
                return Ok("Booking slots processed");
            }
            else 
            {
                return BadRequest("Cannot process the booking slot");
            }
        }
    }

}
