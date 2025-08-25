using System;
using System.Collections.Generic;

namespace M295_ILBA24.Entities;

public partial class SalesByFilmCategory
{
    public string Category { get; set; } = null!;

    public decimal? TotalSales { get; set; }
}
