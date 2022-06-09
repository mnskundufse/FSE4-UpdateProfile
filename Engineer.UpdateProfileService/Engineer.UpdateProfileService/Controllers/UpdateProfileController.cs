using Engineer.UpdateProfileService.Business.Contracts;
using Engineer.UpdateProfileService.Kafka;
using Engineer.UpdateProfileService.Model;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Engineer.UpdateProfileService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("skill-tracker/api/v{version:apiVersion}/engineer")]
    [Produces("application/json")]
    public class UpdateProfileController : ControllerBase
    {
        private readonly ILogger<UpdateProfileController> _logger;
        private readonly ProducerConfig _config;
        private readonly IUpdateProfileBusiness _updateProfileBC;
        private readonly IProducerWrapper _producerWrapper;

        public UpdateProfileController(ILogger<UpdateProfileController> logger, ProducerConfig config, IUpdateProfileBusiness updateProfileBC, IProducerWrapper producerWrapper)
        {
            _updateProfileBC = updateProfileBC;
            _producerWrapper = producerWrapper;
            _logger = logger;
            _config = config;
        }

        /// <summary>
        /// Update User Profile (Controller) 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userExpertiseLevel"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [Authorize]
        [HttpPost("update-profile/{userId}")]
        public async Task<IActionResult> UpdateUserProfile(int userId, UserExpertiseLevel userExpertiseLevel)
        {   
            try
            {
                ApiResponse response = new ApiResponse();
                DateTime updatedDateTime = DateTime.Now;
                response = await _updateProfileBC.UpdateUserProfileBusiness(userId, userExpertiseLevel, updatedDateTime);
                if (response.Status.IsValid)
                {
                    UserProfile userProfile = new UserProfile
                    {
                        UserId = userId,
                        TechnicalSkillExpertiseLevel = userExpertiseLevel.TechnicalSkillExpertiseLevel,
                        NonTechnicalSkillExpertiseLevel = userExpertiseLevel.NonTechnicalSkillExpertiseLevel,
                        UpdatedDate = updatedDateTime
                    };
                    PublishEvent(userProfile);
                    _logger.LogInformation("{date} : UpdateUserProfile of the UpdateProfileController executed.", DateTime.UtcNow);
                    return StatusCode(200, response);
                }
                else
                {
                    _logger.LogInformation("{date} : UpdateUserProfile of the UpdateProfileController Failed : Message {message} ", DateTime.UtcNow, response.Status.Message);
                    return StatusCode(405, response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unknown error occurred on the UpdateUserProfile of the UpdateProfileController");
                throw;
            }
        }

        /// <summary>
        /// Publish User skill expertise level Update event to profileforadminupdatetopic (KAFKA Code)
        /// </summary>
        /// <param name="userProfileForAdmin"></param>
        private async void PublishEvent(UserProfile userProfileForAdmin)
        {
            string serializedUserProfileForAdmin = JsonConvert.SerializeObject(userProfileForAdmin);
            await _producerWrapper.WriteMessage(serializedUserProfileForAdmin, "profileforadminupdatetopic");

            _logger.LogInformation("{date} : PublishEvent of the UpdateProfileController executed.", DateTime.UtcNow);
        }
    }
}