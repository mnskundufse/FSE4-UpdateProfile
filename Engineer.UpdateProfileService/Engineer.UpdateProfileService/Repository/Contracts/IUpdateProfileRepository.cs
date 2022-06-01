using Engineer.UpdateProfileService.Model;
using System;
using System.Threading.Tasks;

namespace Engineer.UpdateProfileService.Repository.Contracts
{
    public interface IUpdateProfileRepository
    {
        Task<ApiResponse> UpdateUserProfileRepository(int userId, UserExpertiseLevel userExpertiseLevel, DateTime updatedDateTime);
    }
}
