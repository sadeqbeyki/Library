using AppFramework.Application;
using AppFramework.Domain;
using BI.ApplicationContracts.Inventory;
using BI.Domain.InventoryAgg;
using BI.Infrastructure.Configurations;
using LI.Infrastructure;
using LMS.Infrastructure;

namespace BI.Infrastructure.Repositories
{
    public class InventoryRepository : Repository<Inventory>, IInventoryRepository
    {
        private readonly InventoryDbContext _inventoryDbContext;
        private readonly BookDbContext _bookDbContext;
        private readonly LiIdentityDbContext _identityDbContext;

        public InventoryRepository(InventoryDbContext inventoryDbContext, BookDbContext bookDbContext, LiIdentityDbContext identityDbContext) : base(inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
            _bookDbContext = bookDbContext;
            _identityDbContext = identityDbContext;
        }

        public Inventory GetBookBy(Guid bookId)
        {
            return _inventoryDbContext.Inventories.FirstOrDefault(b => b.BookId == bookId);
        }

        public EditInventory GetDetails(Guid id)
        {
            return _inventoryDbContext.Inventories.Select(i => new EditInventory
            {
                Id = i.Id,
                BookId = i.BookId,
                UnitPrice = i.UnitPrice,
            }).FirstOrDefault(i => i.Id == id);
        }

        public List<InventoryOperationViewModel> GetOperationLog(Guid inventoryId)
        {
            var accounts = _identityDbContext.Users.Select(x => new { x.Id, x.FirstName, x.LastName }).ToList();
            var inventory = _inventoryDbContext.Inventories.FirstOrDefault(x => x.Id == inventoryId);
            var operations = inventory.Operations.Select(x => new InventoryOperationViewModel
            {
                Id = x.Id,
                Count = x.Count,
                CurrentCount = x.CurrentCount,
                Descriotion = x.Descriotion,
                Operation = x.Operation,
                OperationDate = x.OperationDate.ToFarsi(),
                OperatorId = x.BorrowId,
                BorrowId = x.BorrowId
            }).OrderByDescending(x => x.Id).ToList();

            foreach (var operation in operations)
            {
                operation.Operator = accounts.FirstOrDefault(x => x.Id == operation.OperatorId)?.LastName;
            }
            return operations;
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            var products = _bookDbContext.Books.Select(x => new { x.Id, x.Title }).ToList();
            var query = _inventoryDbContext.Inventories.Select(x => new InventoryViewModel
            {
                Id = x.Id,
                UnitPrice = x.UnitPrice,
                InStock = x.InStock,
                BookId = x.BookId,
                CurrentCount = x.CalculateCurrentCount(),
                CreationDate = x.CreationDate.ToFarsi()
            });
            if (searchModel.BookId > Guid.Empty)
                query = query.Where(x => x.BookId == searchModel.BookId);

            if (searchModel.InStock)
                query = query.Where(x => !x.InStock);

            var inventory = query.OrderByDescending(x => x.Id).ToList();

            inventory.ForEach(item =>
                item.Book = products.FirstOrDefault(x => x.Id == item.BookId)?.Title);

            return inventory;
        }
    }
}