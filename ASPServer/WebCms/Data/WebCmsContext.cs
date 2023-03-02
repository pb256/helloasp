using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebCms.Models;

namespace WebCms.Data
{
    public class WebCmsContext : DbContext
    {
        public WebCmsContext (DbContextOptions<WebCmsContext> options)
            : base(options)
        {
        }

        public DbSet<WebCms.Models.Movie> Movie { get; set; } = default!;
    }
}
