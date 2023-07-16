namespace LendBook.Domain.LendAgg;

public class LendOperation
{
    public long Id { get; private set; }
    public bool Operation { get; private set; }
    public long Count { get; private set; }
    public string OperatorId { get; private set; }
    public DateTime OperationDate { get; private set; }
    public long CurrentCount { get; private set; }
    public string Descriotion { get; set; }
    public Guid LendId { get; private set; }
    public Lend Lend { get; private set; }
    protected LendOperation() { }

    public LendOperation(bool operation, long count, string operationId,
        long currentCount, string descriotion, Guid lendId)
    {
        Operation = operation;
        Count = count;
        OperatorId = operationId;
        CurrentCount = currentCount;
        Descriotion = descriotion;
        LendId = lendId;
        OperationDate = DateTime.Now;
    }
}
