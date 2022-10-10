using System.ComponentModel.DataAnnotations;

namespace HomeInv.Common.Entities
{
    public class AreaUserEntity : EntityBase
    {
        [Required]
        public int AreaId { get; set; }
        public AreaEntity Area { get; set; }

        [Required]
        public string UserId { get; set; }
        public string Username { get; set; }

        public string Role { get; set; }
    }
}
