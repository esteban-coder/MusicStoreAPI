using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Services.Interfaces;

namespace MusicStore.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConcertsController : ControllerBase
{
    private readonly IConcertService _service;

    public ConcertsController(IConcertService service)
    {
        _service = service;
    }

    
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponseGeneric<List<ConcertDtoResponse>>), 200)]
    public async Task<IActionResult> GetConcerts(string? filter, int page = 1, int pageSize = 10)
    {
        return Ok(await _service.ListAsync(filter, page, pageSize));
    }

    // GET: api/Concerts/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BaseResponseGeneric<ConcertDtoResponse>), 200)]
    [ProducesResponseType(typeof(BaseResponseGeneric<ConcertDtoResponse>), 404)]
    public async Task<IActionResult> GetConcert(int id)
    {
       return Ok(await _service.GetAsync(id));
    }


    // PUT: api/Concerts/5
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BaseResponse), 200)]
    [ProducesResponseType(typeof(BaseResponse), 404)]
    public async Task<IActionResult> PutConcert(int id, ConcertDtoRequest request)
    {
        return Ok(await _service.UpdateAsync(id, request));
    }

    // POST: api/Concerts
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse), 200)]
    public async Task<IActionResult> PostConcert(ConcertDtoRequest request)
    {
        return Ok(await _service.AddAsync(request));
    }

    // DELETE: api/Concerts/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(BaseResponse), 200)]
    [ProducesResponseType(typeof(BaseResponse), 404)]
    public async Task<IActionResult> DeleteConcert(int id)
    {
        return Ok(await _service.DeleteAsync(id));
    }
}