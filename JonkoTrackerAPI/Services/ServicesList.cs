using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JonkoTrackerAPI.Services;

public class ServicesList
{
    protected readonly DatabaseContext Context;

    protected readonly IConfiguration Configuration;

    public readonly UserService Users;
    public readonly AuthService Auth;
    public readonly StorageService Storage;
    public readonly FriendsService Friends;
    public readonly SessionService Sessions;
    
    
    public ServicesList(DatabaseContext context, IConfiguration configuration)
    {
        Context = context;
        Configuration = configuration;

        Users = new UserService(context, configuration, this);
        Auth = new AuthService(context, configuration, this);
        Storage = new StorageService(context, configuration, this);
        Friends = new FriendsService(context, configuration, this);
        Sessions = new SessionService(context, configuration, this);
    }

    public void DetachEntity<TEntity>(TEntity entity) where TEntity : notnull
    {
        Context.Entry(entity).State = EntityState.Detached;
    }
    
    public IEnumerable<TProperty> LoadCollection<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> selector) 
        where TEntity : class
        where TProperty : class
    {
        CollectionEntry<TEntity, TProperty> collection = Context.Entry(entity).Collection(selector);
        collection.Load();

        return collection.CurrentValue ?? new List<TProperty>();
    }


    public TProperty? LoadReference<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> selector) 
        where TEntity : class
        where TProperty : class 
    {
        ReferenceEntry<TEntity, TProperty> reference = Context.Entry(entity).Reference(selector);
        reference.Load();

        return reference.CurrentValue;
    }
}