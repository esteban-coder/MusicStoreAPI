using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;
using MusicStore.Services.Interfaces;

namespace MusicStore.Services.Implementations
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _repository;
        private readonly ILogger<GenreService> _logger;
        private readonly IMapper _mapper;

        public GenreService(IGenreRepository repository, ILogger<GenreService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BaseResponseGeneric<IEnumerable<GenreDtoResponse>>> ListAsync()
        {
            var response = new BaseResponseGeneric<IEnumerable<GenreDtoResponse>>();

            try
            {
                response.Data = _mapper.Map<IEnumerable<GenreDtoResponse>>(await _repository.ListAsync());

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Hubo un error al obtener los registros";

                _logger.LogError(ex, ex.Message);
            }

            return response;
        }

        public async Task<BaseResponseGeneric<IEnumerable<GenreDtoResponse>>> ListGenresAsync(string? filter, int page, int rows)
        {
            var response = new BaseResponseGeneric<IEnumerable<GenreDtoResponse>>();

            try
            {
                response.Data = _mapper.Map<IEnumerable<GenreDtoResponse>>(await _repository.ListGenresAsync(filter, page, rows));

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Hubo un error al obtener los registros";

                _logger.LogError(ex, ex.Message);
            }

            return response;
        }

        public async Task<BaseResponseGeneric<GenreDtoResponse>> GetAsync(int id)
        {
            var response = new BaseResponseGeneric<GenreDtoResponse>();

            try
            {
                var genre = await _repository.GetAsync(id);

                if (genre == null)
                {
                    response.Success = false;
                    response.ErrorMessage = "No se encontró el registro";
                }
                else
                {
                    response.Data = _mapper.Map<GenreDtoResponse>(genre);
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Hubo un error al obtener el registro";

                _logger.LogError(ex, ex.Message);
            }

            return response;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(GenreDtoRequest request)
        {
            var response = new BaseResponseGeneric<int>();

            try
            {
                var entity = _mapper.Map<Genre>(request);

                await _repository.AddAsync(entity);

                response.Data = entity.Id;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Hubo un error al agregar el registro";

                _logger.LogError(ex, ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse> UpdateAsync(int id, GenreDtoRequest request)
        {
            var response = new BaseResponse();

            try
            {
                var entity = await _repository.GetAsync(id);

                if (entity == null)
                {
                    response.Success = false;
                    response.ErrorMessage = "No se encontró el registro";
                }
                else
                {
                    // origen y luego destino
                    _mapper.Map(request, entity);

                    await _repository.UpdateAsync();

                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Hubo un error al actualizar el registro";

                _logger.LogError(ex, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var response = new BaseResponse();

            try
            {
                var entity = await _repository.GetAsync(id);

                if (entity == null)
                {
                    response.Success = false;
                    response.ErrorMessage = "No se encontró el registro";
                }
                else
                {
                    await _repository.DeleteAsync(id);

                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "Hubo un error al eliminar el registro";

                _logger.LogError(ex, ex.Message);
            }

            return response;
        }
    }
}