using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAvanzada3Cuatrimestre.APICONSULTA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificarVehiculo : ControllerBase
    {

        private List<VehiculoRegistroCivil> listado = new List<VehiculoRegistroCivil>
        {
            new VehiculoRegistroCivil{  NombreDueno = "Carlos Rodríguez Méndez", Placa= "111"},
            new VehiculoRegistroCivil{  NombreDueno = "María Gómez Vargas", Placa= "222" },
            new VehiculoRegistroCivil{  NombreDueno = "José Antonio Ramírez López", Placa= "333" },
            new VehiculoRegistroCivil{  NombreDueno = "Ana Lucía Fernández Mora", Placa= "444" },
            new VehiculoRegistroCivil{  NombreDueno = "Luis Alberto Sánchez Castro", Placa= "555" },
            new VehiculoRegistroCivil{  NombreDueno = "Sofía Martínez Rojas", Placa= "666" },
            new VehiculoRegistroCivil{  NombreDueno = "Diego Vargas Hernández", Placa= "777" },
            new VehiculoRegistroCivil{  NombreDueno = "Laura Patricia Jiménez Ruiz", Placa= "888" },
            new VehiculoRegistroCivil{  NombreDueno = "Andrés Ramírez Solano", Placa= "999" },
            new VehiculoRegistroCivil{  NombreDueno = "Valeria Torres Mendoza", Placa= "100" },
            new VehiculoRegistroCivil{  NombreDueno = "Miguel Ángel Navarro Pérez", Placa= "200" },
            new VehiculoRegistroCivil{  NombreDueno = "Daniela Castro Villalobos", Placa= "300" },
            new VehiculoRegistroCivil{  NombreDueno = "Fernando Arias González", Placa= "400" }

        };


        [HttpGet(Name = "VerificarVehiculo")]
        [EndpointSummary("Verifica si el cliente posee el vehiculo")]
        [EndpointDescription("Devuelve verdadero o falso si el vehiculo si le pertenece al cliente")]
        [Produces("application/json")]
        public async Task<ActionResult<RespuestaVehiculo>> VerificarPropiedadVehiculo(string cliente, string placa) { 
        
            var response = new RespuestaVehiculo();


            var vehiculoEncontrado = listado.FirstOrDefault(vehiculo =>
                string.Equals(vehiculo.NombreDueno, cliente, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(vehiculo.Placa, placa, StringComparison.OrdinalIgnoreCase));


            response.statusCode = 410;
            response.Message = "Vehículo verificado correctamente";
            response.Data = vehiculoEncontrado != null;

            return Ok(response); // Devuelve la respuesta con el código de estado 200 y el objeto RespuestaVehiculo
        }


    }
}
