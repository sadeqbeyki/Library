using System.ComponentModel.DataAnnotations;

namespace LibInventory.DomainContracts.Inventory;

public class DecreaseInventory
{
    public int InventoryId { get; set; }
    public int BookId { get; set; }
    [Required(ErrorMessage = "Number field is required.")]
    public long Count { get; set; }
    [Required(ErrorMessage = "Description field is required.")]
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
