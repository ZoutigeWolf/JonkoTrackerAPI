using JonkoTrackerAPI.Handlers;
using JonkoTrackerAPI.Models;
using JonkoTrackerAPI.Types;

namespace JonkoTrackerAPI.Services;

public class UserService : Service
{
    public UserService(DatabaseContext context, IConfiguration configuration, ServicesList services) : base(context, configuration, services)
    {
    }

    public User? GetById(int id) => Context.Users.FirstOrDefault(u => u.Id == id);
    
    public User? GetByUsername(string username) => Context.Users.FirstOrDefault(u => u.Username == username);
    
    public User? GetByEmail(string email) => Context.Users.FirstOrDefault(u => u.Email == email);

    public List<User> GetAll() => Context.Users.ToList();

    public User Create(string username, string displayName, string email, string password)
    {
        if (username == null)
        {
            throw new ArgumentException("Username can't be null");
        }
        
        if (displayName == null)
        {
            throw new ArgumentException("DisplayName can't be null");
        }
        
        if (email == null)
        {
            throw new ArgumentException("Email can't be null");
        }
        
        if (password == null)
        {
            throw new ArgumentException("Password can't be null");
        }

        if (GetByUsername(username) != null)
        {
            throw new ArgumentException("An account with this username already exists");
        }
        
        if (GetByEmail(email) != null)
        {
            throw new ArgumentException("An account with this email already exists");
        }

        User user = new User()
        {
            Username = username,
            DisplayName = displayName,
            PasswordHash = AuthService.HashPassword(password),
            Email = email,
        };

        Context.Users.Add(user);
        Context.SaveChanges();

        return user;
    }

    public void Update(User user, UserRegistrationData data)
    {
        if (IsUsernameAvailable(user, data.Username))
        {
            user.Username = data.Username;
        }

        if (data.DisplayName.Length > 0)
        {
            user.DisplayName = data.DisplayName;
        }

        if (IsEmailAvailable(user, data.Email))
        {
            user.Email = data.Email;
        }
        
        if (AuthHandler.PasswordRegex.IsMatch(data.Password))
        {
            user.PasswordHash = AuthService.HashPassword(data.Password);
        }

        Context.SaveChanges();
    }

    public UserMetaData GetMetaData(User user, User other)
    {
        Services.LoadCollection(user, u => u.Friends);

        bool isFriend = user.Friends.Contains(other);
        
        return new UserMetaData()
        {
            IsFriend = isFriend,
        };
    }
    
    private bool IsUsernameAvailable(User user, string username)
    {
        User? existingUser = Services.Users.GetByUsername(username);

        return existingUser == null || existingUser.Id == user.Id;
    }
    
    private bool IsEmailAvailable(User user, string email)
    {
        User? existingUser = Services.Users.GetByEmail(email);

        return existingUser == null || existingUser.Id == user.Id;
    }
}