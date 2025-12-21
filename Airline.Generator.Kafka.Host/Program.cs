using Airline.Application.Contracts.Flight;
using Airline.Generator.Kafka.Host;
using Airline.Generator.Kafka.Host.Interface;
using Airline.Generator.Kafka.Host.Serializers;
using Airline.ServiceDefaults;
using Microsoft.AspNetCore.Builder;
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddScoped<IProducerService, AirlineKafkaProducer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();