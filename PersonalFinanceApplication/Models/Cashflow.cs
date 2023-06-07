using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalFinanceApplication.Models
{
    public class Cashflow
    {
        [Key] public int CashflowId { get; set; }

        // 1 period record corresponds to 1 balance & 1 cashflow record
        [ForeignKey("Period")] public int PeriodId { get; set; }
        public virtual Period Period { get; set; }

        // Amount of cash infow e.g. primary & miscellaneous income
        public decimal Inflow { get; set; }

        // Amount of cash outflow e.g. necessary & discretionary spending
        public decimal Outflow { get; set; }
    }

    public class CashflowDto
    {
        public int CashflowId { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
        public decimal Inflow { get; set; }
        public decimal Outflow { get; set; }
    }
}