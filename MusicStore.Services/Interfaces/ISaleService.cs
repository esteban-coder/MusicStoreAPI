using MusicStore.Dto.Request;
using MusicStore.Dto.Response;

namespace MusicStore.Services.Interfaces;

public interface ISaleService
{
    Task<BaseResponseGeneric<int>> CreateSaleAsync(string email, SaleDtoRequest request);

    Task<BaseResponseGeneric<SaleDtoResponse>> GetSaleAsync(int id);

    Task<BaseResponseGeneric<ICollection<SaleDtoResponse>>> ListSalesAsync(string email, int page, int pageSize);

    Task<BaseResponseGeneric<ICollection<SaleDtoResponse>>> ListSalesAsync(DateTime startDate, DateTime endDate, int page, int pageSize);

    Task<BaseResponseGeneric<ICollection<ReportDtoResponse>>> GetReportSaleAsync(DateTime startDate, DateTime endDate);
}