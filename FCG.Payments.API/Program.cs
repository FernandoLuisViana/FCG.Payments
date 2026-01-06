using FCG.Payments.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
BuilderExtensions.AddProjectServices(builder);

var app = builder.Build();
ApplicationExtensions.UseProjectConfiguration(app);

app.Run();
