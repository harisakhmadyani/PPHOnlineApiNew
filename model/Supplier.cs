namespace newplgapi.model
{
    public class Supplier
    {
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierShortName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public int? COAID { get; set; }
        public string COANo { get; set; }
        public string COAAccRecNo { get; set; }
        public string GroupWil { get; set; }
        public string GroupDept { get; set; }
        public string SupplierNameChinese { get; set; }
        public string CountryID { get; set; }
        public bool Import { get; set; }
        public string CurrencyID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address1Chinese { get; set; }
        public string Address2Chinese { get; set; }
        public string Address3Chinese { get; set; }
        public string Address4Chinese { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string ContactPerson1 { get; set; }
        public string ContactPerson2 { get; set; }
        public bool? Financial { get; set; }
        public bool? Legality { get; set; }
        public bool? Quality { get; set; }
        public bool? Security { get; set; }
        public bool? EnvHealthSafety { get; set; }
        public bool? FoodSafety { get; set; }
        public bool? OrganicAllergent { get; set; }
        public bool? HalalKosher { get; set; }
        public bool? ViaKSP { get; set; }
        public string Remark { get; set; }
        public string SupplierIDOld2 { get; set; }
        public string SupplierIDOld { get; set; }
        public bool Modified { get; set; }
        public bool Register { get; set; }
        public string SupplierType { get; set; }

        //
        public string CountryName { get; set; }     
    }
}