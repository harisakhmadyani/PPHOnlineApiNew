using Dapper.Contrib.Extensions;

namespace newplgapi.model
{
    [Table("plg_tblMstItemSupplier")]
    public class MyItemInput
    {
        [ExplicitKey]
        public string ItemID { get; set; }
        public string SupplierID { get; set; }
        public string NewItemID { get; set; }
    }
}