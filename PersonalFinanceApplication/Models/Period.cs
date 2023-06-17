using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalFinanceApplication.Models
{
    public class Period
    {
        [Key]
        public int PeriodId { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
    }
}