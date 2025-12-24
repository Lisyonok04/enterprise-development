var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddMongoDB("mongo")
    .AddDatabase("db");

var kafka = builder.AddKafka("airline-kafka", 9092) 
    .WithKafkaUI();

var apiHost = builder.AddProject<Projects.Airline_Api_Host>("airline-api-host")
    .WithReference(db, "airlineClient")
    .WithReference(kafka)
    .WithEnvironment("KAFKA_BOOTSTRAP_SERVERS", kafka.GetEndpoint("tcp"))
    .WithEnvironment("Kafka:TopicName", "airline-contracts")
    .WaitFor(db)
    .WaitFor(kafka);

builder.AddProject<Projects.Airline_Generator_Kafka_Host>("airline-generator-kafka-host")
    .WithReference(kafka)
    .WithEnvironment("KAFKA_BOOTSTRAP_SERVERS", kafka.GetEndpoint("tcp"))
    .WithEnvironment("Kafka:TopicName", "airline-contracts") 
    .WaitFor(kafka)
    .WaitFor(apiHost);

builder.Build().Run();