using System;
using System.Collections.Generic;

namespace MovieDatabaseApp.Models;

public partial class Actor
{
    public int Id { get; set; }

    public string ActorName { get; set; } = null!;

    public DateOnly ActorBirthdate { get; set; }

    public byte[]? ActorPicture { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
