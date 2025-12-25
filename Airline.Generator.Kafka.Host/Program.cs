using Airline.Application.Contracts.Flight;
using Airline.Generator.Kafka.Host;
using Airline.Generator.Kafka.Host.Generator;
using Airline.Generator.Kafka.Host.Interfaces;
using Airline.Generator.Kafka.Host.Serializers;
using Airline.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

FlightGenerator.Initialize(builder.Configuration);

builder.AddKafkaProducer<string, IList<CreateFlightDto>>("airline-kafka",
    configureBuilder: kafkaBuilder =>
    {
        kafkaBuilder.SetKeySerializer(new AirlineKeySerializer());
        kafkaBuilder.SetValueSerializer(new AirlineValueSerializer());
    });

builder.Services.AddScoped<IProducerService, AirlineKafkaProducer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var assemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => a.GetName().Name!.StartsWith("Airline"))
        .Distinct();

    foreach (var assembly in assemblies)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath);
    }
});

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