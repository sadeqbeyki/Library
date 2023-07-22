namespace BI.ApplicationContracts.Inventory;

public class DecreaseInventory
{
    public int InventoryId { get; set; }
    public int BookId { get; set; }
    public long Count { get; set; }
    public string Description { get; set; }
    public int LendId { get; set; }

    public DecreaseInventory() { }

    public DecreaseInventory(int bookId, long count, string description, int lendId)
    {
        BookId = bookId;
        Count = count;
        Description = description;
        LendId = lendId;
    }
}
