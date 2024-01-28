using Minio;
using Minio.DataModel.Args;

namespace JonkoTrackerAPI.Services;

public class StorageService : Service
{
    private IMinioClient _minio;
    public StorageService(DatabaseContext context, IConfiguration configuration, ServicesList services) : base(context, configuration, services)
    {
        _minio = new MinioClient()
            .WithEndpoint(Configuration["Storage:Endpoint"])
            .WithCredentials(Configuration["Storage:AccessKey"], Configuration["Storage:SecretKey"])
            .WithSSL(false)
            .Build();
    }

    public async Task Upload(string bucket, string objectName, Stream stream)
    {
        await _minio.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType("application/octet-stream")).ConfigureAwait(false);
    }

    public async Task<Stream?> Get(string bucket, string objectName)
    {
        MemoryStream stream = new MemoryStream();

        try
        {
            Console.WriteLine(bucket);
            Console.WriteLine(objectName);
            await _minio.GetObjectAsync(
                new GetObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(objectName)
                    .WithCallbackStream(streamReader => { streamReader.CopyTo(stream); }));

            stream.Position = 0;
            return stream;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}