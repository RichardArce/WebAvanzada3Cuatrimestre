using System;
using System.Collections.Generic;
using System.Text;
using WebAvanzada3Cuatrimestre.Application.Dtos;

namespace WebAvanzada3Cuatrimestre.Application.Services
{
    public interface ICarroServicio
    {
        Task<List<CarroDto>> ObtenerCarrosAsync(CancellationToken cancellationToken = default);
        Task<CarroDto?> ObtenerCarroPorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Respuesta<CarroDto>> CrearCarroAsync(CarroDto carro, CancellationToken cancellationToken = default);
        Task<Respuesta<CarroDto>> ActualizarCarroAsync(CarroDto carro, CancellationToken cancellationToken = default);
        Task<Respuesta<CarroDto>> EliminarCarroAsync(int id, CancellationToken cancellationToken = default);
    }
}
