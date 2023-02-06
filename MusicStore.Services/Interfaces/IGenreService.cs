using MusicStore.Dto.Request;
using MusicStore.Dto.Response;

namespace MusicStore.Services.Interfaces;

public interface IGenreService
{
    Task<BaseResponseGeneric<IEnumerable<GenreDtoResponse>>> ListAsync();

    Task<BaseResponseGeneric<IEnumerable<GenreDtoResponse>>> ListGenresAsync(string? filter, int page, int rows);

    Task<BaseResponseGeneric<GenreDtoResponse>> GetAsync(int id);

    Task<BaseResponseGeneric<int>> AddAsync(GenreDtoRequest genre);

    Task<BaseResponse> UpdateAsync(int id, GenreDtoRequest genre);

    Task<BaseResponse> DeleteAsync(int id);
}