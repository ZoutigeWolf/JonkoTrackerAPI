using JonkoTrackerAPI.Handlers;
using JonkoTrackerAPI.Models;
using JonkoTrackerAPI.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JonkoTrackerAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : Controller<UserController, UserHandler>
{
    public UserController(ILogger<UserController> logger, DatabaseContext context, IConfiguration configuration) 
        : base(logger, context, configuration)
    {
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<User>> GetAllUsers() => Handler.GetAllUsers();
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<User> GetUser(int id) => Handler.GetUser(id);

    [HttpPut("{id:int}")]
    [Authorize]
    public ActionResult UpdateUser(int id, UserRegistrationData data) => Handler.UpdateUser(GetUserFromClaims(), id, data);
    
    [HttpGet("{id:int}/meta-data")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<UserMetaData> GetMetaData(int id) => Handler.GetMetaData(GetUserFromClaims(), id);

    [HttpGet("{id:int}/profile-picture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetProfilePicture(int id) => await Handler.GetProfilePicture(id);
    
    [HttpPost("{id:int}/profile-picture")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UploadProfilePicture(int id) => await Handler.UploadProfilePicture(id, Request.BodyReader.AsStream());
}