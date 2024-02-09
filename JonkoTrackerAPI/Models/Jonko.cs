using System.Text.Json.Serialization;

namespace JonkoTrackerAPI.Models;

public class Jonko
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SessionId { get; set; }
    [JsonIgnore] public Session? Session { get; set; }
    public List<Ingredient> Ingredients { get; set; }
}