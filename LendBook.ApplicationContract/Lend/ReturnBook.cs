namespace BI.ApplicationContracts.Inventory
{
    public class ReturnBook
    {
        public Guid LendId { get; set; }
        public long Count { get; set; }
        public string Description { get; set; }
    }
}
