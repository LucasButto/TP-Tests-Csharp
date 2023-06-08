using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieDatabaseApp.Models;

namespace MovieDatabaseApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Registra el contexto de la base de datos
            var services = new ServiceCollection()
                .AddDbContext<MovieDatabaseContext>(options =>
                        options.UseMySql("server=localhost;database=MovieDatabase;user=root;password=2409;ConnectionTimeout=30;", new MySqlServerVersion(new Version(8, 0, 25))))

                .BuildServiceProvider();

            // agregar una nueva película y un actor a la base de datos
                using (var scope = services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<MovieDatabaseContext>();

                    var newMovie = new Movie
                    {
                        MovieName = "The Matrix",
                        MovieGenre = "Action",
                        MovieDuration = 136,
                        MovieBudget = 63000000
                    };
                    context.Movies.Add(newMovie);

                    var newActor = new Actor
                    {
                        ActorName = "Keanu Reeves",
                        ActorBirthdate = DateOnly.FromDateTime(new DateTime(1964, 9, 2))
                    };
                    context.Actors.Add(newActor);

                    context.SaveChanges();
                }

                // mostrar las películas y los actores almacenados en la base de datos:
                using (var scope = services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<MovieDatabaseContext>();

                    var movies = context.Movies.ToList();
                    var actors = context.Actors.ToList();

                    Console.WriteLine("Movies:");
                    foreach (var movie in movies)
                    {
                        Console.WriteLine($"- {movie.MovieName} ({movie.MovieGenre}, {movie.MovieDuration} min, {movie.MovieBudget:C})");
                    }

                    Console.WriteLine("\nActors:");
                    foreach (var actor in actors)
                    {
                        Console.WriteLine($"- {actor.ActorName} ({actor.ActorBirthdate.ToShortDateString()})");
                    }
                }

        }
    }
}
