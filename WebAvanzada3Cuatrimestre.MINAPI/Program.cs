using WebAvanzada3Cuatrimestre.MINAPI.Endpoints;
using WebAvanzada3Cuatrimestre.MINAPI.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IPolizaServicio, PolizaServicio>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPolizaEndpoints(); //Encapsular la definición de los endpoints en una clase estática para mantener el Program.cs limpio y organizado


app.Run();