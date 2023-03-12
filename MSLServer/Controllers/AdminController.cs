using Microsoft.AspNetCore.Mvc;
using MSLServer.Logic;
using MSLServer.Models;
using MSLServer.Services.EmailService;

namespace MSLServer.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private IUserRepository repository;
    private IEmailService emailService;

    private readonly ILogger<UserController> _logger;

    public AdminController(ILogger<UserController> logger, IUserRepository repository, IEmailService emailService)
    {
        _logger = logger;
        this.repository = repository;
        this.emailService = emailService;
    }

    [HttpGet("GetUserController")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(repository.GetAll());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            return Ok(repository.GetById(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
    [HttpDelete("DeleteUser")]
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

    [HttpPut("UpdateUser")]
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

