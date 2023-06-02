namespace BI.ApplicationContracts.Inventory
{
    public class IncreaseInventory
    {
        public Guid InventoryId { get; set; }
        public long Count { get; set; }
        public string Description { get; set; }
    }
}
