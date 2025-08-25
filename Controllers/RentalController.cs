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

public class RentalController(ILogger<RentalController> logger, RentalService service) : ControllerBase
{
    /// <summary>
    /// Filtert die Verleihvorgänge nach optionalen Parametern wie Kundenname, Filmname oder Mitarbeitername.
    /// </summary>
    /// <param name="customerFirstName">Vorname des Kunden (optional, StartsWith)</param>
    /// <param name="customerLastName">Nachname des Kunden (optional, StartsWith)</param>
    /// <param name="filmTitle">Titel des Films (optional, StartsWith)</param>
    /// <param name="staffFirstName">Vorname des Mitarbeiters (optional, StartsWith)</param>
    /// <param name="staffLastName">Nachname des Mitarbeiters (optional, StartsWith)</param>
    /// <returns>Liste von gefilterten Verleihvorgängen</returns>
    
    [HttpGet("filtered")]
    public async Task<ActionResult<List<RentalResponseDto>>> GetFilteredRentalsAsync(
        [FromQuery] string? customerFirstName,
        [FromQuery] string? customerLastName,
        [FromQuery] string? filmTitle,
        [FromQuery] string? staffFirstName,
        [FromQuery] string? staffLastName
        )
    {
        var rentals = await service.GetFilteredRentalsAsync(
            customerFirstName,
            customerLastName,
            filmTitle,
            staffFirstName,
            staffLastName
        );
        return Ok(rentals);
    }
    
    /// <summary>
    /// Gibt einen einzelnen Verleihvorgang anhand der ID zurück.
    /// </summary>
    /// <param name="id">ID des Verleihvorgangs</param>
    /// <returns>Details des Verleihvorgangs</returns>
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RentalResponseDto>> GetRentalByIdAsync(int id)
    {
        try
        {
            return Ok(await service.GetById(id));
            
        }
        catch (ResourceNotFoundException e)
        {
            return NotFound(e.Message);
        }
    } 
    
    /// <summary>
    /// Erstellt einen neuen Verleih.
    /// </summary>
    /// <param name="rental">Verleihdaten wie Kunde, Mitarbeiter und InventarNr</param>
    /// <returns>Der erstellte Verleihvorgang</returns>
        
    [HttpPost]
    public async Task<ActionResult<RentalResponseDto>> CreateRentalAsync([FromBody] RentalRequestDto rental)
    {
        return Created ("", await service.CreateRentalAsync(rental));
    }
    
    /// <summary>
    /// Schliesst einen Verleihvorgang ab, indem das Rückgabedatum gesetzt wird.
    /// </summary>
    /// <param name="id">ID des Verleihvorgangs</param>
    /// <returns>Aktualisierte Verleihdaten</returns>
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult> CloseRentalAsync(int id)
    {
        try
        {
            var result = await service.CloseRentalAsync(id);
            return Ok(result);
        }
        catch (ResourceNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

}
