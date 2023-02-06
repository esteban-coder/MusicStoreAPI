using Microsoft.AspNetCore.Mvc;
using MusicStore.Services.Interfaces;

namespace MusicStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IConcertService _concertService;
        private readonly IGenreService _genreService;

        public HomeController(IConcertService concertService, IGenreService genreService)
        {
            _concertService = concertService;
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var genres = await _genreService.ListAsync();

            var concerts = await _concertService.ListAsync(null, 1, 100);

            return Ok(new
            {
                Genres = genres.Data,
                Concerts = concerts.Data,
                Success = true
            });
        }
    }
}
