using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo-airline")
    .AddDatabase("AirlineDb");

var kafka = builder.AddKafka("airline-kafka")
    .WithKafkaUI();

var apiHost = builder.AddProject<Projects.Airline_Api_Host>("airline-api-host")
    .WithReference(mongo, "airlineDb")
    .WithReference(kafka)
    .WithEnvironment("KAFKA_BOOTSTRAP_SERVERS", kafka.GetEndpoint("tcp"))
    .WithEnvironment("Kafka:TopicName", "airline-contracts")
    .WaitFor(mongo)
    .WaitFor(kafka);

builder.AddProject<Projects.Airline_Generator_Kafka_Host>("airline-generator-kafka-host")
    .WithReference(kafka)
    .WithEnvironment("KAFKA_BOOTSTRAP_SERVERS", kafka.GetEndpoint("tcp"))
    .WithEnvironment("Kafka:TopicName", "airline-contracts") 
    .WaitFor(kafka)
    .WaitFor(apiHost);

builder.Build().Run();