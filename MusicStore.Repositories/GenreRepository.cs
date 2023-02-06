using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Entities;

namespace MusicStore.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly MusicStoreDbContext _context;

    public GenreRepository(MusicStoreDbContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<Genre>> ListAsync()
    {
        return await _context.Set<Genre>()
            .Where(p => p.Status)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<Genre>> ListGenresAsync(string? filter, int page, int rows)
    {
        return await _context.Set<Genre>()
            .Where(p => p.Name.Contains(filter ?? string.Empty) && p.Status)
            .OrderBy(p => p.Name)
            .Skip((page - 1) * rows)
            .Take(rows)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Genre?> GetAsync(int id)
    {
        return await _context.Set<Genre>()
            .FindAsync(id);
    }

    public async Task<int> AddAsync(Genre genre)
    {
        _context.Set<Genre>().Add(genre);
        await _context.SaveChangesAsync();
        return genre.Id;
    }

    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var genre = await _context.Set<Genre>().FindAsync(id);

        if (genre != null)
        {
            genre.Status = false;
            await _context.SaveChangesAsync();
        }
    }
}