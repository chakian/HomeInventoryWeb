using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class BaseDbo
    {
        [Key]
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }
}
