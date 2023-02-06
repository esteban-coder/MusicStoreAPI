using MusicStore.Entities;

namespace MusicStore.Repositories;

public interface ICustomerRepository
{
    Task<int> AddAsync(Customer customer);

    Task<Customer> GetByEmailAsync(string email);
    
}