using JonkoTrackerAPI.Models;

namespace JonkoTrackerAPI.Services;

public class FriendsService : Service
{
    public FriendsService(DatabaseContext context, IConfiguration configuration, ServicesList services) : base(context,
        configuration, services)
    {
    }

    public IEnumerable<User> GetAllFriends(User user)
    {
        Services.LoadCollection(user, u => u.Friends);

        return user.Friends;
    }

    public void AddFriend(User user, User friend)
    {
        Services.LoadCollection(user, u => u.Friends);

        if (user.Friends.Contains(friend))
        {
            return;
        }
        
        user.Friends.Add(friend);

        Context.SaveChanges();
    }
    
    public void RemoveFriend(User user, User friend)
    {
        Services.LoadCollection(user, u => u.Friends);

        if (!user.Friends.Contains(friend))
        {
            return;
        }
        
        user.Friends.Remove(friend);

        Context.SaveChanges();
    }
}