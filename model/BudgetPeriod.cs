using System;
using Dapper.Contrib.Extensions;

namespace newplgapi.model
{
    [Table("plg_tblMstBudgetUsing")]
    public class BudgetPeriod
    {
        [Key]
        public long ID { get; set; }
        public string BudgetYear { get; set; }
        public string BudgetMonth { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
    }
}