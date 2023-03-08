using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Contexts;
using Models.Model;

namespace WebCms.Pages.ScoreBoard;

public class Create : PageModel
{
    public class Data
    {
        public string rawString { get; set; }

        public IList<UserScore> BuildFromString()
        {
            var lines = rawString.Split("\n");
            return lines.Select(_ =>
            {
                var tokens = _.Split(',');
                return new UserScore
                {
                    uid = tokens[0],
                    score = int.Parse(tokens[1])
                };
            }).ToArray();
        }
    }
    
    [BindProperty]
    public Data data { get; set; } = default!;
    
    private readonly GameContext _context;

    public Create(GameContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var records = data.BuildFromString();
        if (records.Count > 0)
        {
            _context.Set<UserScore>().AddRange(records);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage("./Index");
    }
}