using M295_ILBA24.DTOs;
using M295_ILBA24.Exceptions;
using M295_ILBA24.Services;
using Microsoft.AspNetCore.Mvc;

namespace M295_ILBA24.Controllers;

[ApiController]
[Route("[controller]")]
public class AddressController(ILogger<AddressController> logger, AddressService service) : ControllerBase
{
    /// <summary>
    /// Returns all addresses
    /// </summary>
    /// <response code="200">Returns all addresses</response>
    /// <response code="500">If an error occurs</response>
    /// <returns>A collection of addresses</returns>
    [HttpGet]
    public async Task<ActionResult<ICollection<AddressResponseDto>>> GetAllAddresses()
    {
        logger.LogInformation("GetAllAddresses called from User Agent {userAgent}",
            HttpContext.Request.Headers["User-Agent"]);
        var addresses = await service.FindAllAddresses();
        return Ok(addresses);
    }

    /// <summary>
    /// Beispielcode - C# kann keine Geometry Data erstellen (sakila)
    /// TODO: Find a way to create geometry data
    /// </summary>
    /// <param name="addressRequestDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<AddressResponseDto>> CreateAddress([FromBody] AddressRequestDto addressRequestDto)
    {
        logger.LogInformation("Trying to create address {address}", addressRequestDto);
        var address = await service.CreateAddress(addressRequestDto);

        return address;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AddressResponseDto>> UpdateAddressAsync(ushort id,
        [FromBody] AddressRequestDto addressRequestDto)
    {
        try
        {
            var address = await service.UpdateAddressAsync(id, addressRequestDto);
            return Ok(address);
        }
        catch (ResourceNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}