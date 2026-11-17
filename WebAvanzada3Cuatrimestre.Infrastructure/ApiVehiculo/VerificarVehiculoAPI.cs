using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace WebAvanzada3Cuatrimestre.Infrastructure.ApiVehiculo
{
    public class VerificarVehiculoAPI : IVerificarVehiculoAPI
    {
        private readonly HttpClient _httpClient;

        public VerificarVehiculoAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<bool> EsPropietarioAsync(string cliente, string placa, CancellationToken cancellationToken = default)
        {
            var url = $"https://localhost:7065/api/VerificarVehiculo?cliente={cliente}&placa={placa}";
            //var url = $"https://localhost:7065/api/VerificarVehiculo?cliente={Uri.EscapeDataString(cliente)}&placa={Uri.EscapeDataString(placa)}";

            //Puede configurarse en appsettings.json y luego inyectarse en el constructor de la clase, para no tenerlo hardcodeado


            var response = await _httpClient.GetAsync(url, cancellationToken);

            if(!response.IsSuccessStatusCode)
            {
               return false; // Cambia esto según tu lógica de manejo de errores
            }

            var result = await response.Content.ReadFromJsonAsync<VerificarVehiculoResult>(); //.Result es una mala practica
            return result.data;
        }
    }
}
