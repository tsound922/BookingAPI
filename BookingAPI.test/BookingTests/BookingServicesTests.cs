using BookingAPI.Services;
using BookingAPI.Model;
using System;
using FluentAssertions;

namespace BookingAPI.test.ControllerTests
{
    public class BookingServicesTests
    {
        BookingRepository bookService = new BookingRepository();

        [Theory]
        [InlineData("47c2c0ac-fbb4-4867-ad8b-0537315939e4")]
        public async Task BookTimeServiceTest(string uuid)
        {
            var result = await bookService.BookTimeSlotAsync(uuid, new BookingRequest
            {
                BookingTime = "14:51",
                Name = "Test booking"
            });

            result.Should().NotBeNull();
            result.Should().Be(uuid);
        }

        [Theory]
        [InlineData("19:00")]
        [InlineData("21:00")]
        [InlineData("17:01")]
        [InlineData("18:01")]
        public void InValidBusinessHourTest(string bookTime) 
        {
            var result = bookService.IsInBusinessHour(bookTime);
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("15:59")]
        [InlineData("09:00")]
        [InlineData("10:00")]
        [InlineData("12:01")]
        public void ValidBusinessHourTest(string bookTime)
        {
            var result = bookService.IsInBusinessHour(bookTime);
            result.Should().BeTrue();
        }
    }
}