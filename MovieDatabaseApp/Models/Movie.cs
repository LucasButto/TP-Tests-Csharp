using System;
using System.Collections.Generic;

namespace MovieDatabaseApp.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string MovieName { get; set; } = null!;

    public string MovieGenre { get; set; } = null!;

    public int MovieDuration { get; set; }

    public decimal MovieBudget { get; set; }

    public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();
}
