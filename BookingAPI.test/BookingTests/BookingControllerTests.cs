using BookingAPI.Controllers;
using BookingAPI.Model;
using BookingAPI.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingAPI.test.BookingTests
{
    public class BookingControllerTests
    {
        [Fact]
        public async Task BookTimeSlot_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var mockRepository = new Mock<IBookingRepository>();
            var controller = new BookingController(mockRepository.Object);
            controller.ModelState.AddModelError("error", "model is invalid");
            var invalidRequest = new BookingRequest();

            // Act
            var result = await controller.BookTimeSlot(invalidRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task BookTimeSlot_OutOfBusinessHours_ReturnsBadRequest()
        {
            // Arrange
            var mockRepository = new Mock<IBookingRepository>();
            mockRepository.Setup(repo => repo.IsInBusinessHour(It.IsAny<string>())).Returns(false);
            var controller = new BookingController(mockRepository.Object);
            var request = new BookingRequest { Name = "Test", BookingTime = "19:00" };

            // Act
            var result = await controller.BookTimeSlot(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Out of business hours", (result as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task BookTimeSlot_TimeSlotsFull_ReturnsConflict()
        {
            // Arrange
            var mockRepository = new Mock<IBookingRepository>();
            mockRepository.Setup(repo => repo.IsInBusinessHour(It.IsAny<string>())).Returns(true);
            mockRepository.Setup(repo => repo.IsTimeSlotsAvailable(It.IsAny<string>())).Returns(false);
            var controller = new BookingController(mockRepository.Object);
            var request = new BookingRequest {Name="Test", BookingTime = "12:00" };

            // Act
            var result = await controller.BookTimeSlot(request);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Booking time slots are full", (result as ConflictObjectResult).Value);
        }

        [Fact]
        public async Task ProcessBookingSlots_Success_ReturnsOk()
        {
            var mockRepository = new Mock<IBookingRepository>();
            mockRepository.Setup(repo => repo.ProcessBookingSlotsAsync()).ReturnsAsync(true);
            var controller = new BookingController(mockRepository.Object);

            var result = await controller.ProcessBookingSlots();

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Booking slots processed", (result as OkObjectResult).Value);
        }

        [Fact]
        public async Task ProcessBookingSlots_Failure_ReturnsBadRequest()
        {
            var mockRepository = new Mock<IBookingRepository>();
            mockRepository.Setup(repo => repo.ProcessBookingSlotsAsync()).ReturnsAsync(false);
            var controller = new BookingController(mockRepository.Object);

            var result = await controller.ProcessBookingSlots();

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cannot process the booking slot", (result as BadRequestObjectResult).Value);
        }
    }
}
