using M295_ILBA24.Context;
using M295_ILBA24.DTOs;
using M295_ILBA24.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace M295_ILBA24.Services;

public class CustomerService (ILogger<CustomerService> logger, SakilaDbContext dbContext)
{
    public async Task<IEnumerable<CustomerResponseDto>> GetAll()
    {
        logger.LogInformation("Fetching all customers from Database");
        var customers = await dbContext.Customers
            .Include(c => c.Address)
            .ToListAsync();

        return customers.Adapt<IEnumerable<CustomerResponseDto>>();
    }
    
    public async Task<CustomerResponseDto> GetById(ushort id)
    {
        logger.LogInformation($"Fetching customer with ID {id} from Database");
        var customer = await dbContext.Customers
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.CustomerId == id);

        if (customer == null)
        {
            logger.LogWarning($"Customer with ID {id} not found");
            throw new KeyNotFoundException($"Customer with ID {id} not found");
        }

        return customer.Adapt<CustomerResponseDto>();
    }
    
    public async Task<CustomerResponseDto> InsertCustomer(CustomerRequestDto customer)
    {
        logger.LogInformation("Inserting new customer into Database, details: {@customer}", customer);
        var newCustomer = customer.Adapt<Customer>();
        dbContext.Customers.Add(newCustomer);
        await dbContext.SaveChangesAsync();

        return await GetById(newCustomer.CustomerId);
    }
    
    public async Task<CustomerResponseDto> UpdateCustomer(ushort id, CustomerRequestDto customer)
    {
        logger.LogInformation("Updating customer with ID {id}, new details: {@customer}", id, customer);
        var existingCustomer = await dbContext.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == id);

        if (existingCustomer == null)
        {
            logger.LogWarning($"Customer with ID {id} not found");
            throw new KeyNotFoundException($"Customer with ID {id} not found");
        }
        
        existingCustomer = customer.Adapt(existingCustomer);
        dbContext.Customers.Update(existingCustomer);
        await dbContext.SaveChangesAsync();

        return await GetById(existingCustomer.CustomerId);
    }
}