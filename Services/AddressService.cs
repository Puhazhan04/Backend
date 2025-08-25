using M295_ILBA24.Context;
using M295_ILBA24.DTOs;
using M295_ILBA24.Entities;
using M295_ILBA24.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace M295_ILBA24.Services;

public class AddressService(ILogger<AddressService> logger, SakilaDbContext context)
{
    public async Task<ICollection<AddressResponseDto>> FindAllAddresses()
    {
        logger.LogInformation("FindAllAddresses called");

        var addressEntities = await context.Addresses
            .Include(a => a.City)
            .ThenInclude(c => c.Country)
            .ToListAsync();

        var mappedAddresses = addressEntities.Adapt<List<AddressResponseDto>>();

        return mappedAddresses.ToList();
    }

    public async Task<AddressResponseDto> FindAddressById(ushort id)
    {
        logger.LogInformation("FindAddressById called with id {id}", id);

        var addressEntity = await context.Addresses
            .Include(a => a.City)
            .ThenInclude(c => c.Country)
            .Where(a => a.AddressId == id)
            .FirstOrDefaultAsync();

        if (addressEntity == null)
        {
            throw new ResourceNotFoundException("Could not find address with ID " + id);
        }

        var mappedAddress = addressEntity.Adapt<AddressResponseDto>();

        return mappedAddress;
    }

    public async Task<AddressResponseDto> CreateAddress(AddressRequestDto addressRequestDto)
    {
        var address = new Address();
        address.Address1 = addressRequestDto.Address1!;
        address.Address2 = addressRequestDto.Address2;
        address.District = addressRequestDto.District!;
        address.CityId = addressRequestDto.CityId ?? 0;
        address.PostalCode = addressRequestDto.PostalCode;
        address.Phone = addressRequestDto.Phone!;

        await context.Addresses.AddAsync(address);
        await context.SaveChangesAsync();

        return new AddressResponseDto
        {
            AddressId = address.AddressId,
            Address1 = address.Address1,
            Address2 = address.Address2,
            District = address.District,
            CityId = address.CityId,
            PostalCode = address.PostalCode,
            Phone = address.Phone
        };
    }

    public async Task<AddressResponseDto> UpdateAddressAsync(ushort id, AddressRequestDto addressRequestDto)
    {
        var address = await context.Addresses.FindAsync(id);

        if (address == null)
        {
            throw new ResourceNotFoundException("Could not find address with ID " + id);
        }
        
        address.Address1 = addressRequestDto.Address1 ?? "";
        address.Address2 = addressRequestDto.Address2;
        address.District = addressRequestDto.District ?? "";
        address.CityId = addressRequestDto.CityId ?? 0;
        address.PostalCode = addressRequestDto.PostalCode;
        address.Phone = addressRequestDto.Phone ?? "";

        await context.SaveChangesAsync();

        return await FindAddressById(address.AddressId);
    }
}