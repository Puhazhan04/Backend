namespace M295_ILBA24.DTOs;

public record RentalResponseDto
(
    
int RentalId,
DateTime RentalDate,
uint InventoryId,
ushort CustomerId,
DateTime? ReturnDate,
byte StaffId,
DateTime LastUpdate);

public record RentalRequestDto(
    uint InventoryId,
    ushort CustomerId,
    byte StaffId
);
