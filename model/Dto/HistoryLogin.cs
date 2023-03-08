using System;
using Dapper.Contrib.Extensions;

namespace newplgapi.model.Dto
{
    [Table("plg_TblUserLoginHistory")]
    public class HistoryLogin
    {
        [Key]
        public int Id { get; set; }

        public string UserID { get; set; }

        public DateTime Tanggal { get; set; }
    }
}