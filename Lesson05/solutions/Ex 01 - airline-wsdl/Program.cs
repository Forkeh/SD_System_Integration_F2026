using AirlineWsdl.Services;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSoapCore();
builder.Services.AddScoped<IAirlineService, AirlineService>();

var app = builder.Build();

app.UseRouting();

app.UseSoapEndpoint<IAirlineService>("/AirlineService.asmx", new SoapEncoderOptions());

app.Run();
