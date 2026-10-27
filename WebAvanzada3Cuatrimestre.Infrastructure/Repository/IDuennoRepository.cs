using WebAvanzada3Cuatrimestre.Domain;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository;

// PRincipio SOLID, I segregacion de interfaz por que tengo separados las interfaces de Carro, de Duenno, y sus servicio
// El CRUD viene de IRepository<Duenno>; aqui se agregan solo las consultas propias de Duenno
public interface IDuennoRepository : IRepository<Duenno>
{
}
