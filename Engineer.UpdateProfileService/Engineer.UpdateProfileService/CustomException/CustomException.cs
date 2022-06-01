using System;
namespace Engineer.UpdateProfileService.CustomException
{
    [Serializable]
    class UpdateProfileAfterValidDateException : Exception
    {
        public UpdateProfileAfterValidDateException()
            : base("Update of profile is allowed only after 10 days of adding profile or last change.")
        {

        }
    }

    [Serializable]
    class InvalidUserIdException : Exception
    {
        public InvalidUserIdException()
            : base("Invalid userid is provied.")
        {

        }
    }

    [Serializable]
    public class InvalidExpertiseLevelException : Exception
    {
        public InvalidExpertiseLevelException() { }

        public InvalidExpertiseLevelException(int expertiseLevel)
            : base(string.Format("Invalid Expertise Level {0}. Expertise level for each skill must range between 0-20.", expertiseLevel))
        {

        }
        public InvalidExpertiseLevelException(string expertiseLevel)
            : base(string.Format("Invalid Expertise Level {0}. Expertise level must not be non empty or a non-numeric value.", string.IsNullOrEmpty(expertiseLevel) ? "<NULL/EMPTY>" : expertiseLevel))
        {

        }
    }
}
