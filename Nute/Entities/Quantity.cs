namespace Nute.Entities
{
    public struct Quantity
    {
        public Quantity(decimal count, Unit unit)
        {
            Count = count;
            Unit = unit;
            UnitId = 0;

        }
        public decimal Count { get; private set;  }
        public long UnitId { get; set; }
        public Unit Unit { get; private set; }
    }
}