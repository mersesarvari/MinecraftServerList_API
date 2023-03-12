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
    public IList<User> GetAll()
    {
        return repository.GetAll();
    }

    [HttpGet("{id}")]
    public User Get(Guid id)
    {
        return repository.GetById(id);
    }

    [HttpDelete(Name = "DeleteUser")]
    public void Delete(Guid id)
    {
        repository.Delete(id);
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register(UserRegisterRequest request)
    {
        try
        {
            repository.Register(request);
            return Ok("User succesfully created!");
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }

    }

    [Route("/login")]
    [HttpGet]
    public bool Login(string email, string password)
    {
        return repository.LoginUser(email, password);
    }

    [HttpPut(Name = "UpdateUser")]
    public void Update(User user)
    {
        repository.Update(user);
    }
}

