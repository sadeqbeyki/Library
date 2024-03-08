namespace Warehouse.Application.DTOs.InventoryOperation;

public class IncreaseInventory
{
    public int InventoryId { get; set; }
    public long Count { get; set; }
    public string Description { get; set; }
}


public class ReturnBook
{
    public int InventoryId { get; set; }
    public int BookId { get; set; }
    public long Count { get; set; }
    public string Description { get; set; }
    public int LendId { get; set; }

    public ReturnBook(int bookId, long count, string description, int lendId)
    {
        BookId = bookId;
        Count = count;
        Description = description;
        LendId = lendId;
    }
}