var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddMongoDB("mongo").AddDatabase("db");

builder.AddProject<Projects.Airline_Api_Host>("airline-api-host")
    .WithReference(db, "airlineClient")
    .WaitFor(db);
builder.Build().Run();
