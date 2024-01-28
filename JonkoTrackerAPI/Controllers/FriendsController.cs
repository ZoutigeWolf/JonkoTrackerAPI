using JonkoTrackerAPI.Handlers;
using JonkoTrackerAPI.Models;
using JonkoTrackerAPI.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JonkoTrackerAPI.Controllers;

[ApiController]
[Route("api/friends")]
public class FriendsController : Controller<FriendsController, FriendsHandler>
{
    public FriendsController(ILogger<FriendsController> logger, DatabaseContext context, IConfiguration configuration) 
        : base(logger, context, configuration)
    {
    }
    
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<User>> GetAllFriends() => Handler.GetAllFriends(GetUserFromClaims());
    
    [HttpPost("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<User> AddFriend(int id) => Handler.AddFriend(GetUserFromClaims(), id);
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<User> RemoveFriend(int id) => Handler.RemoveFriend(GetUserFromClaims(), id);
}