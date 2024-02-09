using JonkoTrackerAPI.Models;
using JonkoTrackerAPI.Types;

namespace JonkoTrackerAPI.Services;

public class SessionService : Service
{
    public SessionService(DatabaseContext context, IConfiguration configuration, ServicesList services) : base(context, configuration, services)
    {
    }

    public List<Session> GetAll() => Context.Sessions.ToList();

    public Session? GetById(int id) => Context.Sessions.FirstOrDefault(s => s.Id == id);

    public Session Create(User user, SessionCreationData data)
    {
        Session session = new Session()
        {
            UserId = user.Id,
            Latitude = data.Latitude,
            Longitude = data.Longitude,
            Jonkos = data.Jonkos
        };

        Context.Sessions.Add(session);
        Context.SaveChanges();

        return session;
    }
}