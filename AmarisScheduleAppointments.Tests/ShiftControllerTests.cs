using AmarisScheduleAppointments.Application.Interfaces;
using AmarisScheduleAppointments.Controllers;
using AmarisScheduleAppointments.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;


public class ShiftControllerTests
{
    [Fact]
    public async Task CreateShift_ReturnsOk_WhenShiftIsCreated()
    {
        // Arrange
        var mockService = new Mock<IShiftService>();
        var shift = new Shift { IdentificationNumber = "123" };
        mockService.Setup(s => s.RegisterShiftAsync(It.IsAny<Shift>()))
                   .ReturnsAsync(shift);

        var controller = new ShiftController(mockService.Object);

        // Act
        var result = await controller.CreateShift(shift);

        // Assert
        var okResult = Assert.IsType<ActionResult<Shift>>(result);
        Assert.IsType<OkObjectResult>(okResult.Result);
    }
}
