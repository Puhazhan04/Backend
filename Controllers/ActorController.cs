using M295_ILBA24.Context;
using M295_ILBA24.DTOs;
using M295_ILBA24.Entities;
using M295_ILBA24.Exceptions;
using M295_ILBA24.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace M295_ILBA24.Controllers;

[ApiController]
[Route("[controller]")]
public class ActorController(ILogger<ActorController> logger, ActorService service) : ControllerBase
{
    /// <summary>
    /// Fetches all actors from the database
    /// </summary>
    /// <param name="actorId">The id of the action to filter for</param>
    /// <param name="page">The page to load</param>
    /// <param name="pageSize">The page-size - default is 10 elements per page</param>
    /// <returns>
    /// A collection of all actors
    /// </returns>
    /// <code>
    /// GET /actor
    /// </code>
    /// <response code="200">Returns all actors</response>
    [HttpGet]
    public async Task<ActionResult<ICollection<ActorResponseDto>>> GetAllActorsAsync(
        [FromQuery] int actorId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        return Ok(await service.GetAllActorsAsync(actorId, page, pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ActorResponseDto>> GetActorByIdAsync(ushort id)
    {
        try
        {
            var actorByIdAsync = await service.GetActorByIdAsync(id);
            return Ok(actorByIdAsync);
        }
        catch (ResourceNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ActorResponseDto>> CreateActorAsync([FromBody] ActorResponseDto actor)
    {
        return Created("", await service.CreateActorAsync(actor));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateActorAsync(ushort id, [FromBody] Actor actor)
    {
        try
        {
            await service.UpdateActorAsync(id, actor);
            return NoContent();
        }
        catch (ResourceNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteActorAsync(ushort id)
    {
        try
        {
            await service.DeleteActorAsync(id);
            return NoContent();
        }
        catch (ResourceNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost("{actorId:int}/film/{filmId:int}")]
    public async Task<ActionResult> AddFilmToActorAsync(ushort actorId, ushort filmId)
    {
        try
        {
            await service.AddFilmToActorAsync(actorId, filmId);
            return NoContent();
        }
        catch (ResourceNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}