namespace M295_ILBA24.DTOs;

public class BaseAddressResponseDto
{
    public ushort AddressId { get; set; }

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public string District { get; set; } = null!;

    public ushort CityId { get; set; }

    public string? PostalCode { get; set; }

    public string Phone { get; set; } = null!;
}

public class AddressResponseDto : BaseAddressResponseDto
{
    public virtual BaseCityResponseDto City { get; set; } = null!;
}