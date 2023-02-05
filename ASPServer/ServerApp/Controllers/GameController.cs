using Api;
using Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ServerApp.Services;

namespace ServerApp.Controllers;

[Route("[Controller]")]
public class GameController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RequestContext _requestContext;

    public GameController(
        IServiceProvider serviceProvider,
        RequestContext requestContext)
    {
        _serviceProvider = serviceProvider;
        _requestContext = requestContext;
    }

    public async Task<string> PostAsync()
    {
        var uid = _requestContext.Uid ?? string.Empty;
        var actions = _requestContext.Actions ?? Array.Empty<JObject>();
        var response = new JArray();
        
        foreach (var actionJson in actions)
        {
            actionJson["uid"] = uid;
            var resultJson = await ProcessActionAsync(actionJson);
            response.Add(resultJson);
        }

        return response.ToString();
    }

    private async Task<JObject> ProcessActionAsync(JObject json)
    {
        var action = json.Value<string>("action");

        var apiService = _serviceProvider.GetApiService(action);
        var response = apiService == default
                           ? new JObject { ["status"] = Status.INVALID_REQUEST }
                           : await apiService.ProcessAsync(json);

        response["action"] = action;
        
        if (response.ContainsKey("status") == false)
            response["status"] = Status.SUCCESS;

        return response;
    }
}