using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Entities;
using MusicStore.Entities.Infos;

namespace MusicStore.Repositories;

public class ConcertRepository : IConcertRepository
{
    private readonly MusicStoreDbContext _context;

    public ConcertRepository(MusicStoreDbContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<ConcertInfo>> ListAsync(string? filter, int page, int rows)
    {
        return await _context.Set<Concert>()
            .AsNoTracking()
            .Where(p => p.Title.Contains(filter ?? string.Empty) && p.Status)
            .OrderByDescending(p => p.DateEvent)
            .Skip((page - 1) * rows)
            .Take(rows)
            .Select(p => new ConcertInfo
            {
                Id = p.Id,
                Title = p.Title,
                Place = p.Place,
                DateEvent = p.DateEvent.ToString("yyyy-MM-dd"),
                TimeEvent = p.DateEvent.ToString("HH:mm:ss"),
                Genre = p.Genre.Name,
                ImageUrl = p.ImageUrl ?? string.Empty,
                Description = p.Description,
                TicketsQuantity = p.TicketsQuantity,
                UnitPrice = p.UnitPrice,
                Status = p.Status ? "Activo" : "Inactivo"
            })
            .ToListAsync();
    }

    public async Task<Concert?> GetAsync(int id)
    {
        return await _context.Set<Concert>()
            .Include(x => x.Genre)
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<int> AddAsync(Concert concert)
    {
        await _context.Set<Concert>().AddAsync(concert);
        await _context.SaveChangesAsync();

        return concert.Id;
    }

    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<Concert>().FindAsync(id);

        if (entity != null)
        {
            entity.Status = false;
            await _context.SaveChangesAsync();
        }

        throw new InvalidOperationException();
    }

    public async Task FinalizeAsync(int id)
    {
         var entity = await _context.Set<Concert>().FindAsync(id);

        if (entity != null)
        {
            entity.Finalized = true;
            await _context.SaveChangesAsync();
        }

        throw new InvalidOperationException();
    }
}