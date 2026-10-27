using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebAvanzada3Cuatrimestre.Domain;
using WebAvanzada3Cuatrimestre.Infrastructure.Data;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository
{
    // El CRUD lo hereda de Repository<Carro>; solo se define lo que es propio de Carro
    public class CarroRepository : Repository<Carro>, ICarroRepository
    {
        public CarroRepository(ApplicationDbContext context) : base(context) // Inyeccion de depencencia
        {
        }

        /// <summary>
        /// Todas las lecturas de carros incluyen a su duenno, para mostrarlo en la tabla.
        /// </summary>
        /// <returns>Consulta base con el Include del duenno.</returns>
        protected override IQueryable<Carro> Consulta()
        {
            return base.Consulta().Include(c => c.FkduennoNavigation);
        }
    }
    //NOTAS:
    //Las consultas pueden devolver los objetovs, y las operaciones si deben devolver un booleano para indicar si se realizo correctamente o no, para que el servicio pueda manejar la logica de negocio y retornar el resultado adecuado al usuario.
}
