var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Airline_Api_Host>("airline-api-host");

builder.Build().Run();
