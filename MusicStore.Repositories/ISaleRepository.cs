using MusicStore.Entities;
using MusicStore.Entities.Infos;

namespace MusicStore.Repositories;

public interface ISaleRepository
{
    Task<int> CreateSaleAsync(Sale sale);

    Task<SaleInfo> GetSaleInfoAsync(int id);

    Task<ICollection<SaleInfo>> ListSalesAsync(string email, int page, int pageSize);

    Task<ICollection<SaleInfo>> ListSalesAsync(DateTime startDate, DateTime endDate, int page, int pageSize);

    Task<ICollection<ReportInfo>> GetReportSaleAsync(DateTime startDate, DateTime endDate);

}