using System;
using System.Collections.Generic;
using System.Text;

namespace WebAvanzada3Cuatrimestre.Infrastructure.ApiVehiculo
{
    public interface IVerificarVehiculoAPI
    {
        Task<bool> EsPropietarioAsync(string cliente, string placa, CancellationToken cancellationToken = default);

    }
}

//Buenos practicas que se aplicaron

//Inversion de dependencias, utilizando HTTPClientFactory(Program.cs linea 23)
//Separacion de responsabilidad, separar la consulta y moverla a infraestructura
//Configuracion de URLS por medio de appsettings.json(P)