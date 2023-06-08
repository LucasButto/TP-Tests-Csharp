using System;
using Xunit;
using MovieDatabaseApp.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace MovieDatabaseApp.Tests
{
    public class MovieTests
    {
        private readonly MovieDatabaseContext _context;

        public MovieTests()
        {
            _context = CreateInMemoryDbContext();
        }

        private MovieDatabaseContext CreateInMemoryDbContext()
        {
            var services = new ServiceCollection()
                .AddDbContext<MovieDatabaseContext>(options =>
                    options.UseInMemoryDatabase("MovieDatabase"))
                .BuildServiceProvider();

            return services.GetRequiredService<MovieDatabaseContext>();
        }


        [Fact]
        public void TestAddMovie()
        {
            var newMovie = new Movie
            {
                MovieName = "The Matrix",
                MovieGenre = "Action",
                MovieDuration = 136,
                MovieBudget = 63000000
            };

            _context.Movies.Add(newMovie);
            _context.SaveChanges();

            var movie = _context.Movies.Find(newMovie.Id);
            Assert.NotNull(movie);
            Assert.Equal("The Matrix", movie.MovieName);
            Assert.Equal("Action", movie.MovieGenre);
            Assert.Equal(136, movie.MovieDuration);
            Assert.Equal(63000000, movie.MovieBudget);
        }

        [Fact]
        public void TestDeleteMovie()
        {
            var newMovie = new Movie
            {
                MovieName = "The Matrix",
                MovieGenre = "Action",
                MovieDuration = 136,
                MovieBudget = 63000000
            };

            _context.Movies.Add(newMovie);
            _context.SaveChanges();

            _context.Movies.Remove(newMovie);
            _context.SaveChanges();

            var movie = _context.Movies.Find(newMovie.Id);
            Assert.Null(movie);
        }

        [Fact]
        public void TestUpdateMovie()
        {
            var newMovie = new Movie
            {
                MovieName = "The Matrix",
                MovieGenre = "Action",
                MovieDuration = 136,
                MovieBudget = 63000000
            };

            _context.Movies.Add(newMovie);
            _context.SaveChanges();

            var movie = _context.Movies.Find(newMovie.Id);
            movie.MovieName = "The Matrix Reloaded";
            movie.MovieBudget = 150000000;
            _context.SaveChanges();

            var updatedMovie = _context.Movies.Find(newMovie.Id);
            Assert.NotNull(updatedMovie);
            Assert.Equal("The Matrix Reloaded", updatedMovie.MovieName);
            Assert.Equal(150000000, updatedMovie.MovieBudget);
        }
    }
}
