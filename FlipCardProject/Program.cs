using System.Configuration;
using FlipCardProject.Data;
using FlipCardProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers(); 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin() // Allow any origin
                .AllowAnyMethod() // Allow any HTTP method
                .AllowAnyHeader(); // Allow any header
        });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<DataContext>((serviceProvider, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseSettings"));
});

builder.Services.AddScoped</*IFlipcardRepository, */FlipcardRepository>();
builder.Services.AddSingleton<UserTrackingService<int>>();
builder.Services.AddSingleton<UserTrackingService<string>>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dbContext.Database.Migrate();
}
app.MapControllers();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.Run();