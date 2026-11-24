using WebAvanzada3Cuatrimestre.MINAPI.Modelos;

namespace WebAvanzada3Cuatrimestre.MINAPI.Servicios;

public sealed class PolizaServicio : IPolizaServicio
{
    private readonly List<PropietariosCarros> _propietarios =
    [
        new("Carlos Rodríguez Méndez", "111"),
        new("María Gómez Vargas", "222"),
        new("José Antonio Ramírez López", "333"),
        new("Ana Lucía Fernández Mora", "444"),
        new("Luis Alberto Sánchez Castro", "555"),
        new("Sofía Martínez Rojas", "666"),
        new("Diego Vargas Hernández", "777"),
        new("Laura Patricia Jiménez Ruiz", "888"),
        new("Andrés Ramírez Solano", "999"),
        new("Valeria Torres Mendoza", "100"),
        new("Miguel Ángel Navarro Pérez", "200"),
        new("Daniela Castro Villalobos", "300"),
        new("Diego Torres Cordero", "400")
    ];

    private readonly List<Poliza> _polizas = [];

    public bool ExistePlaca(string placa)
    {
        return _propietarios.Any(propietario =>
            string.Equals(propietario.Placa, placa, StringComparison.OrdinalIgnoreCase));
    }

    public bool ExisteNumeroPoliza(string numeroPoliza)
    {
        return _polizas.Any(poliza =>
            string.Equals(poliza.numeroPoliza, numeroPoliza, StringComparison.OrdinalIgnoreCase));
    }

    public ResultadoCrearPoliza Crear(Poliza poliza, string placa)
    {
        if (!ExistePlaca(placa))
        {
            return ResultadoCrearPoliza.PlacaNoExiste;
        }

        if (poliza.FechaVencimiento.Date < poliza.FechaContratacion.Date)
        {
            return ResultadoCrearPoliza.FechasInvalidas;
        }

        if (_polizas.Any(polizaExistente =>
            string.Equals(polizaExistente.numeroPoliza, poliza.numeroPoliza, StringComparison.OrdinalIgnoreCase)))
        {
            return ResultadoCrearPoliza.NumeroDuplicado;
        }

        poliza.placa = placa;
        _polizas.Add(poliza);
        return ResultadoCrearPoliza.Creada;
    }

    public IReadOnlyList<Poliza> ObtenerPorPlaca(string placa)
    {
        return _polizas
            .Where(poliza => string.Equals(poliza.placa, placa, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public Poliza? ObtenerPorNumero(string numeroPoliza)
    {
        return _polizas.FirstOrDefault(poliza =>
            string.Equals(poliza.numeroPoliza, numeroPoliza, StringComparison.OrdinalIgnoreCase));
    }
}
