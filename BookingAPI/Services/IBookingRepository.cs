using BookingAPI.Model;

namespace BookingAPI.Services
{
    public interface IBookingRepository
    {
        bool IsInBusinessHour(string bookTime);
        bool IsTimeSlotsAvailable(string bookTime);
        bool IsValidTime(string bookTime);
        Task<string> BookTimeSlotAsync(string uuid, BookingRequest requestBody);
        Task<bool> ProcessBookingSlotsAsync();
    }
}
