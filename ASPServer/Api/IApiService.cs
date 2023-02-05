using Newtonsoft.Json.Linq;

namespace Api;

public interface IApiService
{
    Task<JObject> ProcessAsync(JObject param);
}