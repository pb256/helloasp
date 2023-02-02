using Newtonsoft.Json.Linq;

namespace ServerApp.Controllers.Actions;

public interface IAction
{
    string action { get; }
    object Result(JToken obj);
}