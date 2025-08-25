using System.Reflection;
using M295_ILBA24.Context;
using M295_ILBA24.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Configure Serilog from appsettings.json
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration)
        .Enrich.WithClassName();
});

builder.Services.AddDbContext<SakilaDbContext>(optionsBuilder =>
    optionsBuilder.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.Parse("8.1.0-mysql")
    )
);

builder.Services.AddMapster();

builder.Services.AddScoped<ActorService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<CityService>();
builder.Services.AddScoped<FilmService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<RentalService>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();