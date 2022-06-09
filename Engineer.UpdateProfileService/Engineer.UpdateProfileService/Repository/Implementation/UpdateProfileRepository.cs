using System;
using System.Threading.Tasks;
using Engineer.UpdateProfileService.CustomException;
using Engineer.UpdateProfileService.Model;
using Engineer.UpdateProfileService.Repository.Contracts;
using MongoDB.Driver;

namespace Engineer.UpdateProfileService.Repository.Implementation
{
    public class UpdateProfileRepository : IUpdateProfileRepository
    {
        private readonly IUpdateProfileContext _context;
        public UpdateProfileRepository(IUpdateProfileContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Update User Profile (Repository)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userExpertiseLevel"></param>
        /// <param name="updatedDateTime"></param>
        /// <returns></returns>
        public async Task<ApiResponse> UpdateUserProfileRepository(int userId, UserExpertiseLevel userExpertiseLevel, DateTime updatedDateTime)
        {
            ApiResponse response = new ApiResponse();

            var userItem = await _context
                            .UserProfile
                            .Find(
                                filter: f => f.UserId == userId
                            ).FirstOrDefaultAsync();

            if (userItem != null)
            {
                if ((DateTime.Now - userItem.UpdatedDate).Days <= 10)
                {
                    //Update of Profile must be allowed only after 10 days of adding profile or last change, else throw a custom exception
                    throw new UpdateProfileAfterValidDateException();
                }
                else
                {
                    userItem.TechnicalSkillExpertiseLevel = userExpertiseLevel.TechnicalSkillExpertiseLevel;
                    userItem.NonTechnicalSkillExpertiseLevel = userExpertiseLevel.NonTechnicalSkillExpertiseLevel;
                    userItem.UpdatedDate = updatedDateTime;

                    ReplaceOneResult updatedResult = await _context.UserProfile.ReplaceOneAsync(
                        filter: f => f.UserId == userId,
                        replacement: userItem
                        );

                    if (updatedResult != null && updatedResult.IsAcknowledged && updatedResult.ModifiedCount > 0)
                    {
                        response.Result = "User Profile updated successfully.";
                    }
                    else
                    {
                        response.Status.Message = "Problem occured during updating user profile.";
                    }
                }
            }
            else
            {
                //If invalid UserId is provided, it must throw a custom exception
                throw new InvalidUserIdException();
            }

            if(!string.IsNullOrEmpty(response.Status.Message))
            {
                response.Status.IsValid = false;
                response.Status.Status = "FAIL";
            }
            return response;
        }
    }
}