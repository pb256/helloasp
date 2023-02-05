using Newtonsoft.Json.Linq;

namespace Api.Actions;

public class Echo : IApiService
{
    public Task<JObject> ProcessAsync(JObject param)
    {
        var text = param.Value<string>("text");
        
        return Task.FromResult(new JObject { ["text"] = text });
    }
}