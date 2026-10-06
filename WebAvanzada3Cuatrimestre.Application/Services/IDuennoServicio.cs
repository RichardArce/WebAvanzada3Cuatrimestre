using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAvanzada3Cuatrimestre.Application.Dtos;

namespace WebAvanzada3Cuatrimestre.Application.Services
{
    public interface IDuennoServicio
    {
        Task<List<DuennoDto>> ObtenerDuennosAsync(CancellationToken cancellationToken = default);
        Task<DuennoDto?> ObtenerDuennoPorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Respuesta<DuennoDto>> CrearDuennoAsync(DuennoDto duenno, CancellationToken cancellationToken = default);
        Task<Respuesta<DuennoDto>> ActualizarDuennoAsync(DuennoDto duenno, CancellationToken cancellationToken = default);
        Task<Respuesta<DuennoDto>> EliminarDuennoAsync(int id, CancellationToken cancellationToken = default);
    }
}
