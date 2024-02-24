namespace Warehouse.Application.DTOs;

public class InventoryViewModel
{
    public int Id { get; set; }
    public string Book { get; set; }
    public int BookId { get; set; }
    public double UnitPrice { get; set; }
    public bool InStock { get; set; }
    public long CurrentCount { get; set; }
    public string CreationDate { get; set; }
}
