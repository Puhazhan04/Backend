namespace M295_ILBA24.DTOs;

public class AddressRequestDto
{
    public string? Address1 { get; set; } = "";

    public string? Address2 { get; set; }

    public string? District { get; set; }

    public ushort? CityId { get; set; }

    public string? PostalCode { get; set; }

    public string? Phone { get; set; }
}