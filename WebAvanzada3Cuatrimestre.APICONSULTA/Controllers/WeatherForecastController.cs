using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Metadata;

namespace WebAvanzada3Cuatrimestre.APICONSULTA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        /// <summary>
        /// Obtiene el pronóstico del clima para los próximos cinco días.
        /// </summary>
        /// <returns>Una colección de pronósticos con fecha, temperatura y resumen.</returns>
        [HttpGet(Name = "GetWeatherForecast")]
        [EndpointSummary("Obtiene el pronóstico del clima")]
        [EndpointDescription("Devuelve cinco pronósticos generados aleatoriamente para los próximos días.")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), StatusCodes.Status200OK)]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
