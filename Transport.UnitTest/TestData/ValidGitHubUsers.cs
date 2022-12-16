using Newtonsoft.Json;
using Transport.Application.Contract.Dtos;
using Transport.Domain;

namespace Transport.UnitTest.TestData
{
    public class GitHub
    {
        private readonly List<GitHubUser> _validUsers;
        public IEnumerable<GitHubUser> ValidUsers => _validUsers;
        public string ValidUsersJsonString => JsonConvert.SerializeObject(_validUsers);
        public IEnumerable<User> ValidSystemUsers => _validUsers.Where(w => TryConvertToSystemUser(w, out _)).Select(w => ConvertToSystemUser(w)).ToList();

        public User ChrisWanstrath => ConvertToSystemUser(_validUsers.FirstOrDefault(w => w.Login == "defunkt"));

        public IEnumerable<string> InvalidUserNames => new List<string>
        {
            string.Empty,
            "more_than_39_characters_loooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooong_name",
            "123123123123123123123123123123123123",
            "------------------------------------",
            "----abvcd----",
            "adc--eee"
        };

        public GitHub()
        {
            _validUsers = new List<GitHubUser>
            {
                new GitHubUser
                {
                    Login = "defunkt",
                    Name = "Chris Wanstrath",
                    Company = null,
                    NumberOfFollowers = 21561,
                    NumberOfPublicRepositories = 107
                },
                new GitHubUser
                {
                    Login = "pjhyett",
                    Name = "PJ Hyett",
                    Company = "GitHub, Inc.",
                    NumberOfFollowers = null,
                    NumberOfPublicRepositories = 8
                },
                new GitHubUser
                {
                    Login = "mojombo",
                    Name = "Tom Preston-Werner",
                    Company = "@chatterbugapp, @redwoodjs, @preston-werner-ventures ",
                    NumberOfFollowers = 23283,
                    NumberOfPublicRepositories = 64
                },
                new GitHubUser
                {
                    Login = "topfunky",
                    Name = null,
                    Company = "@hashicorp ",
                    NumberOfFollowers = 0,
                    NumberOfPublicRepositories = 50
                },
                new GitHubUser
                {
                    Login = "wayneeseguin",
                    Name = null,
                    Company = null,
                    NumberOfFollowers = null,
                    NumberOfPublicRepositories = null
                }
            };
        }

        private bool TryConvertToSystemUser(GitHubUser gitHubUser, out User? user)
        {
            user = null;

            if (gitHubUser == null)
                return false;

            var loginNameResult = LoginName.CreateInstance(gitHubUser.Login);

            if (loginNameResult.IsFailure)
                return false;

            user = ConvertToSystemUser(gitHubUser);

            return true;
        }

        private static User ConvertToSystemUser(GitHubUser gitHubUser)
        {
            var loginNameResult = LoginName.CreateInstance(gitHubUser.Login);
            var nameResult = PersonName.CreateInstance(gitHubUser.Name);
            var companyResult = CompanyName.CreateInstance(gitHubUser.Company);
            var user = new User(loginNameResult.Value, nameResult.Value, companyResult.Value, gitHubUser.NumberOfFollowers, gitHubUser.NumberOfPublicRepositories);
            return user;
        }
    }
}
