using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Entities;
using MusicStore.Entities.Infos;

namespace MusicStore.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly MusicStoreDbContext _context;

    public SaleRepository(MusicStoreDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateSaleAsync(Sale sale)
    {
        sale.SaleDate = DateTime.Now;
        var lastNumber = await _context.Set<Sale>().CountAsync() + 1;
        sale.OperationNumber = $"{lastNumber:000000}";

        _context.Set<Sale>().Add(sale);
        await _context.SaveChangesAsync();
        return sale.Id;
    }

    public async Task<SaleInfo> GetSaleInfoAsync(int id)
    {
        return await _context.Set<Sale>()
            .Where(s => s.Id == id)
            .AsNoTracking()
            .Select(s => new SaleInfo
            {
                SaleId = s.Id,
                DateEvent = s.Concert.DateEvent,
                Genre = s.Concert.Genre.Name,
                ImageUrl = s.Concert.ImageUrl,
                Title = s.Concert.Title,
                OperationNumber = s.OperationNumber,
                FullName = s.Customer.FullName,
                Quantity = s.Quantity,
                SaleDate = s.SaleDate,
                Total = s.Total
            })
            .FirstAsync();
    }

    public async Task<ICollection<SaleInfo>> ListSalesAsync(string email, int page, int pageSize)
    {
        var query = await _context.Set<Sale>()
            .Where(p => p.Customer.Email == email && p.Status)
            .AsNoTracking()
            .Select(s => new SaleInfo
            {
                SaleId = s.Id,
                DateEvent = s.Concert.DateEvent,
                Genre = s.Concert.Genre.Name,
                ImageUrl = s.Concert.ImageUrl,
                Title = s.Concert.Title,
                OperationNumber = s.OperationNumber,
                FullName = s.Customer.FullName,
                Quantity = s.Quantity,
                SaleDate = s.SaleDate,
                Total = s.Total
            })
            .OrderBy(s => s.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return query;
    }

    public async Task<ICollection<SaleInfo>> ListSalesAsync(DateTime startDate, DateTime endDate, int page, int pageSize)
    {
        var query = _context.Set<SaleInfo>()
            .FromSqlRaw("EXEC uspListSales {0},{1},{2},{3}", startDate, endDate, (page - 1) * pageSize, pageSize);

        return await query.ToListAsync();
    }

    public async Task<ICollection<ReportInfo>> GetReportSaleAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Set<Sale>()
            .Include(p => p.Concert)
            .Where(x => x.SaleDate >= startDate && x.SaleDate <= endDate)
            .GroupBy(x => x.Concert.Title)
            .Select(p => new ReportInfo
            {
                ConcertName = p.Key,
                TotalAmount = p.Sum(x => x.Total)
            })
            .AsNoTracking()
            .ToListAsync();
    }
}