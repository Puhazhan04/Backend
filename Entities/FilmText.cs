using System;
using System.Collections.Generic;

namespace M295_ILBA24.Entities;

public partial class FilmText
{
    public short FilmId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }
}
