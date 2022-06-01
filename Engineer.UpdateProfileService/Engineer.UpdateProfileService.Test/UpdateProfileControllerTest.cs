using System;
using Xunit;
using Moq;
using Engineer.UpdateProfileService.Business.Contracts;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Confluent.Kafka;
using Engineer.UpdateProfileService.Controllers;
using Engineer.UpdateProfileService.Model;
using Engineer.UpdateProfileService.Kafka;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Engineer.UpdateProfileService.Business.Implementation;
using Engineer.UpdateProfileService.Repository.Contracts;
using Engineer.UpdateProfileService;
using Engineer.UpdateProfileService.CustomException;

namespace Engineer.updateProfileServiceTest
{
    public class UpdateProfileControllerTest
    {
        readonly Mock<IUpdateProfileRepository> _mockUpdateProfileRepo = new Mock<IUpdateProfileRepository>();
        [Fact]
        public async Task UpdateUserProfile_ValidRequest()
        {
            int userid = 1;
            UserProfile request = new UserProfile
            {
                AssociateId = "CTS03",
                Name = "manas",
                Mobile = "3231453213",
                Email = "manas@gmail.com"
            };
            UserExpertiseLevel requestUserExpertiseLevel = new UserExpertiseLevel
            {
                TechnicalSkillExpertiseLevel = new TechnicalSkillExpertiseLevel
                {
                    AzureExpertiseLevel = "0",
                    DockerExpertiseLevel = "1",
                    EntityFrameworkExpertiseLevel = "19",
                    GitExpertiseLevel = "5",
                    HTMLCSSJavaScriptExpertiseLevel = "2",
                    JenkinsExpertiseLevel = "6",
                    ReactExpertiseLevel = "3",
                    RestfulExpertiseLevel = "8",
                    AngularExpertiseLevel = "3",
                    AspNetCoreExpertiseLevel = "10"
                },
                NonTechnicalSkillExpertiseLevel = new NonTechnicalSkillExpertiseLevel
                {
                    AptitudeExpertiseLevel = "10",
                    CommunicationExpertiseLevel = "3",
                    SpokenExpertiseLevel = "1"
                }
            };

            ApiResponse response = new ApiResponse()
            {
                Result = "User Profile updated successfully.",
                Status = new StatusResponse
                {
                    IsValid = true,
                    Status = "SUCCESS",
                    Message = string.Empty
                }
            };

            UpdateProfileBusiness _testObject = new UpdateProfileBusiness(_mockUpdateProfileRepo.Object);
            _mockUpdateProfileRepo.Setup(x => x.UpdateUserProfileRepository(It.IsAny<int>(), It.IsAny<UserExpertiseLevel>(), It.IsAny<DateTime>())).Returns(Task.FromResult(response));

            var result = await _testObject.UpdateUserProfileBusiness(userid, requestUserExpertiseLevel, DateTime.Now);

            Assert.Equal("User Profile updated successfully.", result.Result);
        }
        [Fact]
        public async Task UpdateUserProfile_nullExpertiseRequest()
        {
            int userid = 1;
            UserProfile request = new UserProfile
            {
                AssociateId = "CTS03",
                Name = "manas",
                Mobile = "3231453213",
                Email = "manas@gmail.com"
            };
            UserExpertiseLevel requestUserExpertiseLevel = new UserExpertiseLevel
            {
                TechnicalSkillExpertiseLevel = new TechnicalSkillExpertiseLevel
                {

                },
                NonTechnicalSkillExpertiseLevel = new NonTechnicalSkillExpertiseLevel
                {
                    AptitudeExpertiseLevel = "10",
                    CommunicationExpertiseLevel = "3",
                    SpokenExpertiseLevel = "1"
                }
            };

            ApiResponse response = new ApiResponse()
            {
                Result = "User Profile updated successfully.",
                Status = new StatusResponse
                {
                    IsValid = true,
                    Status = "SUCCESS",
                    Message = string.Empty
                }
            };

            UpdateProfileBusiness _testObject = new UpdateProfileBusiness(_mockUpdateProfileRepo.Object);
            _mockUpdateProfileRepo.Setup(x => x.UpdateUserProfileRepository(It.IsAny<int>(), It.IsAny<UserExpertiseLevel>(), It.IsAny<DateTime>())).Returns(Task.FromResult(response));

            var ex = await Assert.ThrowsAsync<InvalidExpertiseLevelException>(async () => await _testObject.UpdateUserProfileBusiness(userid, requestUserExpertiseLevel, DateTime.Now));
            Assert.Equal("Invalid Expertise Level <NULL/EMPTY>. Expertise level must not be non empty or a non-numeric value.", ex.Message);

        }
        [Fact]
        public async Task UpdateUserProfile_wrongNumericRequest()
        {
            int userid = 1;
            UserProfile request = new UserProfile
            {
                AssociateId = "CTS03",
                Name = "manas",
                Mobile = "3231453213",
                Email = "manas@gmail.com"
            };
            UserExpertiseLevel requestUserExpertiseLevel = new UserExpertiseLevel
            {
                TechnicalSkillExpertiseLevel = new TechnicalSkillExpertiseLevel
                {
                    AzureExpertiseLevel = "0",
                    DockerExpertiseLevel = "1",
                    EntityFrameworkExpertiseLevel = "25",
                    GitExpertiseLevel = "5",
                    HTMLCSSJavaScriptExpertiseLevel = "2",
                    JenkinsExpertiseLevel = "6",
                    ReactExpertiseLevel = "3",
                    RestfulExpertiseLevel = "8",
                    AngularExpertiseLevel = "3",
                    AspNetCoreExpertiseLevel = "10"
                },
                NonTechnicalSkillExpertiseLevel = new NonTechnicalSkillExpertiseLevel
                {
                    AptitudeExpertiseLevel = "10",
                    CommunicationExpertiseLevel = "3",
                    SpokenExpertiseLevel = "1"
                }
            };

            ApiResponse response = new ApiResponse()
            {
                Result = "User Profile updated successfully.",
                Status = new StatusResponse
                {
                    IsValid = true,
                    Status = "SUCCESS",
                    Message = string.Empty
                }
            };

            UpdateProfileBusiness _testObject = new UpdateProfileBusiness(_mockUpdateProfileRepo.Object);
            _mockUpdateProfileRepo.Setup(x => x.UpdateUserProfileRepository(It.IsAny<int>(), It.IsAny<UserExpertiseLevel>(), It.IsAny<DateTime>())).Returns(Task.FromResult(response));

            var ex = await Assert.ThrowsAsync<InvalidExpertiseLevelException>(async () => await _testObject.UpdateUserProfileBusiness(userid, requestUserExpertiseLevel, DateTime.Now));
            Assert.Equal("Invalid Expertise Level 25. Expertise level for each skill must range between 0-20.", ex.Message);
        }


    }
}
