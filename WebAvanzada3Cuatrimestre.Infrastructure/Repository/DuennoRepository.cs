using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAvanzada3Cuatrimestre.Domain;
using WebAvanzada3Cuatrimestre.Infrastructure.Data;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository;

public class DuennoRepository : IDuennoRepository
{
    private readonly ApplicationDbContext _context;

    public DuennoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Duenno>> GetAllDuennosAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Duennos
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Duenno?> GetDuennoByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            return new Duenno();
        }

        return await _context.Duennos
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<bool> CreateDuennoAsync(
        Duenno duenno,
        CancellationToken cancellationToken = default)
    {
        await _context.Duennos.AddAsync(duenno, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdateDuennoAsync(
        Duenno duenno,
        CancellationToken cancellationToken = default)
    {
        var duennoExistente = await _context.Duennos
            .FirstOrDefaultAsync(d => d.Id == duenno.Id, cancellationToken);

        if (duennoExistente is null)
        {
            return false;
        }

        duennoExistente.Nombre = duenno.Nombre;
        duennoExistente.Edad = duenno.Edad;
        duennoExistente.Apellido1 = duenno.Apellido1;
        duennoExistente.Apellido2 = duenno.Apellido2;

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteDuennoAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var duenno = await _context.Duennos
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (duenno is null)
        {
            return false;
        }

        _context.Duennos.Remove(duenno);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
