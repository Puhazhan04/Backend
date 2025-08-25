using M295_ILBA24.DTOs;
using M295_ILBA24.Exceptions;
using M295_ILBA24.Services;
using Microsoft.AspNetCore.Mvc;

namespace M295_ILBA24.Controllers;

[ApiController]
[Route("[controller]")]
public class CityController(ILogger<CityController> logger, CityService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<CityResponseDto>>> GetAllCities()
    {
        logger.LogInformation("GetAllCities called from User Agent {userAgent}",
            HttpContext.Request.Headers["User-Agent"]);

        var cities = await service.FindAllCities();
        return Ok(cities);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ICollection<CityResponseDto>>> GetCityById(ushort id)
    {
        logger.LogInformation("GetCityById called from User Agent {userAgent}",
            HttpContext.Request.Headers["User-Agent"]);

        try
        {
            var cities = await service.FindCityById(id);
            return Ok(cities);
        }
        catch (ResourceNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<CityResponseDto>> CreateCity([FromBody] CityRequestDto requestDto)
    {
        logger.LogInformation("CreateCity called from User Agent {userAgent}",
            HttpContext.Request.Headers["User-Agent"]);
        
        var city = await service.CreateCity(requestDto);
        logger.LogInformation("Created city {city}", city);
        
        return CreatedAtAction(
            nameof(GetCityById), 
            new { id = city.CityId }, 
            city
        );
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CityResponseDto>> UpdateCity(ushort id, [FromBody] CityRequestDto requestDto)
    {
        logger.LogInformation("UpdateCity called from User Agent {userAgent}",
            HttpContext.Request.Headers["User-Agent"]);
        
        try
        {
            var city = await service.UpdateCity(id, requestDto);
            return Ok(city);
        }
        catch (ResourceNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}