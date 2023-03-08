using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Models.Contexts;
using Models.Model;

namespace WebCms.Pages.ScoreBoard;

public class Edit : PageModel
{
    private readonly GameContext _context;

    public Edit(GameContext context)
    {
        _context = context;
    }
    
    [BindProperty]
    public UserScore UserScore { get; set; } = default!;
    
    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id == null)
            return NotFound();

        var score = _context.Set<UserScore>().FirstOrDefault(x => x.uid == id);
        if (score == null)
            return NotFound();
        UserScore = score;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();
        
        _context.Attach(UserScore).State = EntityState.Modified;
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Set<UserScore>().Any(_ => _.uid == UserScore.uid))
                return NotFound();
            throw;
        }

        return RedirectToPage("./Index");
    }
    
    public async Task<IActionResult> OnPostRemoveAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var id = UserScore.uid;
        var found = await 
            _context.Set<UserScore>().FirstOrDefaultAsync(_ => _.uid == id);
        
        if (found != null)
        {
            _context.Set<UserScore>().Remove(found);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage("./Index");
    }
}