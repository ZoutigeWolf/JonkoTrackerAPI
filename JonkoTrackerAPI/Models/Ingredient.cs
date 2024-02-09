using System.Text.Json.Serialization;

namespace JonkoTrackerAPI.Models;

public class Ingredient
{
    public int Id { get; set; }
    public int JonkoId { get; set; }
    [JsonIgnore] public Jonko Jonko { get; set; }
    public string Strain { get; set; }
    public decimal Amount { get; set; }
}