namespace M295_ILBA24.DTOs;

public class BaseCityResponseDto
{
    public ushort CityId { get; set; }

    public string City1 { get; set; } = null!;

    public ushort CountryId { get; set; }
    
    public virtual BaseCountryResponseDto Country { get; set; } = null!;
}

public class CityResponseDto : BaseCityResponseDto
{
    public virtual ICollection<BaseAddressResponseDto> Addresses { get; set; } = new List<BaseAddressResponseDto>();
}