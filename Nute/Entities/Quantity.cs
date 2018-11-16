namespace Nute.Entities
{
    public struct Quantity
    {
        public Quantity(decimal count, Unit unit)
        {
            Count = count;
            Unit = unit;

        }
        public decimal Count { get; private set;  }
        public Unit Unit { get; private set; }
    }
}