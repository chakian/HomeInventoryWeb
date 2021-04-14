using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class BaseDbo
    {
        [Key]
        public string Id { get; set; }
        public bool IsActive { get; set; }
    }
}
