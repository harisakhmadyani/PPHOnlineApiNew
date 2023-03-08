namespace newplgapi.model
{
    public class PriceGet : PriceInput
    {
        public string SupplierName { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string UOMName { get; set; }
    }
}