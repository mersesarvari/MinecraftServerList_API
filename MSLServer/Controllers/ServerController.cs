using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MSLServer.Logic;
using MSLServer.Models;
using MSLServer.SecureServices;
using System.IO;

namespace MSLServer.Controllers;

[ApiController, Route("[controller]"), Authorize]
public class ServerController : ControllerBase
{
    private IServerRepository serverRepository;
    private IUserRepository userRepository;
    private IServerThumbnailRepository thumbnailRepository;
    private IServerLogoRepository logoRepository;
    private readonly ILogger<ServerController> logger;

    public ServerController(ILogger<ServerController> _logger, IServerRepository serverRepository, IServerThumbnailRepository thumbnailRepository, IServerLogoRepository logoRepository, IUserRepository _userRepository)
    {
        this.logger = _logger;
        this.serverRepository = serverRepository;
        this.thumbnailRepository = thumbnailRepository;
        this.logoRepository = logoRepository;
        this.userRepository = _userRepository;
    }
    [HttpGet, AllowAnonymous]
    public IList<Server> GetAll()
    {
        var servers =  serverRepository.GetAll();
        return servers.OrderByDescending(x => x.CurrentPlayers).ToList();
        
    }

    [HttpGet, AllowAnonymous, Route("/premiumserver")]
    public IList<Server> GetAllPremium()
    {
        var servers = serverRepository.GetPremiumServers();
        return servers;

    }

    [HttpGet("{id}"), AllowAnonymous]
    public Server Get(string id)
    {
        return serverRepository.GetById(id);
    }
    [HttpPost]
    public IActionResult Post([FromForm]CreateServerDTO server)
    {
        try
        {
            string accessToken = Request.Headers[HeaderNames.Authorization];
            accessToken = accessToken.Split(" ")[1];
            var emailAddress = JWTToken.GetTokenValueByType(accessToken, "email");
            var user = userRepository.GetByEmail(emailAddress);
            server.Publisherid = user.Id;
            serverRepository.Insert(server);
            if (server.BedrockIp != "")
            {
                var s = serverRepository.GetByIp(server.BedrockIp);
                serverRepository.CheckServerStatus(s);
            }
            if (server.JavaIp != "")
            {
                var s = serverRepository.GetByIp(server.JavaIp);
                serverRepository.CheckServerStatus(s);
            }
            return Ok("Server created succesfully");
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
        

        
    }

    [Route("/uploadthumbnail")]
    [HttpPost]
    public IActionResult PostThumbnail(IFormFile _file, string serverid)
    {
        try
        {
            string filePath = Path.Combine(Resource.thumbnailDirectory, serverid + Path.GetExtension(_file.FileName));
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                _file.CopyToAsync(fileStream);
                var extension = Path.GetExtension(filePath);
                var filename = serverid + extension;
                thumbnailRepository.Create(new ServerThumbnail() { Name = serverid, FullName = filename, Extension = extension, ServerId = serverid });
            }

            return Ok();
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [Route("/uploadlogo")]
    [HttpPost]
    public IActionResult PostLogo(IFormFile _file, string serverid)
    {
        try
        {
            string filePath = Path.Combine(Resource.logoDirectory, serverid + Path.GetExtension(_file.FileName));
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                _file.CopyToAsync(fileStream);
                var extension = Path.GetExtension(filePath);
                var filename = serverid + extension;
                logoRepository.Create(new ServerLogo() { Name = serverid, FullName = filename, Extension = extension, ServerId = serverid });
            }

            return Ok();
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [Route("/deletethumbnail")]
    [HttpDelete]
    public IActionResult DeleteThumbnail(string serverid)
    {
        try
        {
            var currentfile = thumbnailRepository.ReadByServerId(serverid);
            if (System.IO.File.Exists(Resource.thumbnailDirectory + currentfile.FullName))
            {
                System.IO.File.Delete(Resource.thumbnailDirectory + currentfile.FullName);
                thumbnailRepository.Delete(currentfile.Name);
            }
            return Ok();
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [Route("/deletelogo")]
    [HttpDelete]
    public IActionResult Deletelogo(string serverid)
    {
        try
        {
            var currentfile = logoRepository.ReadByServerId(serverid);
            if (System.IO.File.Exists(Resource.logoDirectory + currentfile.FullName))
            {
                System.IO.File.Delete(Resource.logoDirectory + currentfile.FullName);
                logoRepository.Delete(currentfile.Name);
            }
            return Ok();
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [Route("/getthumbnaildata")]
    [HttpGet]
    public IList<ServerThumbnail> GetThumbnails()
    {
        try
        {
            return thumbnailRepository.ReadAll();
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }

    [Route("/thumbnail"), HttpGet, AllowAnonymous]
    public IActionResult GetThumbnail(string id)
    {
        
        try
        {            
            var server = serverRepository.GetById(id);
            var path = Resource.thumbnailDirectory + "/" + server.ThumbnailPath;
            return File(System.IO.File.OpenRead(path),contentType:$"video/{Path.GetFileName(path)}");
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }
    [Route("/logo"), HttpGet, AllowAnonymous]
    public IActionResult GetLogo(string id)
    {

        try
        {
            var server = serverRepository.GetById(id);
            var path = Resource.logoDirectory + "/" + server.LogoPath;
            return File(System.IO.File.OpenRead(path), contentType: $"image/{Path.GetFileName(path)}");
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [Route("/getlogodata")]
    [HttpGet]
    public IList<ServerLogo> GetLogoData()
    {
        try
        {
            return logoRepository.ReadAll();
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }

    [HttpDelete(Name = "DeleteServer")]
    public void Delete(string id)
    {
        serverRepository.Delete(id);
    }


    [HttpPut(Name = "UpdateServer")]
    public void Update(Server server)
    {
        serverRepository.Update(server);
    }

    [Route("/status")]
    [HttpGet]
    public IActionResult ChectServerStatus(string hostname, string port)
    {
        try
        {
            Console.WriteLine(serverRepository.GetServerStatus(hostname, port));
            return Ok(serverRepository.GetServerStatus(hostname, port));
            
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
            
        }
        
        
    }
}

