using M295_ILBA24.DTOs;
using M295_ILBA24.Services;
using Microsoft.AspNetCore.Mvc;

namespace M295_ILBA24.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController(ILogger<CustomerController> logger, CustomerService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetAll()
    {
        logger.LogInformation("Fetching all customers");
        return Ok(await service.GetAll());
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> GetById(ushort id)
    {
        logger.LogInformation($"Fetching customer with ID {id}");
        return Ok(await service.GetById(id));
    }
    
    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> InsertCustomer([FromBody] CustomerRequestDto customer)
    {
        logger.LogInformation("Inserting new customer, details: {@customer}", customer);
        var createdCustomer = await service.InsertCustomer(customer);
        return CreatedAtAction(nameof(GetById), new { id = createdCustomer.CustomerId }, createdCustomer);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerResponseDto>> UpdateCustomer(ushort id, [FromBody] CustomerRequestDto customer)
    {
        logger.LogInformation("Updating customer with ID {id}, new details: {@customer}", id, customer);
        return Ok(await service.UpdateCustomer(id, customer));
    }
}