using System.Text.RegularExpressions;
using JonkoTrackerAPI.Models;
using JonkoTrackerAPI.Services;
using JonkoTrackerAPI.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace JonkoTrackerAPI.Handlers;

public class SessionHandler : Handler
{
    public SessionHandler(ILogger logger, DatabaseContext context, IConfiguration configuration) 
        : base(logger, context, configuration)
    {
        
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
}