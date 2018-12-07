using System;

namespace Nute.Entities
{
    public struct Quantity : IComparable<Quantity>
    {
        public Quantity(decimal count, Unit unit)
        {
            Count = count;
            Unit = unit;

        }
        public decimal Count { get; private set;  }
        public Unit Unit { get; private set; }

        public int CompareTo(Quantity other)
        {
            return Count.CompareTo(other.Count);
        }

        public static bool operator==(Quantity left, Quantity right)
        {
            return left.Equals(right);
        }
        public static bool operator!=(Quantity left, Quantity right)
        {
            return !left.Equals(right);
        }
        public bool Equals(Quantity other)
        {
            return Count == other.Count && Equals(Unit, other.Unit);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Quantity other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Count.GetHashCode() * 397) ^ (Unit != null ? Unit.GetHashCode() : 0);
            }
        }

    }
}