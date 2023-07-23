namespace LibInventory.Domain.InventoryAgg;

public class InventoryOperation
{
    public long Id { get; private set; }
    public bool Operation { get; private set; }
    public long Count { get; private set; }
    public string OperatorId { get; private set; }
    public DateTime OperationDate { get; private set; }
    public long CurrentCount { get; private set; }
    public string Descriotion { get; set; }
    public long LendId { get; private set; }
    public int InventoryId { get; private set; }
    public Inventory Inventory { get; private set; }
    protected InventoryOperation() { }

    public InventoryOperation(bool operation, long count, string operationId,
        long currentCount, string descriotion, long lendId, int inventoryId)
    {
        Operation = operation;
        Count = count;
        OperatorId = operationId;
        CurrentCount = currentCount;
        Descriotion = descriotion;
        LendId = lendId;
        InventoryId = inventoryId;
        OperationDate = DateTime.Now;
    }
}
