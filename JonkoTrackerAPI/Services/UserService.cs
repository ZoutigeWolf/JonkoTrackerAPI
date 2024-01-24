using JonkoTrackerAPI.Models;

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
}