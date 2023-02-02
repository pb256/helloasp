using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerApp.Controllers.Actions;

namespace ServerApp.Controllers;

[ApiController] [Route("[controller]")]
public class ActionController : ControllerBase
{
    // 나의 구세주
    // https://stackoverflow.com/questions/20226169/how-to-pass-json-post-data-to-web-api-method-as-an-object
    [HttpPost]
    public object[] Post([FromBody] MultipleAction m)
    {
        var resultArray =
            m.actions.Select(x =>
            {
                var actions = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(_ => _.GetTypes())
                    .Where(_ => typeof(IAction).IsAssignableFrom(_) && _ != typeof(IAction)) 
                    .Select(Activator.CreateInstance)
                    .Cast<IAction>();

                var actionName = x["action"]?.ToString();

                var failResult = new { action = actionName, status = "INVALID_REQUEST" };
                var act = actions.FirstOrDefault(_ =>  string.Equals(_.action, actionName, StringComparison.InvariantCultureIgnoreCase));
                try
                {
                    return act!.Result(x);
                }
                catch (Exception)
                {
                    return failResult;
                }
            }).ToArray();
        
        return resultArray;
    }
}

public class MultipleAction
{
    public int seqid { get; set; }
    public string uid { get; set; }
    public JArray actions { get; set; }
}
