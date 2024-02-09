using System.Text.RegularExpressions;
using JonkoTrackerAPI.Models;
using JonkoTrackerAPI.Services;
using JonkoTrackerAPI.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace JonkoTrackerAPI.Handlers;

public class UserHandler : Handler
{
    private readonly string _bucket;
    
    public UserHandler(ILogger logger, DatabaseContext context, IConfiguration configuration) 
        : base(logger, context, configuration)
    {
        this._bucket = Configuration["Storage:Buckets:ProfilePictures"] ?? throw new InvalidConfigurationException();
    }

    public ActionResult<IEnumerable<User>> GetAllUsers()
    {
        IEnumerable<User> users = Services.Users.GetAll();


        return new OkObjectResult(users);
    }

    public ActionResult<User> GetUser(int id)
    {
        User? user = Services.Users.GetById(id);

        if (user == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(user);
    }

    public ActionResult UpdateUser(User? user, int id, UserRegistrationData data)
    {
        if (user == null)
        {
            return new UnauthorizedResult();
        }

        if (user.Id != id)
        {
            return new ForbidResult();
        }

        Services.Users.Update(user, data);

        return new OkResult();
    }

    public ActionResult<UserMetaData> GetMetaData(User? user, int id)
    {
        if (user == null)
        {
            return new UnauthorizedResult();
        }

        User? other = Services.Users.GetById(id);

        if (other == null)
        {
            return new NotFoundResult();
        }

        UserMetaData data = Services.Users.GetMetaData(user, other);

        return new OkObjectResult(data);
    }

    public async Task<ActionResult> GetProfilePicture(int id)
    {
        if (Services.Users.GetById(id) == null)
        {
            return new NotFoundResult();
        }

        Stream? stream = await Services.Storage.Get(_bucket, $"{id.ToString()}.png") ??
                         await Services.Storage.Get(_bucket, "default.png");

        if (stream == null)
        {
            return new NotFoundResult();
        }

        return new FileStreamResult(stream, "application/octet-stream");
    }

    public async Task<ActionResult> UploadProfilePicture(int id, int size, Stream stream)
    {
        if (Services.Users.GetById(id) == null)
        {
            return new NotFoundResult();
        }
        
        if (await Services.Storage.Get(_bucket, $"{id.ToString()}.png") != null)
        {
            await Services.Storage.Delete(_bucket, $"{id.ToString()}.png");
        }
        
        await Services.Storage.Upload(_bucket, $"{id.ToString()}.png", stream, size);
        await stream.DisposeAsync();

        return new OkResult();
    }

    public ActionResult<IEnumerable<Session>> GetAllSessions(int userId)
    {
        IEnumerable<Session> sessions = Services.Sessions.GetAll()
            .Where(s => s.UserId == userId);
        
        return new OkObjectResult(sessions);
    }
}