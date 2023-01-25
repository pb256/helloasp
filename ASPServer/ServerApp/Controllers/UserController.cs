using Microsoft.AspNetCore.Mvc;
using ServerApp.Models;
using ServerApp.Services;

namespace ServerApp.Controllers;

[ApiController]
[Route("myapi/[controller]")]
public class UserController : ControllerBase
{
    public UserController() { }

    // 서비스를 통해서 호출해야하는 이유가 뭘까
    // 메인 로직을 Controller에 두면 안 되는 이유?

    // CURD 작업
    // Get action 읽기 R
    [HttpGet]
    public ActionResult<List<User>> GetAll() => UserService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var user = UserService.Get(id);
        if (user == null)
            return NotFound();
        return user;
    }

    // POST action 생성 C
    // 컨트롤러에 [ApiController] 특성이 주석으로 지정되어 있기 때문에 User 매개 변수를 요청 본문에서 찾을 수 있음을 암시합니다
    [HttpPost]
    public IActionResult Create(User user)
    {
        UserService.Add(user);
        
        // 201, 생성된 user를 HTTP 요청헤더에 정의된 대로 미디어 유형의 응답 본문에 해당 user가 포함됨
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    // PUT action 업데이트 U
    [HttpPut("{id}")]
    public IActionResult Update(int id, User user)
    {
        if (id != user.Id)
            return BadRequest(); // 400

        var existingUser = UserService.Get(id);
        if (existingUser is null)
            return NotFound(); // 400

        UserService.Update(user);

        // NoContent 204
        // 빈 응답
        return NoContent();
    }

    // DELETE action 삭제 D
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var user = UserService.Get(id);
        if (user is null)
            return NotFound();

        UserService.Delete(id);
        return NoContent();
    }
}