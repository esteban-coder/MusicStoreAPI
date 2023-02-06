using MusicStore.Entities;

namespace MusicStore.Repositories
{
    public interface IGenreRepository
    {
        Task<ICollection<Genre>> ListAsync();

        Task<ICollection<Genre>> ListGenresAsync(string? filter, int page, int rows);

        Task<Genre?> GetAsync(int id);

        Task<int> AddAsync(Genre genre);

        Task UpdateAsync();
        
        Task DeleteAsync(int id);
    }
}