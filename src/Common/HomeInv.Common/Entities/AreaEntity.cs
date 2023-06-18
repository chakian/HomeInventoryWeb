using System.ComponentModel.DataAnnotations;

namespace HomeInv.Common.Entities
{
    public class AreaEntity : EntityBase
    {
        public int HomeId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Alan ismi boş olamaz")]
        public string Name { get; set; }
        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            return Id == ((AreaEntity)obj)?.Id;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
