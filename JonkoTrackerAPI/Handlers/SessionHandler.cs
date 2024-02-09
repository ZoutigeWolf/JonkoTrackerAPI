using JonkoTrackerAPI.Models;
using JonkoTrackerAPI.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace JonkoTrackerAPI.Handlers;

public class SessionHandler : Handler
{
    private readonly string _sessionBucket;
    private readonly string _jonkoBucket;
    
    public SessionHandler(ILogger logger, DatabaseContext context, IConfiguration configuration) 
        : base(logger, context, configuration)
    {
        this._sessionBucket = Configuration["Storage:Buckets:Sessions"] ?? throw new InvalidConfigurationException();
        this._jonkoBucket = Configuration["Storage:Buckets:Jonkos"] ?? throw new InvalidConfigurationException();

    }

    public ActionResult<IEnumerable<Session>> GetAllSessions()
    {
        return new OkObjectResult(Services.Sessions.GetAll());
    }

    public ActionResult<Session> GetSession(int id)
    {
        Session? session = Services.Sessions.GetById(id);

        if (session == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(session);
    }

    public ActionResult<Session> CreateSession(User? user, SessionCreationData data)
    {
        if (user == null)
        {
            return new UnauthorizedResult();
        }

        Session session = Services.Sessions.Create(user, data);

        return new CreatedAtActionResult(
            "GetSession", 
            "Session", 
            new { id = session.Id }, 
            session);
    }
    
    public async Task<ActionResult> GetPicture(User? user, int id, int n)
    {
        if (user == null)
        {
            return new UnauthorizedResult();
        }

        Session? session = Services.Sessions.GetById(id);
        
        if (session == null)
        {
            return new NotFoundResult();
        }

        if (session.UserId != user.Id)
        {
            return new ForbidResult();
        }

        Stream? stream = await Services.Storage.Get(_sessionBucket, $"{id.ToString()}_{n.ToString()}.png");

        if (stream == null)
        {
            return new NotFoundResult();
        }

        return new FileStreamResult(stream, "application/octet-stream");
    }

    public async Task<ActionResult> UploadPicture(User? user, int id, int n, int size, Stream stream)
    {
        if (user == null)
        {
            return new UnauthorizedResult();
        }

        Session? session = Services.Sessions.GetById(id);
        
        if (session == null)
        {
            return new NotFoundResult();
        }

        if (session.UserId != user.Id)
        {
            return new ForbidResult();
        }
        
        if (await Services.Storage.Get(_sessionBucket, $"{id.ToString()}_{n.ToString()}.png") != null)
        {
            return new ConflictResult();
        }
        
        await Services.Storage.Upload(_sessionBucket, $"{id.ToString()}_{n.ToString()}.png", stream, size);
        await stream.DisposeAsync();

        return new OkResult();
    }

    public ActionResult<IEnumerable<Jonko>> GetJonkos(int id)
    {
        Session? session = Services.Sessions.GetById(id);

        if (session == null)
        {
            return new NotFoundResult();
        }

        Services.LoadCollection(session, s => s.Jonkos);

        return new OkObjectResult(session.Jonkos);
    }
    
    public async Task<ActionResult> GetJonkoPicture(User? user, int sessionId, int jonkoId)
    {
        if (user == null)
        {
            return new UnauthorizedResult();
        }

        Session? session = Services.Sessions.GetById(sessionId);
        
        if (session == null)
        {
            return new NotFoundResult();
        }

        if (session.UserId != user.Id)
        {
            return new ForbidResult();
        }
        
        Services.LoadCollection(session, s => s.Jonkos);

        Jonko? jonko = session.Jonkos.FirstOrDefault(j => j.Id == jonkoId);

        if (jonko == null)
        {
            return new NotFoundResult();
        }

        Stream? stream = await Services.Storage.Get(_jonkoBucket, $"{jonkoId.ToString()}.png");

        if (stream == null)
        {
            return new NotFoundResult();
        }

        return new FileStreamResult(stream, "application/octet-stream");
    }

    public async Task<ActionResult> UploadJonkoPicture(User? user, int sessionId, int jonkoId, int size, Stream stream)
    {
        if (user == null)
        {
            return new UnauthorizedResult();
        }

        Session? session = Services.Sessions.GetById(sessionId);
        
        if (session == null)
        {
            return new NotFoundResult();
        }

        if (session.UserId != user.Id)
        {
            return new ForbidResult();
        }

        Services.LoadCollection(session, s => s.Jonkos);

        Jonko? jonko = session.Jonkos.FirstOrDefault(j => j.Id == jonkoId);

        if (jonko == null)
        {
            return new NotFoundResult();
        }
        
        if (await Services.Storage.Get(_jonkoBucket, $"{jonkoId.ToString()}.png") != null)
        {
            return new ConflictResult();
        }
        
        await Services.Storage.Upload(_jonkoBucket, $"{jonkoId.ToString()}.png", stream, size);
        await stream.DisposeAsync();

        return new OkResult();
    }
}