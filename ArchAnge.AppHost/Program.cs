var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Archange_Front_Server>("archange-front-server");
var app = builder.Build();

await app.RunAsync();
