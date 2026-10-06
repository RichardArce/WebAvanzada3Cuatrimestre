using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAvanzada3Cuatrimestre.Domain;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository;

// PRincipio SOLID, I segregacion de interfaz por que tengo separados las interfaces de Carro, de Duenno, y sus servicio
public interface IDuennoRepository
{
    Task<List<Duenno>> GetAllDuennosAsync(CancellationToken cancellationToken = default);
    Task<Duenno?> GetDuennoByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> CreateDuennoAsync(Duenno duenno, CancellationToken cancellationToken = default);
    Task<bool> UpdateDuennoAsync(Duenno duenno, CancellationToken cancellationToken = default);
    Task<bool> DeleteDuennoAsync(int id, CancellationToken cancellationToken = default);
}
