using Newtonsoft.Json.Linq;

namespace ServerApp.Controllers.Actions.Implements;

public class AddAction : IAction
{
    public string action => "add";

    public object Result(JToken obj)
    {
        var a = obj["a"]!.ToObject<int>();
        var b = obj["b"]!.ToObject<int>();
        
        return new { action, sum = a + b, status = "success" };
    }
}