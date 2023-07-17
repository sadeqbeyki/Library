using AppFramework.Domain;

namespace LMS.Domain.LendAgg;

public class LendItem : BaseEntity
{
    public Guid BookId { get; private set; }
    public int Count { get; private set; }
    public long LendId { get; private set; }
    public Lend Lend { get; private set; }

    public LendItem(Guid bookId, int count)
    {
        BookId = bookId;
        Count = count;
    }
}