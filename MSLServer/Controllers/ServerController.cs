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

    private readonly ILogger<ServerController> _logger;

    public ServerController(ILogger<ServerController> logger, IServerRepository serverRepository, IServerThumbnailRepository thumbnailRepository)
    {
        _logger = logger;
        this.serverRepository = serverRepository;
        this.thumbnailRepository = thumbnailRepository;
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
    public void Post(string ip, string port, string ownerid)
    {
        serverRepository.Insert(ip, port, ownerid);
        serverRepository.GetServerInformation(ip, port);
    }

    [Route("/uploadthumbnail")]
    [HttpPost]
    public IActionResult PostImage(IFormFile _file, string serverid)
    {
        try
        {
            string filePath = Path.Combine(Resource.fileDirectory, serverid + Path.GetExtension(_file.FileName));
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

    [Route("/deletethumbnail")]
    [HttpDelete]
    public IActionResult DeleteImage(string serverid)
    {
        try
        {
            var currentfile = thumbnailRepository.ReadByServerId(serverid);
            if (System.IO.File.Exists(Resource.fileDirectory + currentfile.FullName))
            {
                System.IO.File.Delete(Resource.fileDirectory + currentfile.FullName);
                thumbnailRepository.Delete(currentfile.Name);
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
    public IList<Server> ChectServerStatus()
    {
        var server = serverRepository.GetAll();
        serverRepository.SetServerListInformation(server);
        return server;
    }
}

