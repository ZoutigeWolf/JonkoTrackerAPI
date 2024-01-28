using System.Text.RegularExpressions;
using JonkoTrackerAPI.Models;
using JonkoTrackerAPI.Types;
using Microsoft.AspNetCore.Mvc;

namespace JonkoTrackerAPI.Handlers;

public class UserHandler : Handler
{
    public UserHandler(ILogger logger, DatabaseContext context, IConfiguration configuration) 
        : base(logger, context, configuration)
    {
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

    public async Task<ActionResult> GetProfilePicture(int id)
    {
        if (Services.Users.GetById(id) == null)
        {
            return new NotFoundResult();
        }

        Stream stream = await Services.Storage.Get(Configuration["Storage:Buckets:ProfilePictures"], id.ToString());

        if (stream == null)
        {
            stream = await Services.Storage.Get(Configuration["Storage:Buckets:ProfilePictures"], "default");
        }

        if (stream == null)
        {
            return new NotFoundResult();
        }

        return new FileStreamResult(stream, "application/octet-stream");
    }

    public async Task<ActionResult> UploadProfilePicture(int id, IFormFile file)
    {
        if (Services.Users.GetById(id) == null)
        {
            return new NotFoundResult();
        }
        
        if (file.Length == 0)
        {
            return new BadRequestResult();
        }

        await using Stream stream = file.OpenReadStream();
        await Services.Storage.Upload(Configuration["Storage:Buckets:ProfilePictures"], id.ToString(), stream);

        return new OkResult();
    }
}