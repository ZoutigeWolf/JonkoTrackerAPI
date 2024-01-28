using System.Text.RegularExpressions;
using JonkoTrackerAPI.Models;
using JonkoTrackerAPI.Types;
using Microsoft.AspNetCore.Mvc;

namespace JonkoTrackerAPI.Handlers;

public class FriendsHandler : Handler
{
    public FriendsHandler(ILogger logger, DatabaseContext context, IConfiguration configuration) 
        : base(logger, context, configuration)
    {
    }

    public ActionResult<IEnumerable<User>> GetAllFriends(User? user)
    {
        if (user == null)
        {
            return new UnauthorizedResult();
        }

        Services.LoadCollection(user, u => u.Friends);

        return new OkObjectResult(user.Friends);
    }

    public ActionResult AddFriend(User? user, int friendId)
    {
        if (user == null)
        {
            return new UnauthorizedResult();
        }

        User? friend = Services.Users.GetById(friendId);

        if (friend == null)
        {
            return new NotFoundResult();
        }
        
        Services.Friends.AddFriend(user, friend);

        return new OkResult();
    }
    
    public ActionResult RemoveFriend(User? user, int friendId)
    {
        if (user == null)
        {
            return new UnauthorizedResult();
        }
        
        User? friend = Services.Users.GetById(friendId);

        if (friend == null)
        {
            return new NotFoundResult();
        }
        
        Services.Friends.RemoveFriend(user, friend);

        return new OkResult();
    }
}