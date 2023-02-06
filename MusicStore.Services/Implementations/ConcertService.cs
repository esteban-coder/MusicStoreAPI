using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;
using MusicStore.Services.Interfaces;

namespace MusicStore.Services.Implementations;

public class ConcertService : IConcertService
{
    private readonly IConcertRepository _concertRepository;
    private readonly ILogger<ConcertService> _logger;
    private readonly IMapper _mapper;
    private readonly IFileUploader _fileUploader;

    public ConcertService(IConcertRepository concertRepository, ILogger<ConcertService> logger, IMapper mapper, IFileUploader fileUploader)
    {
        _concertRepository = concertRepository;
        _logger = logger;
        _mapper = mapper;
        _fileUploader = fileUploader;
    }

    public async Task<BaseResponseGeneric<ICollection<ConcertDtoResponse>>> ListAsync(string? filter, int page, int rows)
    {
        var response = new BaseResponseGeneric<ICollection<ConcertDtoResponse>>();

        try
        {
            var concerts = await _concertRepository.ListAsync(filter, page, rows);

            response.Data = _mapper.Map<ICollection<ConcertDtoResponse>>(concerts);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al listar los conciertos");
            response.ErrorMessage = "Error al listar los conciertos";
        }

        return response;

    }

    public async Task<BaseResponseGeneric<ConcertSingleDtoResponse>> GetAsync(int id)
    {
        var response = new BaseResponseGeneric<ConcertSingleDtoResponse>();

        try
        {
            var concert = await _concertRepository.GetAsync(id);

            if (concert == null)
            {
                response.ErrorMessage = "No se encontró el concierto";
                return response;
            }

            response.Data = _mapper.Map<ConcertSingleDtoResponse>(concert);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el concierto");
            response.ErrorMessage = "Error al obtener el concierto";
        }

        return response;
    }

    public async Task<BaseResponseGeneric<int>> AddAsync(ConcertDtoRequest request)
    {
        var response = new BaseResponseGeneric<int>();

        try
        {
            var entity = _mapper.Map<Concert>(request);

            entity.ImageUrl = await _fileUploader.UploadFileAsync(request.Base64Image, request.FileName);

            response.Data = await _concertRepository.AddAsync(entity);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar el concierto");
            response.ErrorMessage = "Error al agregar el concierto";
        }

        return response;
    }

    public async Task<BaseResponse> UpdateAsync(int id, ConcertDtoRequest request)
    {
        var response = new BaseResponse();

        try
        {
            var entity = await _concertRepository.GetAsync(id);

            if (entity == null)
            {
                response.ErrorMessage = "No se encontró el concierto";
                return response;
            }

            _mapper.Map(request, entity);

            if (!string.IsNullOrEmpty(request.FileName))
                entity.ImageUrl = await _fileUploader.UploadFileAsync(request.Base64Image, request.FileName);

            await _concertRepository.UpdateAsync();
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el concierto");
            response.ErrorMessage = "Error al actualizar el concierto";
        }

        return response;
    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        var response = new BaseResponse();

        try
        {
            await _concertRepository.DeleteAsync(id);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el concierto");
            response.ErrorMessage = "Error al eliminar el concierto";
        }

        return response;
    }

    public async Task<BaseResponse> FinalizeAsync(int id)
    {
        var response = new BaseResponse();

        try
        {
            await _concertRepository.FinalizeAsync(id);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al finalizar el concierto");
            response.ErrorMessage = "Error al finalizar el concierto";
        }

        return response;
    }
}