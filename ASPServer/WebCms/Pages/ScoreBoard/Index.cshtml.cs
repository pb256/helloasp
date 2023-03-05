using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Models.Contexts;
using Models.Model;

namespace WebCms.Pages.ScoreBoard;

public class Index : PageModel
{
    public sealed class UserScoreDto
    {
        public string uid;
        public int rank;
        public int score;
        
        public UserScoreDto(UserScore u, int rank)
        {
            uid = u.uid;
            score = u.score;
            this.rank = rank;
        }
    }
    
    [BindProperty(SupportsGet = true)]
    public int? Page { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }
    
    public IList<UserScoreDto> Score { get; set; } = default!;
    public int TotalCount { get; set; }
    
    public const int ITEM_COUNT_ON_PAGE = 20;

    private readonly GameContext _context;

    public Index(GameContext context)
    {
        _context = context;
    }

    public async Task OnGetAsync()
    {
        var query =
            _context.Set<UserScore>()
                .AsNoTracking();
        TotalCount = await query.CountAsync();

        query = query.OrderByDescending(x => x.score);
        
        var indices = Enumerable.Range(1, TotalCount);
        // async 함수가 없나?
        var rankedQuery = 
            query.AsEnumerable()
                .Zip(indices, (score, i) => new UserScoreDto(score, i));
        
        if (Page.HasValue)
            rankedQuery = rankedQuery.Skip(Math.Max(Page.Value, 0) * ITEM_COUNT_ON_PAGE);
        
        // 갈 곳을 잃음
        if (!string.IsNullOrEmpty(SearchString)) 
            rankedQuery = rankedQuery.Where(x => x.uid.Contains(SearchString));

        Score = rankedQuery.Take(ITEM_COUNT_ON_PAGE).ToList();
    }
}