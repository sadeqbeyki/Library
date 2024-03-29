﻿using AppFramework.Domain;

namespace Warehouse.Domain.Entities.InventoryAgg;

public class Inventory : BaseEntity<int>
{
    public int BookId { get; private set; }
    public double UnitPrice { get; private set; }
    public bool InStock { get; private set; }
    public bool IsLoaned { get; set; }
    public List<InventoryOperation> Operations { get; private set; }


    public Inventory(int bookId, double unitPrice)
    {
        BookId = bookId;
        UnitPrice = unitPrice;
        InStock = false;
        IsLoaned = false;
    }
    public void Edit(int bookId, double unitPrice)
    {
        BookId = bookId;
        UnitPrice = unitPrice;
    }
    public long CalculateCurrentCount()
    {
        var plus = Operations.Where(x => x.Operation).Sum(x => x.Count);
        var minus = Operations.Where(x => !x.Operation).Sum(x => x.Count);
        return plus - minus;
    }

    public void Increase(long count, Guid operatorId, string description)
    {
        var currentCount = CalculateCurrentCount() + count;

        var operation = new InventoryOperation(true, count, operatorId, currentCount, description, 0, Id);
        Operations.Add(operation);
        InStock = currentCount > 0;
    }
    //public void Decrease(long count, string operatorId, string description, int lendId)
    //{
    //    var currentCount = CalculateCurrentCount() - count;
    //    var operation = new InventoryOperation(false, count, operatorId, currentCount, description, lendId, Id);
    //    Operations.Add(operation);
    //    InStock = currentCount > 0;
    //}

    public void Decrease(long count, Guid operatorId, string description, int lendId)
    {
        var currentCount = CalculateCurrentCount();

        if (currentCount < count)
            throw new InvalidOperationException("Count to decrease is greater than the current inventory count.");

        object locker = new();
        //borrower 2 waiting to end thred 1
        lock (locker)
        {
            var operation = new InventoryOperation(false, count, operatorId, currentCount - count, description, lendId, Id);
            Operations.Add(operation);
            InStock = currentCount - count > 0;
        }
    }

    public void Return(long count, Guid operatorId, string description, long lendId)
    {
        var currentCount = CalculateCurrentCount() + count;
        var operation = new InventoryOperation(true, count, operatorId, currentCount, description, lendId, Id);
        Operations.Add(operation);
        InStock = currentCount > 0;
    }
}