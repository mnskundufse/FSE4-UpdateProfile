using Engineer.UpdateProfileService.Business.Contracts;
using Engineer.UpdateProfileService.Model;
using Engineer.UpdateProfileService.Repository.Contracts;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Engineer.UpdateProfileService.CustomException;

namespace Engineer.UpdateProfileService.Business.Implementation
{
    public class UpdateProfileBusiness : IUpdateProfileBusiness
    {
        public readonly IUpdateProfileRepository _repo;
        public UpdateProfileBusiness(IUpdateProfileRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse> UpdateUserProfileBusiness(int userId, UserExpertiseLevel userExpertiseLevel, DateTime updatedDateTime)
        {
            ValidateExpertiseLevel(userExpertiseLevel.TechnicalSkillExpertiseLevel);
            ValidateExpertiseLevel(userExpertiseLevel.NonTechnicalSkillExpertiseLevel);

            return await _repo.UpdateUserProfileRepository(userId, userExpertiseLevel, updatedDateTime);
        }

        /// <summary>
        /// Validate Expertise level
        /// </summary>
        /// <param name="myObject"></param>
        private void ValidateExpertiseLevel(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value) || !(int.TryParse(value, out int numericValue)))
                    {
                        throw new InvalidExpertiseLevelException(value);
                    }
                    else
                    {
                        if (numericValue < 0 || numericValue > 20)
                        {
                            throw new InvalidExpertiseLevelException(numericValue);
                        }
                    }
                }
            }
        }
    }
}