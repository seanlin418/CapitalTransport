namespace Transport.Domain
{
    //Use Entity<T> if the user is an entity
    public class User : ValueObject  //Entity<int>
    {
        public LoginName Login { get; }
        public PersonName Name { get; }
        public CompanyName Company { get; }
        public int? NumberOfFollowers { get; }
        public int? NumberOfPublicRepositories { get; }

        public User(LoginName login, PersonName name, CompanyName company, int? numberOfFollowers, int? numberOfPublicRepositories)
        {
            Name = name;
            Login = login;
            Company = company;
            NumberOfFollowers = numberOfFollowers;
            NumberOfPublicRepositories = numberOfPublicRepositories;
        }

        //User is an immutable, compariable value object, similar for using entity
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Login;
            yield return Name;
            yield return Company;
            yield return NumberOfFollowers;
            yield return NumberOfPublicRepositories;
        }
    }
}