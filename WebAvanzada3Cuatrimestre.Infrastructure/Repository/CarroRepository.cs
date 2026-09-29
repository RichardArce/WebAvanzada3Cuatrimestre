using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAvanzada3Cuatrimestre.Domain;
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

        public async Task<List<Carro>> GetAllCarrosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Carros
                .AsNoTracking()
                .Include(c => c.FkduennoNavigation)
                .ToListAsync(cancellationToken);

        }

        public async Task<Carro> GetCarroByIdAsync(int id, CancellationToken cancellationToken = default)
        {

            //Que pasa si el carro no existe? Que pasa si el id es null? Que pasa si el id es negativo?

            //Es correcto validar el id antes de hacer la consulta?  Debemos evitar doble validacion

            var carro = await _context.Carros.FindAsync(id, cancellationToken);

            if (carro == null)
            {
                return new Carro(); // Retornamos un objeto vacio para evitar null reference exception
            }
                

            return carro;
        }

        public async Task<bool> CreateCarroAsync(Carro carro, CancellationToken cancellationToken = default)
        {
            _context.Carros.Add(carro);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
            
        }

        public async Task<bool> UpdateCarroAsync(Carro carro, CancellationToken cancellationToken = default)
        {
            var carroExistente = await _context.Carros
                .FirstOrDefaultAsync(c => c.Id == carro.Id, cancellationToken);

            if (carroExistente is null)
            {
                return false; 
            }

            carroExistente.Placa = carro.Placa;
            carroExistente.Marca = carro.Marca;
            carroExistente.Fkduenno = carro.Fkduenno;

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteCarroAsync(int id, CancellationToken cancellationToken = default)// Devuelve true si se elimino correctamente, false si no se encontro el carro o fallo
        {
            var carro = await _context.Carros.FindAsync(id, cancellationToken);
            if (carro == null)
            {
                return false;
            }

            _context.Carros.Remove(carro);
            return await _context.SaveChangesAsync(cancellationToken) > 0; // Leer el resultado de SaveChangesAsync para determinar si se elimino correctamente

        }
    }
    //NOTAS:
    //Programacion no es tan blanco o negro, hay que pensar en el contexto y en la experiencia del usuario. En este caso, si el carro no existe, es mejor retornar un objeto vacio para evitar errores en la aplicacion y permitir al usuario saber que el carro no existe.
    //Las consultas pueden devolver los objetovs, y las operaciones si deben devolver un booleano para indicar si se realizo correctamente o no, para que el servicio pueda manejar la logica de negocio y retornar el resultado adecuado al usuario.


    //Capa de repositorio tiene bastantes buenas practicas
}
