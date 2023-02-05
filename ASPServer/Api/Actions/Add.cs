using Newtonsoft.Json.Linq;

namespace Api.Actions;

public class Add : IApiService
{
    public Task<JObject> ProcessAsync(JObject param)
    {
        var a = param.Value<int>("a");
        var b = param.Value<int>("b");
        
        return Task.FromResult(new JObject { ["sum"] = a + b });
    }
}