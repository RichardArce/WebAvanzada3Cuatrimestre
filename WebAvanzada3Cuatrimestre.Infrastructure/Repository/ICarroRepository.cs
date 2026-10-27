using WebAvanzada3Cuatrimestre.Domain;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository
{
    // El CRUD viene de IRepository<Carro>; aqui se agregan solo las consultas propias de Carro (p. ej. ExistePlacaAsync)
    public interface ICarroRepository : IRepository<Carro>
    {
    }
}
