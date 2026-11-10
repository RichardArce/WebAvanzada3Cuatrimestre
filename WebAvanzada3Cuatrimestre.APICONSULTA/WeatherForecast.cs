using System.Globalization;

namespace WebAvanzada3Cuatrimestre.APICONSULTA
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }

    public class VehiculoRegistroCivil
    {
        public string Placa { get; set; } = null!;
        public string NombreDueno { get; set; }
    }


    public class RespuestaVehiculo {
        public int statusCode { get; set; }
        
        public string Message { get; set; }

        public object Data { get; set; }

    }
}
