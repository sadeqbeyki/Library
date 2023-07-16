using AppFramework.Application;
using BI.ApplicationContracts.Inventory;
using BI.Domain.InventoryAgg;
using LI.ApplicationContracts.Auth;

namespace BI.ApplicationServices
{
    public class InventoryService : IInventoryService
    {
        private readonly IAuthHelper _authHelper;
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository, IAuthHelper authHelper)
        {
            _inventoryRepository = inventoryRepository;
            _authHelper = authHelper;
        }

        public async Task<OperationResult> Create(CreateInventory command)
        {
            var operation = new OperationResult();
            if (_inventoryRepository.Exists(x => x.BookId == command.BookId))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var inventory = new Inventory(command.BookId, command.UnitPrice);
            await _inventoryRepository.CreateAsync(inventory);
            //_inventoryRepository.SaveChanges();
            return operation.Succeeded();
        }

        public async Task<OperationResult> Edit(EditInventory command)
        {
            var operation = new OperationResult();
            var inventory = await _inventoryRepository.GetByIdAsync(command.Id);
            if (inventory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_inventoryRepository.Exists(x => x.BookId == command.BookId && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            inventory.Edit(command.BookId, command.UnitPrice);
            _inventoryRepository.SaveChanges();
            return operation.Succeeded();
        }

        public EditInventory GetDetails(Guid id)
        {
            return _inventoryRepository.GetDetails(id);
        }

        public List<InventoryOperationViewModel> GetOperationLog(Guid operationId)
        {
            return _inventoryRepository.GetOperationLog(operationId);
        }

        public async Task<OperationResult> Increase(IncreaseInventory command)
        {
            var operation = new OperationResult();
            var inventory = await _inventoryRepository.GetByIdAsync(command.InventoryId);
            if (inventory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            var operatorId = _authHelper.CurrentAccountId();
            inventory.Increase(command.Count, operatorId, command.Description);
            _inventoryRepository.SaveChanges();
            return operation.Succeeded();
        }

        public async Task<OperationResult> Decrease(DecreaseInventory command)
        {
            var operation = new OperationResult();
            var inventory = await _inventoryRepository.GetByIdAsync(command.InventoryId);
            if (inventory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            var operatorId = _authHelper.CurrentAccountId();
            inventory.Decrease(command.Count, operatorId, command.Description, 0);
            _inventoryRepository.SaveChanges();
            return operation.Succeeded();
        }


        public OperationResult Decrease(List<DecreaseInventory> command)
        {
            var operation = new OperationResult();
            var operatorId = _authHelper.CurrentAccountId();
            foreach (var item in command)
            {
                var inventory = _inventoryRepository.GetBookBy(item.BookId);
                inventory.Decrease(item.Count, operatorId, item.Description, item.LendId);
            }
            _inventoryRepository.SaveChanges();
            return operation.Succeeded();
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            return _inventoryRepository.Search(searchModel);
        }
    }
}
