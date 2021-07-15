using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeInv.Common.Entities
{
    public class CategoryEntity : EntityBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Alan ismi boş olamaz")]
        public string Name { get; set; }
        public string Description { get; set; }

        public bool HasParent { get; set; }

        public int? ParentCategoryId { get; set; }

        public bool HasChild { get; set; }

        public List<CategoryEntity> Children { get; set; }
    }
}
