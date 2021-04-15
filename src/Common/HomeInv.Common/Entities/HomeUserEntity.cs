using System.ComponentModel.DataAnnotations;

namespace HomeInv.Common.Entities
{
    public class HomeUserEntity : EntityBase
    {
        [Required]
        public int HomeId { get; set; }
        public HomeEntity Home { get; set; }

        [Required]
        public string UserId { get; set; }
        public string Username { get; set; }

        public string Role { get; set; }
    }
}
