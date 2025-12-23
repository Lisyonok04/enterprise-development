using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddMongoDB("mongo").AddDatabase("db");

builder.AddProject<Projects.Airline_Api_Host>("airline-api-host")
    .WithReference(db, "airlineClient")
    .WaitFor(db);
builder.AddProject<Projects.Airline_Generator_Kafka_Host>("airline-generator-kafka-host");
builder.Build().Run();

