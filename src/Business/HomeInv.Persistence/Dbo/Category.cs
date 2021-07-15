using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class Category : BaseDbo
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual IEnumerable<Category> ChildCategories { get; set; }

        public virtual IEnumerable<Item> Items { get; set; }

        [Required]
        public int HomeId { get; set; }
        public virtual Home Home { get; set; }
    }
}
