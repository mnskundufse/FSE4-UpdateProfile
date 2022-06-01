using Engineer.UpdateProfileService.Model;
using System;
using System.Threading.Tasks;

namespace Engineer.UpdateProfileService.Business.Contracts
{
    public interface IUpdateProfileBusiness
    {
        Task<ApiResponse> UpdateUserProfileBusiness(int userId, UserExpertiseLevel userExpertiseLevel, DateTime updatedDateTime);
    }
}
