namespace BI.ApplicationContracts.Inventory
{
    public class BookLoan
    {
        public Guid LendId { get; set; }
        public Guid BookId { get; set; }
        public long Count { get; set; }
        public string Description { get; set; }

        public BookLoan() { }

        public BookLoan(Guid bookId, long count, string description)
        {
            BookId = bookId;
            Count = count;
            Description = description;
        }
    }
}
