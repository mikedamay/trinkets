using System;
using com.TheDisappointedProgrammer.IOCC;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    public class ColumnMap
    {
        public class ColumnSpec
        {
            public ColumnSpec(int index, Type type, Func<string, object> parser, string heading)
            {
                Index = index;
                Type = type;
                Parser = parser;
                Heading = heading;
            }
            public readonly int Index;
            public readonly Type Type;
            public readonly Func<string, object> Parser;
            public readonly string Heading;
        }
        private readonly ColumnSpec[] map =
        {
            new ColumnSpec( 1, typeof(DateTime), s => DateTime.Parse(s), "Date")
            ,new ColumnSpec( 2, typeof(object), s => s, "Description")
            ,new ColumnSpec( 3, typeof(object), s => s, "Category")
            ,new ColumnSpec( 4, typeof(object), s => s, "Receipts")
            ,new ColumnSpec( 5, typeof(object), s => s, "Payments")
            ,new ColumnSpec( 6, typeof(decimal?), s => ParseDecimal(s), "Cash")
            ,new ColumnSpec( 7, typeof(decimal?), s => ParseDecimal(s), "Groceries")
            ,new ColumnSpec( 8, typeof(decimal?), s => ParseDecimal(s), "Flat")
            ,new ColumnSpec( 9, typeof(decimal?), s => ParseDecimal(s), "Entertainment 1")
            ,new ColumnSpec( 10, typeof(decimal?), s => ParseDecimal(s), "Phones & Computers")
            ,new ColumnSpec( 11, typeof(decimal?), s => ParseDecimal(s), "Recreation")
            ,new ColumnSpec( 12, typeof(decimal?), s => ParseDecimal(s), "Sundry")
            ,new ColumnSpec( 13, typeof(decimal?), s => ParseDecimal(s), "Investments")
            ,new ColumnSpec( 14, typeof(decimal?), s => ParseDecimal(s), "Recreation 2")
        };

        public ColumnSpec[] Columns => map;
        public int FirstExpenseColumn => 5;
        public int NumExpenseColumns => map.Length - FirstExpenseColumn;

        private static decimal? ParseDecimal(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? (decimal?)null : Decimal.Parse(s);
        }
    }
}