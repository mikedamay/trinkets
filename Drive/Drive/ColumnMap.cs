using System;
using com.TheDisappointedProgrammer.IOCC;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    public class ColumnMap
    {
        public class ColumnSpec
        {
            public ColumnSpec(int index, Type type, Func<string, object> parser)
            {
                Index = index;
                Type = type;
                Parser = parser;
            }
            public readonly int Index;
            public readonly Type Type;
            public readonly Func<string, object> Parser;
        }
        private readonly ColumnSpec[] map =
        {
            new ColumnSpec( 1, typeof(DateTime), s => DateTime.Parse(s))
            ,new ColumnSpec( 2, typeof(object), s => s)
            ,new ColumnSpec( 3, typeof(object), s => s)
            ,new ColumnSpec( 4, typeof(object), s => s)
            ,new ColumnSpec( 5, typeof(object), s => s)
            ,new ColumnSpec( 6, typeof(decimal?), s => ParseDecimal(s))
            ,new ColumnSpec( 7, typeof(decimal?), s => ParseDecimal(s))
            ,new ColumnSpec( 8, typeof(decimal?), s => ParseDecimal(s))
            ,new ColumnSpec( 9, typeof(decimal?), s => ParseDecimal(s))
            ,new ColumnSpec( 10, typeof(decimal?), s => ParseDecimal(s))
            ,new ColumnSpec( 11, typeof(decimal?), s => ParseDecimal(s))
            ,new ColumnSpec( 12, typeof(decimal?), s => ParseDecimal(s))
            ,new ColumnSpec( 13, typeof(decimal?), s => ParseDecimal(s))
            ,new ColumnSpec( 14, typeof(decimal?), s => ParseDecimal(s))
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