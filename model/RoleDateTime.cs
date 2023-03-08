using System;
using Dapper.Contrib.Extensions;

namespace newplgapi.model
{
   [Table("plg_tblMstRoleDateTime")]
    public class RoleDateTime
    {
        [Key]
        public long ID { get; set; }
        public Nullable<DateTime> OpenDate { get; set; }
        public Nullable<DateTime> CloseDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }        
    }
}