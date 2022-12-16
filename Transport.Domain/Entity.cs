using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport.Domain
{
    public abstract class Entity<T>
    {
        public T Id { get; set; }

        protected Entity() { }

        protected Entity(T id) : this()
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Entity<T> other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetRealType() != other.GetRealType())
                return false;

            return Comparer<T>.Default.Compare(Id, other.Id) == 0;
        }

        public static bool operator ==(Entity<T>? a, Entity<T>? b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity<T>? a, Entity<T>? b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetRealType().ToString() + Id).GetHashCode();
        }

        private Type? GetRealType()
        {
            var type = GetType();

            //only valid when EF lazy-load enabled
            if (type?.ToString()?.Contains("Castle.Proxies.") == true)
                return type.BaseType;

            return type;
        }
    }
}
