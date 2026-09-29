using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAvanzada3Cuatrimestre.Domain;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository
{
    public interface ICarroRepository //Basico, luego lo cambiamos
    {
        Task<List<Carro>> GetAllCarrosAsync(CancellationToken cancellationToken = default);
        Task<Carro> GetCarroByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> CreateCarroAsync(Carro carro, CancellationToken cancellationToken = default);
        Task<bool> UpdateCarroAsync(Carro carro, CancellationToken cancellationToken = default);
        Task<bool> DeleteCarroAsync(int id, CancellationToken cancellationToken = default);
    }
}
//CreateReadUpdateDelete 
