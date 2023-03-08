using Dapper.Contrib.Extensions;

namespace newplgapi.model
{
    [Table("plg_tblMstUser")]
    public class User
    {
        [ExplicitKey]
        public string userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
        public bool notActive { get; set; }
        public int roles { get; set; }
        public string SecretKey { get; set; }
        public int Factory { get; set; }
        public bool HaveGA { get; set; }
        public string FacAbbr { get; set; }
        public string Dept { get; set; }
        public string GroupAccess { get; set; }
        public string nohp { get; set; }
        public string TelegramID { get; set; }
        public string SupplierID { get; set; }
    }
}