using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using FlipCardProject.Data;
using FlipCardProject.Helpers;
using FlipCardProject.Models;
using FlipCardProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Serilog;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(); 
builder.Services.AddCors(options =>
{ 
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin() 
                .AllowAnyMethod() 
                .AllowAnyHeader(); 
        });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<DataContext>((serviceProvider, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseSettings"));
});

builder.Services.AddScoped<FlipcardRepository>();
builder.Services.AddSingleton<UserTrackingService<int>>();

builder.Services.AddScoped(typeof(GenericValidator<>));

var app = builder.Build();


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