namespace M295_ILBA24.DTOs;

public record CustomerResponseDto(
    ushort CustomerId,
    byte StoreId,
    string FirstName,
    string LastName,
    string? Email,
    ushort AddressId,
    bool? Active,
    DateTime CreateDate,
    DateTime? LastUpdate,
    AddressResponseDto Address
);

public record CustomerRequestDto(
    byte StoreId,
    string FirstName,
    string LastName,
    string? Email,
    ushort AddressId,
    bool? Active
);