using System.Text.Json.Serialization;

namespace JonkoTrackerAPI.Models;

public class Session
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [JsonIgnore] public User User { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public List<Jonko> Jonkos { get; set; }
}