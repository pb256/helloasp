using Microsoft.EntityFrameworkCore;

namespace WebCms.Data
{
    public class WebCmsContext : DbContext
    {
        public WebCmsContext (DbContextOptions<WebCmsContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Movie> Movie { get; set; } = default!;
    }
}
