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

    [HttpGet("{id:int}/pictures/{n:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetSessionPictures(int id, int n) =>
        await Handler.GetPicture(GetUserFromClaims(), id, n);

    [HttpPost("{id:int}/pictures/{n:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UploadSessionPictures(int id, int n, int size) =>
        await Handler.UploadPicture(GetUserFromClaims(), id, n, size, Request.BodyReader.AsStream());

    [HttpGet("{id:int}/jonkos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Jonko>> GetSessionJonkos(int id) => Handler.GetJonkos(id);

    [HttpGet("{id:int}/jonkos/{jonkoId:int}/picture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetSessionJonkoPicture(int id, int jonkoId) =>
        await Handler.GetJonkoPicture(GetUserFromClaims(), id, jonkoId);

    [HttpGet("{id:int}/jonkos/{jonkoId:int}/picture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UploadSessionJonkoPicture(int id, int jonkoId, int size) =>
        await Handler.UploadJonkoPicture(GetUserFromClaims(), id, jonkoId, size, Request.BodyReader.AsStream());
}