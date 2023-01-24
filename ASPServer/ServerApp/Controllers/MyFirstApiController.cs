using Microsoft.AspNetCore.Mvc;

namespace ServerApp.Controllers;

[ApiController] [Route("[controller]")]
public class MyFirstApiController : ControllerBase
{
    // inner class도 되나 싶었는데 된다! 하지만 굳이?
    // 일반 퍼블릭 변수는 안되고 프로퍼티만 되는듯
    public class Model
    {
        public string id { get; set; }
        public int someNumber { get; set; }
        public float someFloat { get; set; }
        public DateTime someDateTime { get; set; }
        public string description { get; set; }
    }

    [HttpGet(Name = "GetModel")]
    public Model Get()
    {
        return new Model
        {
            id = "256",
            someFloat = 3.14f,
            description = "안녕 세상아",
            someDateTime = DateTime.Now,
            someNumber = 1212
        };
    }
    
    // 테스트할때 핫리로드나 비슷한 그런 방법없나?
    // 계속 스웨거가 켜진다 으아
}