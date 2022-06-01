using System;
using MongoDB.Driver;
using Engineer.UpdateProfileService.Config;
using Engineer.UpdateProfileService.Model;
using Engineer.UpdateProfileService.Repository.Contracts;

namespace Engineer.UpdateProfileService.Repository.Implementation
{
    public class UpdateProfileContext: IUpdateProfileContext
    {
        private readonly IMongoDatabase _db;

        public UpdateProfileContext(MongoDbConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            _db = client.GetDatabase(config.Database);
        }
        public IMongoCollection<UserProfile> UserProfile => _db.GetCollection<UserProfile>("UserProfile");
    }
}
