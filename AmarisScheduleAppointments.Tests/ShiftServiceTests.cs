global using Xunit;
using AmarisScheduleAppointments.Application.Services;
using AmarisScheduleAppointments.Domain.Entities;
using AmarisScheduleAppointments.Infrastructure.DataBase;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
public class ShiftServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task RegisterShiftAsync()
    {
        // Arrange
        var context = GetDbContext();
        var service = new ShiftService(context);
        var cedula = "123456";

        // Creamos 5 turnos previos para hoy
        for (int i = 0; i < 5; i++)
        {
            context.Shifts.Add(new Shift
            {
                IdentificationNumber = cedula,
                CreatedAt = DateTime.Now,
                BranchId = 1,
                Status = "Pending"
            });
        }
        await context.SaveChangesAsync();

        var newShift = new Shift { IdentificationNumber = cedula, BranchId = 1 };

        // Act & Assert
        Func<Task> act = async () => await service.RegisterShiftAsync(newShift);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Ya has solicitado el máximo de 5 turnos permitidos por hoy.");
    }

    [Fact]
    public async Task RegisterShiftLimitTimeCorrecty()
    {
        // Arrange
        var context = GetDbContext();
        var service = new ShiftService(context);
        var newShift = new Shift { IdentificationNumber = "999", BranchId = 1 };

        // Act
        var result = await service.RegisterShiftAsync(newShift);

        // Assert
        result.LimitTime.Should().BeCloseTo(result.CreatedAt.AddMinutes(15), TimeSpan.FromSeconds(1));
        result.Status.Should().Be("Pending");
    }
}