using System;

namespace Engineer.UpdateProfileService.Model
{
    public class UserExpertiseLevel
    {
        public TechnicalSkillExpertiseLevel TechnicalSkillExpertiseLevel { get; set; }
        public NonTechnicalSkillExpertiseLevel NonTechnicalSkillExpertiseLevel { get; set; }
    }

    public class TechnicalSkillExpertiseLevel
    {
        public string HTMLCSSJavaScriptExpertiseLevel { get; set; }
        public string AngularExpertiseLevel { get; set; }
        public string ReactExpertiseLevel { get; set; }
        public string AspNetCoreExpertiseLevel { get; set; }
        public string RestfulExpertiseLevel { get; set; }
        public string EntityFrameworkExpertiseLevel { get; set; }
        public string GitExpertiseLevel { get; set; }
        public string DockerExpertiseLevel { get; set; }
        public string JenkinsExpertiseLevel { get; set; }
        public string AzureExpertiseLevel { get; set; }
    }

    public class NonTechnicalSkillExpertiseLevel
    {
        public string SpokenExpertiseLevel { get; set; }
        public string CommunicationExpertiseLevel { get; set; }
        public string AptitudeExpertiseLevel { get; set; }
    }
}
