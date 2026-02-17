using Microsoft.EntityFrameworkCore;
using AmarisScheduleAppointments.Infrastructure.DataBase;
using AmarisScheduleAppointments.Application.Interfaces;
using AmarisScheduleAppointments.Application.Services;
using AmarisScheduleAppointments.Domain.Entities;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=AmarisTurns.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString,
    b => b.MigrationsAssembly("AmarisScheduleAppointments.Infrastructure")));

builder.Services.AddScoped<IShiftService, ShiftService>();

//CORS para Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Crea BD y añade sucursales automáticamente al ejecutar 
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();