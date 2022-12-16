using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using Transport.Application.Services;
using Transport.Infrastructure;
using Transport.UnitTest.TestData;

namespace Transport.UnitTest
{
    public class GitHubUserServiceUnderTest
    {
        [Fact]
        public async Task Should_Retrieve_Five_Valid_Users_Sorted_Alphabetically_By_Name()
        {
            //Arrange
            var baseAddress = "https://api.github.com";
            var gitHub = new GitHub();
            var userNames = new List<string> { "topfunky", "mojombo", "defunkt", "nonexistent-user-1", "wayneeseguin", "nonexistent-user-2", "pjhyett", "nonexistent-user-3" };
            var mockHttp = new MockHttpMessageHandler();
            foreach(var userName in userNames)
            {
                var jUser = JsonConvert.SerializeObject(gitHub.ValidUsers.FirstOrDefault(w => w.Login == userName));
                mockHttp.When($"{baseAddress}/users/{userName}").Respond("application/json", jUser);
            }
            var httpClient = new HttpClient(mockHttp) { BaseAddress = new Uri(baseAddress) };
            var repo = new GitHubUserRepository(httpClient, new Mock<ILogger<GitHubUserRepository>>().Object);
            var sut = new GitHubUserService(repo, new Mock<ILogger<GitHubUserService>>().Object);

            //Act
            var users = await sut.RetrieveUsersAsync(userNames);

            //Assert
            CollectionAssert.AreEqual(gitHub.ValidSystemUsers, users);
        }

        [Fact]
        public async Task Should_Ignore_Duplicate_Users()
        {
            //Arrange
            var baseAddress = "https://api.github.com";
            var gitHub = new GitHub();
            var duplicateUserNames = new List<string> { "defunkt", "defunkt", "defunkt" };
            var mockHttp = new MockHttpMessageHandler();
            foreach (var userName in duplicateUserNames.Distinct())
            {
                var jUser = JsonConvert.SerializeObject(gitHub.ValidUsers.FirstOrDefault(w => w.Login == userName));
                mockHttp.When($"{baseAddress}/users/{userName}").Respond("application/json", jUser);
            }
            var httpClient = new HttpClient(mockHttp) { BaseAddress = new Uri(baseAddress) };
            var repo = new GitHubUserRepository(httpClient, new Mock<ILogger<GitHubUserRepository>>().Object);
            var sut = new GitHubUserService(repo, new Mock<ILogger<GitHubUserService>>().Object);

            //Act
            var users = await sut.RetrieveUsersAsync(duplicateUserNames);

            //Assert
            Xunit.Assert.True(users?.Count() == 1);
            Xunit.Assert.True(gitHub.ChrisWanstrath.Equals(users?.FirstOrDefault()));
        }

        [Fact]
        public async Task Should_Ignore_Invalid_GitHub_UserNames()
        {
            //Arrange
            var baseAddress = "https://api.github.com";
            var gitHub = new GitHub();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"{baseAddress}/users/*").Respond("application/json", string.Empty);
            var httpClient = new HttpClient(mockHttp) { BaseAddress = new Uri(baseAddress) };
            var repo = new GitHubUserRepository(httpClient, new Mock<ILogger<GitHubUserRepository>>().Object);
            var sut = new GitHubUserService(repo, new Mock<ILogger<GitHubUserService>>().Object);

            //Act
            var users = await sut.RetrieveUsersAsync(gitHub.InvalidUserNames);

            //Assert
            Xunit.Assert.Empty(users);
        }
    }
}