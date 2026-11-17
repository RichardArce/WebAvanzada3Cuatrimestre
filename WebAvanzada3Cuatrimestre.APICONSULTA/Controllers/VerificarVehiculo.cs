using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAvanzada3Cuatrimestre.APICONSULTA.Services;

namespace WebAvanzada3Cuatrimestre.APICONSULTA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificarVehiculo : ControllerBase
    {
        private readonly IVerificarVehiculoService _verificarVehiculoService;

        public VerificarVehiculo(IVerificarVehiculoService verificacionVehiculoService)
        {
            _verificarVehiculoService = verificacionVehiculoService;
        }


        [HttpGet(Name = "VerificarVehiculo")]
        [EndpointSummary("Verifica si el cliente posee el vehiculo")]
        [EndpointDescription("Devuelve verdadero o falso si el vehiculo si le pertenece al cliente")]
        [Produces("application/json")]
        public async Task<ActionResult<RespuestaVehiculo>> VerificarPropiedadVehiculo(string cliente, string placa) { 
        

            var esPropietario = _verificarVehiculoService.esPropietario(cliente, placa);

            var respuesta = new RespuestaVehiculo //Esto podria quedar aca? Si por que es interpretar la respuesta de la api y devolverla al cliente. Tambien podria estar en el servicio, pero es mejor que quede aca para que el servicio solo se encargue de la logica de negocio y no de la interpretacion de la respuesta.
            {
                statusCode = StatusCodes.Status200OK,
                Message = "Vehiculo verificado correctamente",
                Data = esPropietario
            };


            return Ok(respuesta); // Devuelve la respuesta con el código de estado 200 y el objeto RespuestaVehiculo
        }


    }
}
