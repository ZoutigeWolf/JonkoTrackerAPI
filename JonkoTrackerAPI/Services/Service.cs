namespace JonkoTrackerAPI.Services;

public abstract class Service
{
    protected readonly ServicesList Services;
    
    protected readonly DatabaseContext Context;

    protected readonly IConfiguration Configuration;

    protected Service(DatabaseContext context, IConfiguration configuration, ServicesList services)
    {
        Context = context;
        Configuration = configuration;
        Services = services;
    }
}