namespace M295_ILBA24.DTOs;

public record BaseFilmDto(
    ushort FilmId,
    string? Title,
    string? Description,
    short? ReleaseYear,
    byte LanguageId,
    byte? OriginalLanguageId,
    byte RentalDuration,
    decimal RentalRate,
    ushort Length,
    decimal ReplacementCost,
    string? Rating,
    DateTime LastUpdate
);

public record FilmResponseDto(
    ushort FilmId,
    string? Title,
    string? Description,
    short? ReleaseYear,
    byte LanguageId,
    byte? OriginalLanguageId,
    byte RentalDuration,
    decimal RentalRate,
    ushort Length,
    decimal ReplacementCost,
    string? Rating,
    DateTime LastUpdate,
    ICollection<FilmActorDto> FilmActors
) : BaseFilmDto(FilmId, Title, Description, ReleaseYear, LanguageId, OriginalLanguageId, RentalDuration, RentalRate,
    Length, ReplacementCost, Rating, LastUpdate);