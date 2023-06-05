namespace BI.ApplicationContracts.Inventory
{
    public class InventorySearchModel
    {
        public Guid BookId { get; set; }
        public bool InStock { get; set; }
    }
}
