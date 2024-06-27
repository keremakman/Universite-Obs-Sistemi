using System;
using System.Collections.Generic;

namespace Universite.Models;

public partial class Fakulteler
{
    public int FakulteId { get; set; }

    public string? FakulteAd { get; set; }

    public virtual ICollection<Bolumler> Bolumlers { get; set; } = new List<Bolumler>();
}
