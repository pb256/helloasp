using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCms.Data;
using WebCms.Models;

namespace WebCms.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly WebCms.Data.WebCmsContext _context;

        public IndexModel(WebCms.Data.WebCmsContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; } = default!;
        
        // 동일한 이름으로 바인딩 (SearchString)
        // SupportsGet = true ?
        // Get 요청에서도 받아올 수 있게 지원한다는건가
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        
        // 선택 리스트 (Microsoft.AspNetCore.MVC.Rendering)
        public SelectList? Genres { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? MovieGenre { get; set; }

        public async Task OnGetAsync()
        {
            var genreQuery = 
                _context.Movie.OrderBy(m => m.Genre)
                    .Select(m => m.Genre);
            
            var movies = _context.Movie.Select(m => m);
            if (!string.IsNullOrEmpty(SearchString)) 
                movies = movies.Where(s => s.Title.Contains(SearchString));

            if (!string.IsNullOrEmpty(MovieGenre)) 
                movies = movies.Where(x => x.Genre == MovieGenre);

            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            Movie = await movies.ToListAsync();
        }
    }
}
