using AppFramework.Application;
using AppFramework.Domain;
using Identity.Persistance;
using LibBook.Infrastructure;
using LibInventory.Domain.InventoryAgg;
using LibInventory.DomainContracts.Inventory;

namespace LibInventory.Infrastructure.Repositories
{
    public class InventoryRepository : Repository<Inventory, int>, IInventoryRepository
    {
        private readonly InventoryDbContext _inventoryDbContext;
        private readonly BookDbContext _bookDbContext;
        private readonly AppIdentityDbContext _identityDbContext;

        public InventoryRepository(InventoryDbContext inventoryDbContext, BookDbContext bookDbContext, AppIdentityDbContext identityDbContext) : base(inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
            _bookDbContext = bookDbContext;
            _identityDbContext = identityDbContext;
        }

        public Inventory GetBy(int bookId)
        {
            return _inventoryDbContext.Inventory.FirstOrDefault(b => b.BookId == bookId);
        }

        public EditInventory GetDetails(int id)
        {
            return _inventoryDbContext.Inventory.Select(i => new EditInventory
            {
                Id = i.Id,
                BookId = i.BookId,
                UnitPrice = i.UnitPrice,
            }).FirstOrDefault(i => i.Id == id);
        }

        public List<InventoryOperationViewModel> GetOperationLog(int inventoryId)
        {
            var accounts = _identityDbContext.Users.Select(x => new { x.Id, FullName = x.FirstName + ' ' + x.LastName }).ToList();
            var inventory = _inventoryDbContext.Inventory.FirstOrDefault(x => x.Id == inventoryId);
            var operations = inventory.Operations.Select(x => new InventoryOperationViewModel
            {
                Id = x.Id,
                Count = x.Count,
                CurrentCount = x.CurrentCount,
                Description = x.Description,
                Operation = x.Operation,
                OperationDate = x.OperationDate.ToFarsi(),
                OperatorId = x.OperatorId,
                LendId = x.LendId
            }).OrderByDescending(x => x.Id).ToList();

            foreach (var operation in operations)
            {
                operation.Operator = accounts.FirstOrDefault(x => x.Id.ToString() == operation.OperatorId)?.FullName;
            }
            return operations;
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            var products = _bookDbContext.Books.Select(x => new { x.Id, x.Title }).ToList();
            var query = _inventoryDbContext.Inventory.Select(x => new InventoryViewModel
            {
                Id = x.Id,
                UnitPrice = x.UnitPrice,
                InStock = x.InStock,
                BookId = x.BookId,
                CurrentCount = x.CalculateCurrentCount(),
                CreationDate = x.CreationDate.ToFarsi()
            });
            if (searchModel.BookId > 0)
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