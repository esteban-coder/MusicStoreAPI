using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Services.Interfaces;

namespace MusicStore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IGenreService _service;

    public GenresController(IGenreService service)
    {
        _service = service;
    }

    // GET: api/Genres
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponseGeneric<List<GenreDtoResponse>>), 200)]
    public async Task<IActionResult> GetGenres()
    {
        return Ok(await _service.ListAsync());
    }

    // GET: api/Genres/list
    [HttpGet("list")]
    [ProducesResponseType(typeof(BaseResponseGeneric<List<GenreDtoResponse>>), 200)]
    public async Task<IActionResult> listGenres(string? filter, int page = 1, int pageSize = 10)
    {
        return Ok(await _service.ListGenresAsync(filter, page, pageSize));
    }

    // GET: api/Genres/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BaseResponseGeneric<GenreDtoResponse>), 200)]
    [ProducesResponseType(typeof(BaseResponseGeneric<GenreDtoResponse>), 404)]
    [Authorize]
    public async Task<IActionResult> GetGenre(int id)
    {
        return Ok(await _service.GetAsync(id));
    }


    // PUT: api/Genres/5
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BaseResponse), 200)]
    [ProducesResponseType(typeof(BaseResponse), 404)]
    public async Task<IActionResult> PutGenre(int id, GenreDtoRequest request)
    {
       return Ok(await _service.UpdateAsync(id, request));
    }

    // POST: api/Genres
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse), 200)]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> PostGenre(GenreDtoRequest genreDtoRequest)
    {
        return Ok(await _service.AddAsync(genreDtoRequest));
    }

    // DELETE: api/Genres/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(BaseResponse), 200)]
    [ProducesResponseType(typeof(BaseResponse), 404)]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        return Ok(await _service.DeleteAsync(id));
    }

}