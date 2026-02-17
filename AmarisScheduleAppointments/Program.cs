using Microsoft.EntityFrameworkCore;
using AmarisScheduleAppointments.Infrastructure.DataBase;
using AmarisScheduleAppointments.Application.Interfaces;
using AmarisScheduleAppointments.Application.Services;
using AmarisScheduleAppointments.Domain.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=AmarisTurns.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString,
    b => b.MigrationsAssembly("AmarisScheduleAppointments.Infrastructure")));

builder.Services.AddScoped<IShiftService, ShiftService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS para Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AngularApp");
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        if (!context.Branches.Any())
        {
            context.Branches.AddRange(
                new Branch { Name = "Sucursal Norte", Address = "Calle 100 # 15" },
                new Branch { Name = "Sucursal Centro", Address = "Carrera 7 # 26" },
                new Branch { Name = "Sucursal Sur", Address = "Avenida 1 # 10" }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al migrar o sembrar la base de datos.");
    }
}

app.Run();