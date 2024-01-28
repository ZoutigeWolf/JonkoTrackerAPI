using System.Text.RegularExpressions;
using JonkoTrackerAPI.Models;
using JonkoTrackerAPI.Types;
using Microsoft.AspNetCore.Mvc;

namespace JonkoTrackerAPI.Handlers;

public class AuthHandler : Handler
{
    public static Regex PasswordRegex =
        new Regex(@"/^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,32}$/");
    
    public AuthHandler(ILogger logger, DatabaseContext context, IConfiguration configuration) 
        : base(logger, context, configuration)
    {
    }
    
    public ActionResult Login(UserCredentials credentials)
    {
        User? user = Services.Auth.AuthenticateUser(credentials);

        if (user == null)
        {
            return new UnauthorizedResult();
        }

        string token = Services.Auth.GenerateJsonWebToken(user);
        
        return new OkObjectResult(new { token });
    }

    public ActionResult Register(UserRegistrationData data)
    {
        User? existingUser = Services.Users.GetByUsername(data.Username);

        if (existingUser != null)
        {
            return new BadRequestObjectResult("username-exists");
        }

        existingUser = Services.Users.GetByEmail(data.Email);
        
        if (existingUser != null)
        {
            return new BadRequestObjectResult("email-exists");
        }

        if (!PasswordRegex.IsMatch(data.Password))
        {
            return new BadRequestObjectResult("invalid-password");
        }

        Services.Users.Create(data.Username, data.DisplayName, data.Email, data.Password);

        return new OkResult();
    }
    
    public static ActionResult<User> GetUser(User? user)
    {
        if (user == null)
        {
            return new NotFoundResult();
        }
        
        return new OkObjectResult(user);    
    }
}