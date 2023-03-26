using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MSLServer.Logic;
using MSLServer.Models;

namespace MSLServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ServerController : ControllerBase
{
    private IServerRepository serverRepository;
    private IServerThumbnailRepository thumbnailRepository;
    private IServerLogoRepository logoRepository;

    private readonly ILogger<ServerController> _logger;

    public ServerController(ILogger<ServerController> logger, IServerRepository serverRepository, IServerThumbnailRepository thumbnailRepository, IServerLogoRepository logoRepository)
    {
        _logger = logger;
        this.serverRepository = serverRepository;
        this.thumbnailRepository = thumbnailRepository;
        this.logoRepository = logoRepository;
    }

    [HttpGet(Name = "GetServerController")]
    public IList<Server> GetAll()
    {
        return serverRepository.GetAll();
    }

    [HttpGet("{id}")]
    public Server Get(string id)
    {
        return serverRepository.GetById(id);
    }
    [HttpPost]
    public IActionResult Post([FromForm]CreateServerDTO server)
    {
        try
        {
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

    [Route("/getthumbnails")]
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

    [Route("/getlogo")]
    [HttpGet]
    public IList<ServerLogo> GetLogo()
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

    [Route("/statusall")]
    [HttpGet]
    public IList<Server> ChectServerStatus()
    {
        var server = serverRepository.GetAll();
        serverRepository.CheckSpecificServersStatus(server);
        return server;
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

