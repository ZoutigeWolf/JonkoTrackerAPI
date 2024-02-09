using JonkoTrackerAPI.Models;

namespace JonkoTrackerAPI.Types;

public class SessionCreationData
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public List<Jonko> Jonkos { get; set; }
}