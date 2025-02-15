using System.Data.Common;
using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

//Add database connection
var dbConnectionString = builder.Configuration.GetConnectionString("MySqlDbConnection");
if(dbConnectionString == null)
{
    throw new Exception("Invalid Connection String");
}

builder.Services.AddDbContext<ApplicationDbContext>(option => 
    option.UseMySql(dbConnectionString, new MySqlServerVersion(new Version(8,4,3))));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
