using WebAvanzada3Cuatrimestre.MINAPI.Modelos;
using WebAvanzada3Cuatrimestre.MINAPI.Servicios;

namespace WebAvanzada3Cuatrimestre.MINAPI.Endpoints;

public static class PolizaEndpoints
{
    public static IEndpointRouteBuilder MapPolizaEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/polizas/{placa}", CrearPoliza)
            .WithName("CrearPoliza")
            .WithSummary("Crea una póliza para un carro cuyo propietario está registrado")
            .WithTags("Pólizas")
            .Produces<Poliza>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .Produces<string>(StatusCodes.Status404NotFound)
            .Produces<string>(StatusCodes.Status409Conflict);

        endpoints.MapGet("/api/carros/{placa}/polizas", ObtenerPolizasPorPlaca)
            .WithName("ObtenerPolizasPorPlaca")
            .WithSummary("Obtiene las pólizas asociadas a una placa")
            .WithTags("Pólizas")
            .Produces<IReadOnlyList<Poliza>>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status404NotFound);

        endpoints.MapGet("/api/polizas/{numeroPoliza}/vencimiento", VerificarVencimiento)
            .WithName("VerificarVencimientoPoliza")
            .WithSummary("Verifica si una póliza está vencida")
            .WithTags("Pólizas")
            .Produces<bool>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status404NotFound);

        return endpoints;
    }

    private static IResult CrearPoliza( //Esta mal por que esta extenso pero tampoco es algo del otro mundo.
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

        var resultado = servicio.Crear(poliza, placa);

        if (resultado == ResultadoCrearPoliza.PlacaNoExiste)
        {
            return Results.NotFound($"No existe un propietario registrado para la placa {placa}.");
        }

        if (resultado == ResultadoCrearPoliza.FechasInvalidas)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["FechaVencimiento"] = ["La fecha de vencimiento debe ser igual o posterior a la fecha de contratación."]
            });
        }

        if (resultado == ResultadoCrearPoliza.NumeroDuplicado)
        {
            return Results.Conflict($"Ya existe la póliza {poliza.numeroPoliza}.");
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
        IPolizaServicio servicio,
        IReloj reloj)//por que una interfaz?
    {
        var poliza = servicio.ObtenerPorNumero(numeroPoliza);

        if (poliza is null)
        {
            return Results.NotFound($"No existe la póliza {numeroPoliza}.");
        }

        return Results.Ok(
            poliza.FechaVencimiento.Date < reloj.Hoy);
    }
}
