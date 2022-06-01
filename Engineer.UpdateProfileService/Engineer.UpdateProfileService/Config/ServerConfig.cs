using Engineer.UpdateProfileService.Config;

namespace Engineer.UpdateProfileService.Models
{
    public class ServerConfig
    {
        public MongoDbConfig MongoDB { get; set; } = new MongoDbConfig();
    }
}