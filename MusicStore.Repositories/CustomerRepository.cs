using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Entities;

namespace MusicStore.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly MusicStoreDbContext _context;

    public CustomerRepository(MusicStoreDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> AddAsync(Customer customer)
    {
        await _context.Set<Customer>().AddAsync(customer);
        await _context.SaveChangesAsync();
        return customer.Id;
    }

    public async Task<Customer> GetByEmailAsync(string email)
    {
        return await _context.Set<Customer>()
            .AsNoTracking()
            .FirstAsync(x => x.Email == email);
    }
}