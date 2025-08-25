using System;
using System.Collections.Generic;

namespace M295_ILBA24.Entities;

public partial class SalesByStore
{
    public string? Store { get; set; }

    public string? Manager { get; set; }

    public decimal? TotalSales { get; set; }
}
