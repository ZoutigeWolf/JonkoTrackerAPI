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

    public async Task Upload(string bucket, string objectName, Stream stream, int size)
    {
        await _minio.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(size)
                .WithContentType("application/octet-stream")).ConfigureAwait(false);
    }

    public async Task<Stream?> Get(string bucket, string objectName)
    {
        MemoryStream stream = new MemoryStream();

        try
        {
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
            return null;
        }
    }

    public async Task Delete(string bucket, string objectName)
    {
        await _minio.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName));
    }
}