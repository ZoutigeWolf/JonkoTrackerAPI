using System.Text.Json.Serialization;

namespace JonkoTrackerAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string DisplayName { get; set; }
    [JsonIgnore] public string PasswordHash { get; set; }
    public string Email { get; set; }
}