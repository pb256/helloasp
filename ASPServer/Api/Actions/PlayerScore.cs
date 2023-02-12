using Core;
using Models.Contexts;
using Models.Model;
using Newtonsoft.Json.Linq;

namespace Api.Actions;

public class GetPlayerScore : IApiService
{
    private readonly GameContext _game;

    public GetPlayerScore(GameContext game)
    {
        _game = game;
    }
    
    public async Task<JObject> ProcessAsync(JObject param)
    {
        var uid = param.Value<string>("uid");

        var highScoreData = await _game.FindAsync<UserScore>(uid);
        return new JObject{ ["score"] = highScoreData?.score ?? 0 };
    }
}

public class SetPlayerScore : IApiService
{
    private readonly GameContext _game;

    public SetPlayerScore(GameContext game)
    {
        _game = game;
    }
    
    public async Task<JObject> ProcessAsync(JObject param)
    {
        var score = param.Value<int>("score");
        var uid = param.Value<string>("uid");
        
        if (score is not (> 0 and < 10000))
            return new JObject { ["status"] = Status.INVALID_REQUEST };
        
        if (uid == default)
            return new JObject { ["status"] = Status.INVALID_REQUEST };
        
        var highScoreData = await _game.FindAsync<UserScore>(uid);
        if (highScoreData == default)
        {
            highScoreData = new UserScore
            {
                uid = uid,
                score = 0,
                time_stamp = DateTime.UtcNow
            };
            await _game.AddAsync(highScoreData);
        }

        highScoreData.score = Math.Max(highScoreData.score, score);
        await _game.SaveChangesAsync();
        
        return new JObject();
    }
}