using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace WebCms.Pages.Test;

public class Add : PageModel
{
    private Api.Actions.Add _addApi;
    public string testText;

    public string valueA { get; set; } = string.Empty;
    public string valueB { get; set; } = string.Empty;
    public string valueC { get; set; } = string.Empty;

    public Add(Api.Actions.Add addApi)
    {
        _addApi = addApi;
    }
    
    public void OnGet()
    {
        testText = $"hello world! {DateTime.Now}";
    }

    public async Task OnPostAdd()
    {
        _addApi = new Api.Actions.Add();
        var result = await _addApi.ProcessAsync(new JObject { ["a"] = valueA, ["b"] = valueB });
        var sum = result.Value<int>(result);
        valueC = $"{sum}";
    }
}