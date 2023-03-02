using Microsoft.EntityFrameworkCore;
using WebCms.Data;
using WebCms.Models;

namespace WebCms.SeedData;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new WebCmsContext(serviceProvider.GetRequiredService<DbContextOptions<WebCmsContext>>());
        
        if (context.Movie == null)
        {
            throw new ArgumentNullException("Null RazorPagesMovieContext");
        }

        // Look for any movies.
        if (context.Movie.Any())
        {
            // DB has been seeded
            return;
        }

        context.Movie.AddRange(
            new Movie
            {
                Title = "When Harry Met Sally",
                ReleaseDate = DateTime.Parse("1989-2-12"),
                Genre = "Romantic Comedy",
                Price = 7.99M,
                Rating = "R"
            },
            new Movie
            {
                Title = "Ghostbusters",
                ReleaseDate = DateTime.Parse("1984-3-13"),
                Genre = "Comedy",
                Price = 8.99M,
                Rating = "R"
            },
            new Movie
            {
                Title = "Ghostbusters 2",
                ReleaseDate = DateTime.Parse("1986-2-23"),
                Genre = "Comedy",
                Price = 9.99M,
                Rating = "R"
            },
            new Movie
            {
                Title = "Rio Bravo",
                ReleaseDate = DateTime.Parse("1959-4-15"),
                Genre = "Western",
                Price = 3.99M,
                Rating = "R"
            }
        );
        context.SaveChanges();
    }
}