using System.ComponentModel.DataAnnotations;

namespace HomeInv.Common.Entities
{
    public class HomeEntity : EntityBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ev ismi boş olamaz")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
