using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo;

public class ShoppingList : BaseAuditableDbo
{
    public string Name { get; set; }
    public string Description { get; set; }

    [Required]
    public int HomeId { get; set; }
    public virtual Home Home { get; set; }

    public virtual IEnumerable<ShoppingListItem> Items { get; set; }
}
