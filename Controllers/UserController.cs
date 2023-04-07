using Microsoft.AspNetCore.Mvc;
using UserApi.Models;
using UserApi.Models.Dto;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{

    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger,
                          IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        return Ok(await _userService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> Get(int id)
    {
        return Ok(await _userService.Get(id));
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create([FromBody] CreateUserDto user)
    {
        return Ok(await _userService.Create(user));
    }

    [HttpPut]
    public async Task<ActionResult<User>> Update([FromBody] UpdateUserDto user)
    {
        return Ok(await _userService.Update(user));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _userService.Delete(id);
        return NoContent();
    }

}
