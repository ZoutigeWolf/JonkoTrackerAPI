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
        };

        Context.Sessions.Add(session);
        Context.SaveChanges();

        CreateJonkos(session, data.Jonkos);

        return session;
    }

    private void CreateJonkos(Session session, List<Jonko> jonkos)
    {
        foreach (Jonko j in jonkos)
        {
            Jonko jonko = new Jonko()
            {
                Name = j.Name,
                SessionId = session.Id,
            };

            Context.Jonkos.Add(jonko);
            Context.SaveChanges();
            
            CreateIngredients(jonko, j.Ingredients);
        }
    }
    
    private void CreateIngredients(Jonko jonko, List<Ingredient> ingredients)
    {
        foreach (Ingredient i in ingredients)
        {
            Ingredient ingredient = new Ingredient()
            {
                JonkoId = jonko.Id,
                Strain = i.Strain,
                Amount = i.Amount,
            };

            Context.Jonkos.Add(jonko);
            Context.SaveChanges();
        }
    }
}