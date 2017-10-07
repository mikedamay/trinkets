using System;

namespace com.TheDisappointedProgrammer.Drive
{
    public class AccountTotals
    {
        public AccountTotals(int AccountingMonth, decimal[] Summary)
        {
            this.AccountingMonth = AccountingMonth;
            this.Summary = Summary;
        }
        public int AccountingMonth { get; }
        public decimal[] Summary { get; }
    }
}