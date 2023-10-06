using MongoExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using pergisafar.Shared.Models;

namespace MongoExample.Services;

public class MongoDBService
{

    private readonly IMongoCollection<User> _playlistCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _playlistCollection = database.GetCollection<User>(mongoDBSettings.Value.CollectionName);
    }

}