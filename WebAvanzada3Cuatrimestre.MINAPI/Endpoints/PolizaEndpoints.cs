using WebAvanzada3Cuatrimestre.MINAPI.Modelos;
using WebAvanzada3Cuatrimestre.MINAPI.Servicios;

namespace WebAvanzada3Cuatrimestre.MINAPI.Endpoints;

public static class PolizaEndpoints
{
    public static IEndpointRouteBuilder MapPolizaEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/polizas/{placa}", CrearPoliza) // al final de la ruta se puede poner el parametro que se va a recibir, en este caso la placa
            .WithName("CrearPoliza")
            .WithSummary("Crea una póliza para un carro cuyo propietario está registrado");

        endpoints.MapGet("/api/carros/{placa}/polizas", ObtenerPolizasPorPlaca)
            .WithName("ObtenerPolizasPorPlaca")
            .WithSummary("Obtiene las pólizas asociadas a una placa");

        endpoints.MapGet("/api/polizas/{numeroPoliza}/vencimiento", VerificarVencimiento)
            .WithName("VerificarVencimientoPoliza")
            .WithSummary("Verifica si una póliza está vencida");

        return endpoints;
    }

    private static IResult CrearPoliza(
        string placa,
        Poliza poliza,
        IPolizaServicio servicio)
    {
        if (string.IsNullOrWhiteSpace(placa) ||
            string.IsNullOrWhiteSpace(poliza.codigoPoliza) ||
            string.IsNullOrWhiteSpace(poliza.numeroPoliza))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["poliza"] = ["La placa, el código y el número de póliza son obligatorios."]
            });
        }

        if (!servicio.ExistePlaca(placa))
        {
            return Results.NotFound($"No existe un propietario registrado para la placa {placa}.");
        }

        if (poliza.FechaVencimiento.Date < poliza.FechaContratacion.Date)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["FechaVencimiento"] = ["La fecha de vencimiento debe ser igual o posterior a la fecha de contratación."]
            });
        }

        if (servicio.ExisteNumeroPoliza(poliza.numeroPoliza))
        {
            return Results.Conflict($"Ya existe la póliza {poliza.numeroPoliza}.");
        }

        poliza.placa = placa;

        if (!servicio.Agregar(poliza))
        {
            return Results.Conflict("No fue posible crear la póliza porque sus datos no son válidos o el número ya existe.");
        }

        return Results.Created($"/api/polizas/{poliza.numeroPoliza}/vencimiento", poliza);
    }

    private static IResult ObtenerPolizasPorPlaca(
        string placa,
        IPolizaServicio servicio)
    {
        if (!servicio.ExistePlaca(placa))
        {
            return Results.NotFound($"No existe un propietario registrado para la placa {placa}.");
        }

        return Results.Ok(servicio.ObtenerPorPlaca(placa));
    }

    private static IResult VerificarVencimiento(
        string numeroPoliza,
        IPolizaServicio servicio)
    {
        var poliza = servicio.ObtenerPorNumero(numeroPoliza);

        if (poliza is null)
        {
            return Results.NotFound($"No existe la póliza {numeroPoliza}.");
        }

        return Results.Ok(
            poliza.FechaVencimiento.Date < DateTime.Now.Date);
    }
}

//Aunque no tengamos controladores, necesitamos una estructura o clase que me permite manejar las peticion