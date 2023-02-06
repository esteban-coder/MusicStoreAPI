using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Services.Interfaces;

namespace MusicStore.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly ISaleService _service;

    public ReportsController(ISaleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetReportSaleAsync(string startDate, string endDate)
    {
        var response = await _service.GetReportSaleAsync(DateTime.Parse(startDate), DateTime.Parse(endDate));

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }
}