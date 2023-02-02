using Newtonsoft.Json.Linq;

namespace ServerApp.Controllers.Actions.Implements;

public class EchoAction : IAction
{
    public string action => "echo";
    public object Result(JToken obj)
    {
        var text = obj["text"]!.ToString();
        return new { action, text, status = "success" };
    }
}