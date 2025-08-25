using M295_ILBA24.DTOs;
using M295_ILBA24.Services;
using Microsoft.AspNetCore.Mvc;

namespace M295_ILBA24.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmController(ILogger<FilmController> logger, FilmService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<FilmResponseDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20
    )
    {
        logger.LogInformation("Fetching all films with pagination: page {page}, pageSize {pageSize}", page, pageSize);
        var films = await service.GetAllFilms(page, pageSize);
        logger.LogInformation("Fetched {count} films", films.Count);
        return Ok(films);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FilmResponseDto>> GetById([FromRoute] ushort id)
    {
        var film = await service.GetFilmById(id);
        if (film == null)
        {
            return NotFound();
        }

        return Ok(film);
    }

    [HttpPost]
    public async Task<ActionResult<FilmResponseDto>> Create([FromBody] FilmRequestDto filmRequestDto)
    {
        logger.LogInformation("Creating film {film}", filmRequestDto);
        var film = await service.CreateFilm(filmRequestDto);
        return CreatedAtAction(
            nameof(GetById),
            new { id = film.FilmId },
            film
        );
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<FilmResponseDto>> Update([FromRoute] ushort id,
        [FromBody] FilmRequestDto filmRequestDto)
    {
        logger.LogInformation("Updating film {id} with data {@film}", id, filmRequestDto);
        var film = await service.UpdateFilm(id, filmRequestDto);
        
        if (film == null)
        {
            return NotFound();
        }
        
        return Ok(film);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] ushort id)
    {
        logger.LogInformation("Deleting film {id}", id);
        var film = await service.DeleteFilm(id);
        if(!film)
        {
            return NotFound();
        }
        return NoContent();
    }
}