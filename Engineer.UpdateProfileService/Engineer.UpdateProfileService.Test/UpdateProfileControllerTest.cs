﻿using System;
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

namespace Engineer.updateProfileServiceTest
{
    public class UpdateProfileControllerTest
    {
        readonly Mock<IUpdateProfileBusiness> _mockProfileBusiness = new Mock<IUpdateProfileBusiness>();
        readonly Mock<IProducerWrapper> _mockProducerWrapper = new Mock<IProducerWrapper>();
        readonly Mock<ILogger<UpdateProfileController>> _mockLogger = new Mock<ILogger<UpdateProfileController>>();
        readonly Mock<ProducerConfig> _mockProducerConfig = new Mock<ProducerConfig>();

        [Fact]
        public async Task UpdateUserProfile_ValidRequest()
        {
            //UserProfile request = new UserProfile
            //{
            //    AssociateId = "1",
            //    Name = "manas",
            //    Mobile = "32314532132"
            //};
            int userid = 1;
            UserExpertiseLevel request = new UserExpertiseLevel
            {
                NonTechnicalSkillExpertiseLevel = new NonTechnicalSkillExpertiseLevel
                {
                    AptitudeExpertiseLevel = "10"
                },
                TechnicalSkillExpertiseLevel = new TechnicalSkillExpertiseLevel
                {
                    AngularExpertiseLevel = "3"
                }
            };

            ApiResponse response = new ApiResponse()
            {
                Result = 2,
                Status = new StatusResponse
                {
                    IsValid = true,
                    Status = "SUCCESS",
                    Message = string.Empty
                }
            };

            UpdateProfileController _testObject = new UpdateProfileController(_mockLogger.Object, _mockProducerConfig.Object, _mockProfileBusiness.Object, _mockProducerWrapper.Object);
            ProducerWrapper _producerTestObject = new ProducerWrapper(_mockProducerConfig.Object);
            Mock<IProducer<string, string>> _mockProducerBuilder = new Mock<IProducer<string, string>>();
            _mockProducerWrapper.Setup(x => x.WriteMessage(It.IsAny<string>(), It.IsAny<string>()));
            _mockProfileBusiness.Setup(x => x.UpdateUserProfileBusiness(It.IsAny<int>(), It.IsAny<UserExpertiseLevel>(), It.IsAny<DateTime>())).Returns(Task.FromResult(response));
            var result = (ObjectResult)await _testObject.UpdateUserProfile(userid, request);

            ApiResponse apiResponse = (ApiResponse)result.Value;

            Assert.NotNull(apiResponse.Result);
            Assert.Equal(2, (int)apiResponse.Result);
            Assert.NotNull(apiResponse.Status);
            Assert.True(apiResponse.Status.IsValid);
            Assert.Equal("SUCCESS", apiResponse.Status.Status);
            Assert.Empty(apiResponse.Status.Message);
        }

        [Fact]
        public async Task UpdateUserProfile_InvalidRequest()
        {
            int userid = 1;
            UserExpertiseLevel request = new UserExpertiseLevel
            {
                NonTechnicalSkillExpertiseLevel = new NonTechnicalSkillExpertiseLevel
                {
                    AptitudeExpertiseLevel = "10"
                },
                TechnicalSkillExpertiseLevel = new TechnicalSkillExpertiseLevel
                {
                    AngularExpertiseLevel = "3"
                }
            };

            ApiResponse response = new ApiResponse()
            {
                Result = 0,
                Status = new StatusResponse
                {
                    IsValid = false,
                    Status = "FAIL",
                    Message = string.Empty
                }
            };

            UpdateProfileController _testObject = new UpdateProfileController(_mockLogger.Object, _mockProducerConfig.Object, _mockProfileBusiness.Object, _mockProducerWrapper.Object);
            ProducerWrapper _producerTestObject = new ProducerWrapper(_mockProducerConfig.Object);
            Mock<IProducer<string, string>> _mockProducerBuilder = new Mock<IProducer<string, string>>();
            _mockProducerWrapper.Setup(x => x.WriteMessage(It.IsAny<string>(), It.IsAny<string>()));
            _mockProfileBusiness.Setup(x => x.UpdateUserProfileBusiness(It.IsAny<int>(), It.IsAny<UserExpertiseLevel>(), It.IsAny<DateTime>())).Returns(Task.FromResult(response));
            var result = (ObjectResult)await _testObject.UpdateUserProfile(userid, request);

            ApiResponse apiResponse = (ApiResponse)result.Value;

            Assert.NotNull(apiResponse.Result);
            Assert.Equal(0, (int)apiResponse.Result);
            Assert.NotNull(apiResponse.Status);
            Assert.False(apiResponse.Status.IsValid);
            Assert.Equal("FAIL", apiResponse.Status.Status);

        }
    }
}
