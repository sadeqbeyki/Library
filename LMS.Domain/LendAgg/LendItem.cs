using AppFramework.Domain;

namespace LMS.Domain.LendAgg;

public class LendItem : BaseEntity
{
    public int BookId { get; private set; }
    public int Count { get; private set; }
    public int LendId { get; private set; }
    public Lend Lend { get; private set; }

    public LendItem(int bookId, int count)
    {
        BookId = bookId;
        Count = count;
    }
}