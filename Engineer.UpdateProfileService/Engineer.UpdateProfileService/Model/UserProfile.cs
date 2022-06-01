using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Engineer.UpdateProfileService.Model
{
    public class UserProfile
    {
        [BsonId]
        public long UserId { get; set; }

        public string Name { get; set; }
        public string AssociateId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public TechnicalSkillExpertiseLevel TechnicalSkillExpertiseLevel { get; set; }
        public NonTechnicalSkillExpertiseLevel NonTechnicalSkillExpertiseLevel { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
