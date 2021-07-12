using System.ComponentModel.DataAnnotations;

namespace HomeInv.Common.Entities
{
    public class AreaEntity : EntityBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Alan ismi boş olamaz")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
