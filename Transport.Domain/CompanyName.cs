using FunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport.Domain
{
    public class CompanyName : ValueObject
    {
        public string? Name { get; }
        protected CompanyName() { }

        private CompanyName(string? name)
        {
            Name = name;
        }
        public static Result<CompanyName> CreateInstance(string? name)
        {
            //Apply validation agaist the string name to ensure it's a valid company name

            return Result.Ok(new CompanyName(name));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
