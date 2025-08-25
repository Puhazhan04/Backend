namespace M295_ILBA24.DTOs;

public record FilmActorDto(
    BaseActorDto Actor,
    BaseFilmDto Film
);