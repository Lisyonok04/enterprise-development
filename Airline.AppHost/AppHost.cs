using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);
var kafka = builder.AddKafka("kafka");

builder.AddProject<Projects.Airline_Generator_Kafka_Host>("airline-generator")
    .WithReference(kafka);

builder.AddProject<Projects.Airline_Api_Host>("airline-api-host")
    .WithReference(kafka);

builder.Build().Run();
