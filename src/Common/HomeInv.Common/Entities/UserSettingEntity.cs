using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HomeInv.Common.Entities
{
    public class UserSettingEntity : EntityBase
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Aktif Ev")]
        public int DefaultHomeId { get; set; }
        [JsonIgnore]
        public HomeEntity DefaultHome { get; set; }
    }
}
