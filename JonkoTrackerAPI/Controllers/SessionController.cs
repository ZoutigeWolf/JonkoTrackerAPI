using JonkoTrackerAPI.Handlers;
using JonkoTrackerAPI.Models;
using JonkoTrackerAPI.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JonkoTrackerAPI.Controllers;

[ApiController]
[Route("api/sessions")]
public class SessionController : Controller<SessionController, SessionHandler>
{
    public SessionController(ILogger<SessionController> logger, DatabaseContext context, IConfiguration configuration) 
        : base(logger, context, configuration)
    {
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Session>> GetAllSessions() => Handler.GetAllSessions();
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Session> GetSession(int id) => Handler.GetSession(id);

    [HttpPost]
    [Authorize]
    public ActionResult<Session> CreateSession(SessionCreationData data) =>
        Handler.CreateSession(GetUserFromClaims(), data);
}