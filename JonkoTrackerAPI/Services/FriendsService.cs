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
        Services.LoadCollection(friend, u => u.Friends);

        if (!user.Friends.Contains(friend))
        {
            user.Friends.Add(friend);
        }
        
        if (!friend.Friends.Contains(user))
        {
            friend.Friends.Add(user);
        }

        Context.SaveChanges();
    }
    
    public void RemoveFriend(User user, User friend)
    {
        Services.LoadCollection(user, u => u.Friends);
        Services.LoadCollection(friend, u => u.Friends);

        if (user.Friends.Contains(friend))
        {
            user.Friends.Remove(friend);
        }
        
        if (friend.Friends.Contains(user))
        {
            friend.Friends.Remove(user);
        }

        Context.SaveChanges();
    }
}