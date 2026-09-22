using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository
{
    public interface ICarroRepository //Basico, luego lo cambiamos
    {
        Task<List<Carro>> GetAllCarrosAsync();
        Task<Carro> GetCarroByIdAsync(int id);
        Task<Carro> CreateCarroAsync(Carro carro);
        Task<Carro> UpdateCarroAsync(Carro carro);
        Task<bool> DeleteCarroAsync(int id);
    }
}
//CreateReadUpdateDelete 
