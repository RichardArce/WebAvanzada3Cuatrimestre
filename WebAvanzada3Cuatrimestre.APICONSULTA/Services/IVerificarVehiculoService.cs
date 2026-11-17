namespace WebAvanzada3Cuatrimestre.APICONSULTA.Services
{
    public interface IVerificarVehiculoService
    {
        bool esPropietario(string cliente, string placa);

        //Utilizar una simulacion de capas
        //Mejor manero de las respuestas es utilizar un objeto de respuesta, que contenga un mensaje y un estado de exito o fracaso
    }
}
