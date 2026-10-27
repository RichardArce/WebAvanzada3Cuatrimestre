using WebAvanzada3Cuatrimestre.Domain;
using WebAvanzada3Cuatrimestre.Infrastructure.Data;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository;

// El CRUD lo hereda de Repository<Duenno>. Al eliminar, la BD borra en cascada los carros del duenno (ON DELETE CASCADE)
public class DuennoRepository : Repository<Duenno>, IDuennoRepository
{
    public DuennoRepository(ApplicationDbContext context) : base(context)
    {
    }
}
