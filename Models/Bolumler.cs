using System;
using System.Collections.Generic;

namespace Universite.Models;

public partial class Bolumler
{
    public int BolumId { get; set; }

    public int? FakulteId { get; set; }

    public string? BolumAd { get; set; }

    public virtual Fakulteler? Fakulte { get; set; }
}
