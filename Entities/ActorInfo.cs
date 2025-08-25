using System;
using System.Collections.Generic;

namespace M295_ILBA24.Entities;

public partial class ActorInfo
{
    public ushort ActorId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? FilmInfo { get; set; }
}
