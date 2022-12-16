namespace HomeInv.Persistence.Dbo;

public class ShoppingListItem : BaseAuditableDbo
{
    public int ShoppingListId { get; set; }
    public virtual ShoppingList ShoppingList { get; set; }

    public string ItemName { get; set; }

    public decimal Amount { get; set; }

    #region Size Properties
    public int SizeUnitId { get; set; }
    public virtual SizeUnit SizeUnit { get; set; }
    #endregion
}
