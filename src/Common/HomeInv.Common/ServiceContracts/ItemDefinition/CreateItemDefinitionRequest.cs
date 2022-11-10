using HomeInv.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace HomeInv.Common.ServiceContracts.ItemDefinition
{
    public class CreateItemDefinitionRequest : BaseRequest
    {
        public ItemDefinitionEntity ItemEntity { get; set; }
        [Required]
        public int HomeId { get; set; }
    }
}
