namespace BI.ApplicationContracts.Inventory
{
    public class InventoryViewModel
    {
        public Guid Id { get; set; }
        public string Book { get; set; }
        public Guid BookId { get; set; }
        public double UnitPrice { get; set; }
        public bool InStock { get; set; }
        public long CurrentCount { get; set; }
        public string CreationDate { get; set; }
    }
}
