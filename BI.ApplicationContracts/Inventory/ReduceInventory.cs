namespace BI.ApplicationContracts.Inventory
{
    public class ReduceInventory
    {
        public Guid InventoryId { get; set; }
        public Guid BookId { get; set; }
        public long Count { get; set; }
        public string Description { get; set; }
        public long BorrowId { get; set; }

        public ReduceInventory() { }

        public ReduceInventory(Guid bookId, long count, string description, long borrowId)
        {
            BookId = bookId;
            Count = count;
            Description = description;
            BorrowId = borrowId;
        }
    }
}
