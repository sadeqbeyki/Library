namespace BI.ApplicationContracts.Inventory
{
    public class DecreaseInventory
    {
        public Guid InventoryId { get; set; }
        public Guid BookId { get; set; }
        public long Count { get; set; }
        public string Description { get; set; }
        public long LendId { get; set; }

        public DecreaseInventory() { }

        public DecreaseInventory(Guid bookId, long count, string description, long lendId)
        {
            BookId = bookId;
            Count = count;
            Description = description;
            LendId = lendId;
        }
    }
}
