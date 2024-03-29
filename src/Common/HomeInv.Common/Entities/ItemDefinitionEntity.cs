﻿using System.ComponentModel.DataAnnotations;

namespace HomeInv.Common.Entities
{
    public class ItemDefinitionEntity : EntityBase
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryFullName { get; set; }

        public bool IsExpirable { get; set; }

        public int SizeUnitId { get; set; }
        public string SizeUnitName { get; set; }

        public bool IsActive { get; set; }
    }
}
