using Microsoft.AspNetCore.Mvc;
using MSLServer.Logic;
using MSLServer.Models;

namespace MSLServer.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserRepository repository;

    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, IUserRepository repository)
    {
        _logger = logger;
        this.repository = repository;
    }

    [HttpGet(Name = "GetUserController")]
    public async Task<IList<User>> GetAll()
    {
        try
        {
            return await repository.GetAll();
        }
        catch (Exception ex)
        {
            BadRequest(ex.Message);
            throw new Exception(ex.Message);
        }
        
    }

    [HttpGet("{id}")]
    public async Task<User> Get(string id)
    {
        try
        {
            return await repository.GetById(id);
        }
        catch (Exception ex)
        {
            BadRequest(ex.Message);
            throw new Exception(ex.Message);
        }
        
    }

    [HttpDelete(Name = "DeleteUser")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await repository.Delete(id);
            return Ok("User was succesfully deleted");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register(UserRegisterRequest request)
    {
        try
        {
            await repository.RegisterUser(request);
            return Ok("User succesfully created!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [Route("/login")]
    [HttpPost]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        try
        {
            await repository.LoginUser(request);
            return Ok("Login was succesfull");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut(Name = "UpdateUser")]
    public async Task<IActionResult> Update(User user)
    {
        try
        {
            await repository.Update(user);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
}

