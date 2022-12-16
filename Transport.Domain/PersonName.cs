using FunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport.Domain
{
    public class PersonName : ValueObject
    {
        public string? Name { get; }
        protected PersonName() { }

        private PersonName(string? name)
        {
            Name = name;
        }
        public static Result<PersonName> CreateInstance(string? name)
        {
            //Apply validation agaist the string name to ensure it's a valid person name

            return Result.Ok(new PersonName(name));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
