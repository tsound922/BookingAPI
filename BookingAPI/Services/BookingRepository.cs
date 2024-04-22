using BookingAPI.Model;
using System;

namespace BookingAPI.Services
{
    public class BookingRepository: IBookingRepository
    {
        private static readonly Dictionary<string, BookingInformation> _bookings = new Dictionary<string, BookingInformation>();
        private const int MaxSimultaneousBookings = 4;


        public bool IsInBusinessHour(string bookingTime)
        {
            if (string.IsNullOrEmpty(bookingTime) || bookingTime.Length != 5 || !int.TryParse(bookingTime.Substring(0, 2), out int hour) || !int.TryParse(bookingTime.Substring(3, 2), out int minute))
                return false;

            if (hour < 9 || hour > 16)
                return false;

            if (hour == 16 && minute > 0)
                return false;

            return true;
        }

        public bool IsTimeSlotsAvailable(string bookingTime)
        {
            var bookingHour = bookingTime.Substring(0, 2);
            var existingBookingsForHour = _bookings.Values.Count(b => b.BookingTime.Substring(0, 2) == bookingHour);
            if (existingBookingsForHour >= MaxSimultaneousBookings)
            {
                return false;
            }
            else 
            {
                return true;
            }
        }

        public bool IsValidTime(string bookingTime)
        {
            return TimeSpan.TryParse(bookingTime, out _);
        }

        public async Task<string> BookTimeSlotAsync(string uuid, BookingRequest requestBody)
        {
            try
            {
                BookingInformation booking = new BookingInformation
                {
                    Id = uuid,
                    Name = requestBody.Name,
                    BookingTime = requestBody.BookingTime
                };

                await Task.Run(() => _bookings.Add(uuid, booking));

                return uuid;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot book time slot, please check your input", ex);
            }
            finally 
            {
                _bookings.Remove(uuid);
            }
        }

        //Assume we process the booked slots so we can reset the slot for new booking.
        public async Task<bool> ProcessBookingSlotsAsync() 
        {
            try
            {
                await Task.Run(() => _bookings.Clear());

                if (_bookings.Count() == 0)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot processing the Booking slot", ex);
            }
        }
    }
}
