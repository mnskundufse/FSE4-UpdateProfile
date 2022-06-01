using System;
using MongoDB.Driver;
using Engineer.UpdateProfileService.Model;

namespace Engineer.UpdateProfileService.Repository.Contracts
{
    public interface IUpdateProfileContext
    {
        IMongoCollection<UserProfile> UserProfile { get; }
    }
}
