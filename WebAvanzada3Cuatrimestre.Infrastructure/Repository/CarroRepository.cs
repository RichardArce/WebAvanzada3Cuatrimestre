using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAvanzada3Cuatrimestre.Infrastructure.Data;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository
{
    public class CarroRepository : ICarroRepository
    {

        private readonly ApplicationDbContext _context;


        public CarroRepository(ApplicationDbContext context) // Inyeccion de depencencia
        {
            _context = context;
        }

        public async Task<List<Carro>> GetAllCarrosAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Carro> GetCarroByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Carro> CreateCarroAsync(Carro carro)
        {
            throw new NotImplementedException();
        }

        public async Task<Carro> UpdateCarroAsync(Carro carro)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteCarroAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
