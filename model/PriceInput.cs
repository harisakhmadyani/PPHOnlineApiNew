using System;
using Dapper.Contrib.Extensions;

namespace newplgapi.model
{
    [Table("plg_tblTrnInputHarga")]
    public class PriceInput
    { 
        [Key]
        public long Id { get; set; }
        public string SupplierId { get; set; }
        public string ItemId { get; set; }
        public decimal Qnty { get; set; }
        public decimal QtyRt { get; set; }
        public decimal Harga { get; set; }
        public string Currency { get; set; }
        public DateTime ValidUntil { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Periode { get; set; }
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; } 
        public string NewItemID { get; set; }
        public string Factory { get; set; }
        public string FileName { get; set; }
        public string ChangeName { get; set; }        
    }
}