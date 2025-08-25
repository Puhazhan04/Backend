using M295_ILBA24.Context;
using M295_ILBA24.DTOs;
using M295_ILBA24.Entities;
using M295_ILBA24.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace M295_ILBA24.Services;

public class CityService(ILogger<CityService> logger, SakilaDbContext context)
{
    private IIncludableQueryable<City, ICollection<Address>> CityBaseQuery
        => context.Cities
            .Include(c => c.Country)
            .Include(c => c.Addresses);
    
    public async Task<ICollection<CityResponseDto>> FindAllCities()
    {
        logger.LogInformation("FindAllCities called");

        var cities = await CityBaseQuery.ToListAsync();

        logger.LogInformation("Found {count} cities", cities.Count);

        return cities.Adapt<List<CityResponseDto>>();
    }
    
    public async Task<CityResponseDto> FindCityById(ushort id)
    {
        logger.LogInformation("FindCityById called with id {id}", id);

        var city = await CityBaseQuery
            .Where(c => c.CityId == id)
            .FirstOrDefaultAsync();

        if (city == null)
        {
            throw new ResourceNotFoundException("Could not find city with ID " + id);
        }

        return city.Adapt<CityResponseDto>();
    }
    
    public async Task<CityResponseDto> CreateCity(CityRequestDto cityRequestDto)
    {
        var city = cityRequestDto.Adapt<City>();

        await context.Cities.AddAsync(city);
        await context.SaveChangesAsync();

        return await FindCityById(city.CityId);
    }

    public async Task<CityResponseDto> UpdateCity(ushort id, CityRequestDto requestDto)
    {
        var city = await context.Cities
            .Where(c => c.CityId == id)
            .FirstOrDefaultAsync();
        
        if (city == null)
            throw new ResourceNotFoundException("Could not find city with ID " + id);
        
        requestDto.Adapt(city);
        city.LastUpdate = DateTime.UtcNow;
        
        await context.SaveChangesAsync();
        
        logger.LogInformation("Updated city {city}", city);
        return await FindCityById(city.CityId);
    }
}