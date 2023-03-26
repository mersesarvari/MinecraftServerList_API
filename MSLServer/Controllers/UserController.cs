using Azure;
using Microsoft.AspNetCore.Mvc;
using MSLServer.Logic;
using MSLServer.Models;
using MSLServer.Services.EmailService;
using System;

namespace MSLServer.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserRepository repository;
    private IEmailService emailService;

    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, IUserRepository repository, IEmailService emailService)
    {
        _logger = logger;
        this.repository = repository;
        this.emailService = emailService;
    }
    [HttpPost("/register")]
    public async Task<IActionResult> Register(UserRegisterRequest request)
    {
        try
        {
            await repository.RegisterUser(request);
            var currentuser = repository.GetByEmail(request.Email);
            emailService.SendEmail(new Email()
            {
                To = currentuser.Email,
                Subject = "Minemaina account verification",
                Body = $"<h1>Minemania Account Verification</h1>" +
                $"<br>" +
                $"https://localhost:3000/verify/{currentuser.VerificationToken}"
            });
            return Ok("User succesfully created!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpPost("/login")]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        try
        {
            var id = await repository.LoginUser(request);
            EmailService emailHandlerRepository = new EmailService();
            return Ok(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("/verify")]
    public async Task<IActionResult> Verify(string token)
    {
        try
        {
            await repository.VerifyUser(token);
            return Ok("Verification was succesfull");
        }
        catch (Exception ex)
        {
            return BadRequest("An error occured during the verification.");
        }
    }
    [HttpPost("/forgotpassword")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        try
        {
            await repository.ForgotPassword(email);
            var user = repository.GetByEmail(email);
            if (user == null)
            {
                throw new Exception("Email address was invalid");
            }
            emailService.SendEmail(new Email()
            {
                To = user.Email,
                Subject = "Minemaina password restoration",
                Body = $"<h1>Minemania Password Restoration</h1>" +
                $"<br>" +
                $"https://localhost:3000/resetpassword/{user.PasswordResetToken}"
            });
            return Ok("If the email was correct you will recieve an eamil with the password reset");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("/resetpassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        try
        {
            await repository.ResetPassword(request);
            return Ok("Password reset was succesfull");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

