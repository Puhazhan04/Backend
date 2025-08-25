using M295_ILBA24.Context;
using M295_ILBA24.DTOs;
using M295_ILBA24.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace M295_ILBA24.Services;

public class FilmService (ILogger<FilmService> logger, SakilaDbContext context)
{
    public async Task<ICollection<FilmResponseDto>> GetAllFilms(int page = 1, int pageSize = 20)
    {
        logger.LogInformation("Fetching all films with pagination: page {page}, pageSize {pageSize}", page, pageSize);
        
        if (page < 1)
        {
            page = 1;
        }
        
        if (pageSize < 1)
        {
            pageSize = 20;
        }
        
        var skip = (page - 1) * pageSize;
        var take = pageSize;
        
        var films = await context.Films
            .Include(f => f.FilmActors)
            .ThenInclude(fa => fa.Actor)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return films.Adapt<ICollection<FilmResponseDto>>();
    }
    
    public async Task<FilmResponseDto?> GetFilmById(ushort id)
    {
        var film = await context.Films
            .Include(f => f.FilmActors)
            .ThenInclude(fa => fa.Actor)
            .FirstOrDefaultAsync(f => f.FilmId == id);

        return film.Adapt<FilmResponseDto>();
    }
    
    public async Task<FilmResponseDto> CreateFilm(FilmRequestDto filmRequestDto)
    {
        var film = filmRequestDto.Adapt<Film>();
        context.Films.Add(film);
        // Hier wird die ID des neu erstellten Films generiert
        await context.SaveChangesAsync();
        return film.Adapt<FilmResponseDto>();
    }

    public async Task<FilmResponseDto?> UpdateFilm(ushort id, FilmRequestDto filmRequestDto)
    {
        var film = await context.Films.FindAsync(id);
        if (film == null)
            return null;
        
        filmRequestDto.Adapt(film);
        context.Entry(film).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return await GetFilmById(id);
    }

    public async Task<bool> DeleteFilm(ushort id)
    {
        var film = await context.Films.FindAsync(id);
        if (film == null)
        {
            logger.LogWarning("Film with ID {id} not found for deletion", id);
            return false;
        }
        
        context.Films.Remove(film);
        await context.SaveChangesAsync();
        return true;
    }
}