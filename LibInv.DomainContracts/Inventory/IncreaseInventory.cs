namespace LibInventory.DomainContracts.Inventory;

public class IncreaseInventory
{
    public int InventoryId { get; set; }
    public long Count { get; set; }
    public string Description { get; set; }
}
