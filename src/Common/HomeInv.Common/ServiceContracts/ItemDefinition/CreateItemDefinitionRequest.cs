using HomeInv.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace HomeInv.Common.ServiceContracts.ItemDefinition
{
    public class CreateItemDefinitionRequest : BaseHomeRelatedRequest
    {
        public ItemDefinitionEntity ItemEntity { get; set; }
        [Required]
        public string ImageFileExtension { get; set; }
    }
}
