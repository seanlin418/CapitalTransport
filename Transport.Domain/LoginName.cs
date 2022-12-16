using FunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Transport.Domain
{
    //Immutable Username class
    public class LoginName : ValueObject
    {
        public string? Name { get; }

        protected LoginName() { }

        private LoginName(string name)
        {
            Name = name;
        }

        public static Result<LoginName> CreateInstance(string? name)
        {
            //GitHub username constraints:
            //Only contain alphanumeric characters or hyphens
            //Should not have multiple consecutive hyphens
            //Should not begin or end with a hyphen
            //Maximum is 39 characters

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(name))
                return Result.Fail<LoginName>("Name should not be empty");

            if (name.Length > 39)
                return Result.Fail<LoginName>($"Name is too long: {name}");

            if(Regex.IsMatch(name, @"^[0-9 -]+$"))
                return Result.Fail<LoginName>($"Name should not only contain alphanumeric characters or hyphens: {name}");

            if (Regex.IsMatch(name, @"^-.*-$"))
                return Result.Fail<LoginName>($"Name should not begin or end with a hyphen: {name}");

            if (Regex.IsMatch(name, @"[-]{2,}"))
                return Result.Fail<LoginName>($"Name should not have multiple consecutive hyphens: {name}");

            return Result.Ok(new LoginName(name));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
