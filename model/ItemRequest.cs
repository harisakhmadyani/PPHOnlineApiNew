using System;

namespace newplgapi.model
{
    public class ItemRequest
    {
        public string ItemID { get; set; }
        public int SubCategoryID { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string UOMName { get; set; }
        public decimal Qnty { get; set; }
        public decimal QtyRt { get; set; }
        public decimal Harga { get; set; }
        public string Currency { get; set; }
        public Nullable<DateTime> ValidUntil { get; set; }
        public Nullable<DateTime> DeliveryDate { get; set; }
        public long IDInput { get; set; }
        public string Periode { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string Remark { get; set; }
        public string ImportBy { get; set; }
        public string NewItemID { get; set; }
        public string Factory { get; set; }
        public string FileName { get; set; }   
        
    }
}