using M295_ILBA24.Context;
using M295_ILBA24.DTOs;
using M295_ILBA24.Entities;
using M295_ILBA24.Exceptions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace M295_ILBA24.Services;

public class ActorService(ILogger<ActorService> logger, SakilaDbContext dbContext)
{
    public async Task<List<ActorResponseDto>> GetAllActorsAsync(int actorId, int page = 1, int pageSize = 10)
    {
        var pagedActors = dbContext.Actors
            .Where(actor => actor.ActorId == actorId || actorId <= 0)
            .Include(a => a.FilmActors)
            .ThenInclude(fa => fa.Film)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        
        var pagedActorsResponse = await pagedActors.ToListAsync();
        return pagedActorsResponse.Select(GenerateActorResponseDto).ToList();
    }

    public async Task<ActorResponseDto> GetActorByIdAsync(ushort id)
    {
        // Schauspieler aus Datenbank auslesen --> Join auf FilmActors und Film
        var actor = await dbContext
            .Actors
            .Include(a => a.FilmActors)
            .ThenInclude(fa => fa.Film)
            .FirstOrDefaultAsync(a => a.ActorId == id);

        if (actor == null)
        {
            throw new ResourceNotFoundException("Could not find actor with ID " + id);
        }

        // Wir verschieben die Daten in ein neues Objekt, um Rekursionsfrei zu werden
        return GenerateActorResponseDto(actor);
    }

    private static ActorResponseDto GenerateActorResponseDto(Actor actor)
    {
        return new ActorResponseDto(actor.ActorId, actor.FirstName, actor.LastName, actor.LastUpdate,
            actor.FilmActors.Select(fa =>
                new BaseFilmDto(
                    fa.Film.FilmId,
                    fa.Film.Title,
                    fa.Film.Description,
                    fa.Film.ReleaseYear,
                    fa.Film.LanguageId,
                    fa.Film.OriginalLanguageId,
                    fa.Film.RentalDuration,
                    fa.Film.RentalRate,
                    fa.Film.Length ?? 0,
                    fa.Film.ReplacementCost,
                    fa.Film.Rating,
                    fa.Film.LastUpdate
                )).ToList());
    }

    public async Task<ActorResponseDto> CreateActorAsync(ActorResponseDto actor)
    {
        logger.LogInformation("Creating actor {@actor}", actor);

        dbContext.Actors.Add(actor.Adapt<Actor>());
        await dbContext.SaveChangesAsync();

        return await GetActorByIdAsync(actor.ActorId);
    }

    public async Task UpdateActorAsync(ushort id, [FromBody] Actor actor)
    {
        var actorFromDb = await dbContext.Actors.FindAsync(id);
        if (actorFromDb == null)
        {
            throw new ResourceNotFoundException("Could not find actor with ID " + id);
        }

        actorFromDb.FirstName = actor.FirstName;
        actorFromDb.LastName = actor.LastName;
        actorFromDb.LastUpdate = DateTime.Now;

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteActorAsync(ushort id)
    {
        var actor = await dbContext.Actors.FindAsync(id);
        if (actor == null)
        {
            throw new ResourceNotFoundException("Could not find actor with ID " + id);
        }

        dbContext.Actors.Remove(actor);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddFilmToActorAsync(ushort actorId, ushort filmId)
    {
        var actor = await dbContext.Actors
            .Include(a => a.FilmActors)
            .Where(a => 
                a.ActorId == actorId 
                        && a.FilmActors
                .All(fa => fa.FilmId != filmId)).FirstOrDefaultAsync();
        
        if (actor == null)
            throw new ResourceNotFoundException("Could not find actor with ID " + actorId + " or actor is already in film with ID " + filmId);
        
        var film = await dbContext.Films.FindAsync(filmId);
        
        if (film == null)
            throw new ResourceNotFoundException("Could not find film with ID " + filmId);

        var filmActor = new FilmActor();
        filmActor.ActorId = actorId;
        filmActor.FilmId = filmId;
        filmActor.LastUpdate = DateTime.Now;
        
        dbContext.FilmActors.Add(filmActor);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Added film with ID {filmId} to actor with ID {actorId}", filmId, actorId);
    }
}