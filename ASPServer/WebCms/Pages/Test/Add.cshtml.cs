using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Contexts;
using Newtonsoft.Json.Linq;

namespace WebCms.Pages.Test;

public class Add : PageModel
{
    private readonly Api.Actions.Add _addApi;
    private readonly GameContext _context;
    public string testText;

    [BindProperty]
    public string valueA { get; set; } = string.Empty;
    
    [BindProperty]
    public string valueB { get; set; } = string.Empty;
    
    public string valueC { get; set; } = string.Empty;

    public Add(Api.Actions.Add addApi, GameContext context)
    {
        _addApi = addApi;
        _context = context;
    }
    
    public void OnGet()
    {
        testText = $"hello world! {DateTime.Now}";
    }

    public async Task OnPostAdd()
    {
        var result = await _addApi.ProcessAsync(new JObject { ["a"] = valueA, ["b"] = valueB });
        var sum = result.Value<int>("sum");
        valueC = $"{sum}";
    }
}