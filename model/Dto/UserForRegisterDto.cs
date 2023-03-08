using System.ComponentModel.DataAnnotations;

namespace newplgapi.model.Dto
{
    public class UserForRegisterDto
    {
         public string userId { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        [Required]
        [StringLength(12, ErrorMessage = "You must specify password between 6 and 12 characters", MinimumLength = 6)]
        public string password { get; set; }

        public int roles { get; set; }

        public string CreatedBy { get; set; }

        public string Factory { get; set; }

        public string Dept { get; set; }

        public string GroupAcces { get; set; }

        public string NoHp { get; set; }
    }
}