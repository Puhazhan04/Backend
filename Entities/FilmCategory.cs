using System;
using System.Collections.Generic;

namespace M295_ILBA24.Entities;

public partial class FilmCategory
{
    public ushort FilmId { get; set; }

    public byte CategoryId { get; set; }

    public DateTime LastUpdate { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Film Film { get; set; } = null!;
}
