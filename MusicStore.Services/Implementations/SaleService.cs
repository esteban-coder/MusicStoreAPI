using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;
using MusicStore.Services.Interfaces;

namespace MusicStore.Services.Implementations;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _repository;
    private readonly IConcertRepository _concertRepository;
    private readonly ILogger<SaleService> _logger;
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _customerRepository;

    public SaleService(ISaleRepository repository, 
        IConcertRepository concertRepository, 
        ILogger<SaleService> logger, 
        IMapper mapper, 
        ICustomerRepository customerRepository)
    {
        _repository = repository;
        _concertRepository = concertRepository;
        _logger = logger;
        _mapper = mapper;
        _customerRepository = customerRepository;
    }

    public async Task<BaseResponseGeneric<int>> CreateSaleAsync(string email, SaleDtoRequest request)
    {

        var response = new BaseResponseGeneric<int>();
        
        try
        {
            var concert = await _concertRepository.GetAsync(request.ConcertId);

            if (concert == null)
                throw new Exception("No se encontró el concierto");

            if (concert.Finalized)
                throw new Exception("El concierto ya fue finalizado");

            if (concert.DateEvent <= DateTime.Now)
                throw new Exception("El concierto ya se realizo");

            // Obtenemos el registro de customer por el Email.
            var customer = await _customerRepository.GetByEmailAsync(email);

            var sale = new Sale
            {
                CustomerForeignKey = customer.Id,
                ConcertId = request.ConcertId,
                Quantity = request.TicketsQuantity,
            };

            sale.Total = sale.Quantity * concert.UnitPrice;

            response.Data = await _repository.CreateSaleAsync(sale);

            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la venta {Message}", ex.Message);
            response.ErrorMessage = ex.Message;
        }

        return response;

    }

    public async Task<BaseResponseGeneric<SaleDtoResponse>> GetSaleAsync(int id)
    {
        var response = new BaseResponseGeneric<SaleDtoResponse>();

        try
        {
            var sale = await _repository.GetSaleInfoAsync(id);

            //if (sale == null)
            //{
            //    response.ErrorMessage = "No se encontró la venta";
            //    return response;
            //}

            response.Data = _mapper.Map<SaleDtoResponse>(sale);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la venta");
            response.ErrorMessage = "Error al obtener la venta";
        }

        return response;

    }

    public async Task<BaseResponseGeneric<ICollection<SaleDtoResponse>>> ListSalesAsync(string email, int page, int pageSize)
    {
        var response = new BaseResponseGeneric<ICollection<SaleDtoResponse>>();

        try
        {
            var sales =await _repository.ListSalesAsync(email, page, pageSize);

            response.Data = _mapper.Map<ICollection<SaleDtoResponse>>(sales);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las ventas");
            response.ErrorMessage = "Error al obtener las ventas";
        }

        return response;
    }

    public async Task<BaseResponseGeneric<ICollection<SaleDtoResponse>>> ListSalesAsync(DateTime startDate, DateTime endDate, int page, int pageSize)
    {
        var response = new BaseResponseGeneric<ICollection<SaleDtoResponse>>();

        try
        {
            var sales = await _repository.ListSalesAsync(startDate, endDate, page, pageSize);

            response.Data = _mapper.Map<ICollection<SaleDtoResponse>>(sales);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las ventas");
            response.ErrorMessage = "Error al obtener las ventas";
        }

        return response;
    }

    public async Task<BaseResponseGeneric<ICollection<ReportDtoResponse>>> GetReportSaleAsync(DateTime startDate, DateTime endDate)
    {
        var response = new BaseResponseGeneric<ICollection<ReportDtoResponse>>();

        try
        {
            var sales = await _repository.GetReportSaleAsync(startDate, endDate);

            response.Data = _mapper.Map<ICollection<ReportDtoResponse>>(sales);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el reporte de ventas");
            response.ErrorMessage = "Error al obtener el reporte de ventas";
        }

        return response;
    }
}