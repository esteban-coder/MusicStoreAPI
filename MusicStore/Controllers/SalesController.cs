using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Services.Interfaces;

namespace MusicStore.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesController : ControllerBase
{
    private readonly ISaleService _service;

    public SalesController(ISaleService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSaleAsync([FromBody] SaleDtoRequest request)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        
        var response = await _service.CreateSaleAsync(email, request);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateAsync([FromBody] SaleDtoRequest request)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        var response = await _service.CreateSaleAsync(email, request);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSaleAsync(int id)
    {
        var response = await _service.GetSaleAsync(id);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var response = await _service.GetSaleAsync(id);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpGet("ListSales")]
    [Authorize]
    public async Task<IActionResult> GetListSales(int page = 1, int pageSize = 10)
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)!.Value;

        var response = await _service.ListSalesAsync(email, page, pageSize);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpGet("ListSalesByDate")]
    [Authorize]
    public async Task<IActionResult> GetListSalesByDate(string startDate, string endDate, int page = 1, int pageSize = 10)
    {
        var response = await _service.ListSalesAsync(DateTime.Parse(startDate), 
            DateTime.Parse(endDate), page, pageSize);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpGet("ListSalesByGenre")]
    [Authorize]
    public async Task<IActionResult> GetListSalesByGenre(string startDate, string endDate, int page = 1, int pageSize = 10)
    {
        var response = await _service.ListSalesAsync(DateTime.Parse(startDate),
            DateTime.Parse(endDate), page, pageSize);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }
}