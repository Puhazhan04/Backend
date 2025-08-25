using M295_ILBA24.Context;
using M295_ILBA24.DTOs; 
using M295_ILBA24.Entities;
using M295_ILBA24.Exceptions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace M295_ILBA24.Services;

public class RentalService(ILogger<RentalService> logger, SakilaDbContext dbContext)
{
    /// <summary>
    /// Holt alle Verleihvorgänge mit optionalen Filterparametern.
    /// </summary>
    public async Task<ActionResult<List<RentalResponseDto>>> GetFilteredRentalsAsync(
        [FromQuery] string? customerFirstName,
        [FromQuery] string? customerLastName,
        [FromQuery] string? filmTitle,
        [FromQuery] string? staffFirstName,
        [FromQuery] string? staffLastName
    )
    {
        var quary = dbContext.Rentals
            .Include(r => r.Customer)
            .Include(r => r.Staff)
            .Include(r => r.Inventory)
            .ThenInclude(i => i.Film)
            .AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(customerFirstName))
            quary = quary.Where(r => r.Customer.FirstName.StartsWith(customerFirstName));
        
        if (!string.IsNullOrWhiteSpace(customerLastName))
            quary = quary.Where(r => r.Customer.LastName.StartsWith(customerLastName));
        
        if (!string.IsNullOrWhiteSpace(filmTitle))
            quary = quary.Where(r => r.Inventory.Film.Title.StartsWith(filmTitle));
        
        if (!string.IsNullOrWhiteSpace(staffFirstName))
            quary = quary.Where(r => r.Staff.FirstName.StartsWith(staffFirstName));
        
        if (!string.IsNullOrWhiteSpace(staffLastName))
            quary = quary.Where(r => r.Staff.LastName.StartsWith(staffLastName));
        
        var rentals = await quary.ToListAsync();
        
        return rentals.Select(r => new RentalResponseDto(
                r.RentalId,
                r.RentalDate,
                r.InventoryId,
                r.CustomerId,
                r.ReturnDate,
                r.StaffId,
                r.LastUpdate
            )).ToList();
    }
    
    /// <summary>
    /// Holt einen einzelnen Verleihvorgang anhand der ID.
    /// </summary>
    /// <param name="id">ID des Verleihs</param>
    /// <returns>Ein einzelner Verleihvorgang als DTO</returns>
    public async Task<RentalResponseDto> GetById(int id)
    {
        var rental = await dbContext
            .Rentals
            .Include(r => r.Customer)
            .Include(r => r.Inventory)
            .Include(r => r.Staff)
            .FirstOrDefaultAsync(r => r.RentalId == id);

        if (rental == null)
        {
            throw new ResourceNotFoundException("Could not find rental with ID " + id);
        }
        
        return new RentalResponseDto(
                rental.RentalId,
                rental.RentalDate,
                rental.InventoryId,
                rental.CustomerId,
                rental.ReturnDate,
                rental.StaffId,
                rental.LastUpdate
        );

    }
    
    /// <summary>
    /// Erstellt einen neuen Verleihvorgang im System.
    /// </summary>
    /// <param name="rental">DTO mit InventarNr, Kunde und Mitarbeiter</param>
    /// <returns>Das erstellte Verleihobjekt</returns>
    public async Task<RentalResponseDto> CreateRentalAsync(RentalRequestDto rental)
    {
        logger.LogInformation("Creating rental {@rental}", rental);
        
        var rentalEntity = rental.Adapt<Rental>();
        rentalEntity.RentalDate = DateTime.Now;
        rentalEntity.LastUpdate = DateTime.Now;
        
        dbContext.Rentals.Add(rentalEntity);
        await dbContext.SaveChangesAsync();

        return await GetById(rentalEntity.RentalId);
    }
    
    /// <summary>
    /// Schliesst einen Verleihvorgang ab, indem das Rückgabedatum gesetzt wird.
    /// </summary>
    /// <param name="id">ID des zu schließenden Verleihs</param>
    /// <returns>Das aktualisierte Verleihobjekt</returns>
    /// <exception cref="ResourceNotFoundException">Wenn kein Verleih mit der ID gefunden wird</exception>

    public async Task<RentalResponseDto> CloseRentalAsync(int id)
    {
        var rental = await dbContext.Rentals.FindAsync(id);
        if (rental == null)
        {
            throw new ResourceNotFoundException("Could not find rental with ID " + id);
        }

        rental.ReturnDate = DateTime.Now;
        rental.LastUpdate = DateTime.Now;
        await dbContext.SaveChangesAsync();
        
        return await GetById(rental.RentalId);
        
    }
    
}
