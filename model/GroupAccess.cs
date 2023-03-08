using System;
using Dapper.Contrib.Extensions;

namespace newplgapi.model
{
    [Table("plg_tblMstGroupAccess")]
    public class GroupAccess
    {
        [ExplicitKey]
        public string GroupAccessID { get; set; }
        public string GroupAccessName { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public short DeletedStatus  { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
    }
}