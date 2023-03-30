using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MSLServer.Logic;
using MSLServer.Models;
using MSLServer.Models.User;
using MSLServer.SecureServices;
using MSLServer.Services.EmailService;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MSLServer.Controllers;

[ApiController, Authorize, Route("[controller]")]
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
    [HttpPost("/register"), AllowAnonymous]
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

    [HttpPost("/login"), AllowAnonymous]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        try
        {
            var user = await repository.LoginUser(request);
            EmailService emailHandlerRepository = new EmailService();
            string token = JWTToken.CreateToken(user);
            var userInfo = new UserInfoDTO() { Email=user.Email, Token=token};
            return Ok(userInfo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("/auth")]
    public async Task<IActionResult> Auth(string email)
    {
        try
        {
            string accessToken = Request.Headers[HeaderNames.Authorization];
            accessToken = accessToken.Split(" ")[1];
            var user = repository.GetByEmail(email);
            var asd = JWTToken.GetTokenValueByType(accessToken, "email");
            if (user.Email != asd)
            {
                return Ok(false);
            }
            return Ok(true);
            
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/verify"), AllowAnonymous]
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
    [HttpPost("/forgotpassword"), AllowAnonymous]
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
    [HttpPost("/resetpassword"), AllowAnonymous]
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

