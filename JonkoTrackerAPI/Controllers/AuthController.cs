using JonkoTrackerAPI.Handlers;
using JonkoTrackerAPI.Models;
using JonkoTrackerAPI.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JonkoTrackerAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller<AuthController, AuthHandler>
{
    public AuthController(ILogger<AuthController> logger, DatabaseContext context, IConfiguration configuration) 
        : base(logger, context, configuration)
    {
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult Login(UserCredentials credentials) => Handler.Login(credentials);

    [HttpGet("current-user")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<User> GetUser() => AuthHandler.GetUser(GetUserFromClaims());
}