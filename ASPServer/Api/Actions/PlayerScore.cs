using Core;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace Api.Actions;

public class GetPlayerScore : IApiService
{
    private readonly IMemoryCache _memoryCache;

    public GetPlayerScore(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public Task<JObject> ProcessAsync(JObject param)
    {
        var score = 0;
        if (_memoryCache.TryGetValue(Tuple.Create(param.Value<string>("uid"), "score"), out string scoreStr))
            int.TryParse(scoreStr, out score);
        return Task.FromResult(new JObject{ ["score"] = score });
    }
}

public class SetPlayerScore : IApiService
{
    private readonly IMemoryCache _memoryCache;

    public SetPlayerScore(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public Task<JObject> ProcessAsync(JObject param)
    {
        var score = param.Value<int>("score");

        // score 범위 체크
        if (score is not (> 0 and < 10000))
            return Task.FromResult(new JObject { ["status"] = Status.INVALID_REQUEST });
        
        _memoryCache.Set(Tuple.Create(param.Value<string>("uid"), "score"), score.ToString());
        return Task.FromResult(new JObject());
    }
}