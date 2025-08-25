namespace M295_ILBA24.DTOs;

public record BaseActorDto(
    ushort ActorId,
    string FirstName,
    string LastName,
    DateTime LastUpdate
);

public record FullActorDto(
    ushort ActorId,
    string FirstName,
    string LastName,
    DateTime LastUpdate,
    ICollection<BaseFilmDto> Films
) : BaseActorDto(ActorId, FirstName, LastName, LastUpdate);

public record ActorResponseDto(
    ushort ActorId,
    string FirstName,
    string LastName,
    DateTime LastUpdate,
    ICollection<BaseFilmDto> Films
): BaseActorDto(ActorId, FirstName, LastName, LastUpdate);