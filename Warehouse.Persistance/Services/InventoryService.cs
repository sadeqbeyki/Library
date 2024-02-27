using AppFramework.Application;
using Library.ACL.Identity;
using Warehouse.Application.Contracts;
using Warehouse.Application.DTOs;
using Warehouse.Application.DTOs.Inventory;
using Warehouse.Application.DTOs.InventoryOperation;
using Warehouse.Domain.Entities.InventoryAgg;

namespace Warehouse.Service.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ILibraryIdentityAcl _identityAcl;
        public InventoryService(IInventoryRepository inventoryRepository, ILibraryIdentityAcl identityAcl)
        {
            _inventoryRepository = inventoryRepository;
            _identityAcl = identityAcl;
        }

        public async Task<OperationResult> Create(CreateInventory command)
        {
            var operation = new OperationResult();
            if (_inventoryRepository.Exists(x => x.BookId == command.BookId))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var inventory = new Inventory(command.BookId, command.UnitPrice);
            await _inventoryRepository.CreateAsync(inventory);
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

        public EditInventory GetDetails(int id)
        {
            return _inventoryRepository.GetDetails(id);
        }

        public List<InventoryOperationViewModel> GetOperationLog(int operationId)
        {
            return _inventoryRepository.GetOperationLog(operationId);
        }

        public async Task<OperationResult> Increase(IncreaseInventory command)
        {
            var operationResult = new OperationResult();
            var inventory = await _inventoryRepository.GetByIdAsync(command.InventoryId);
            if (inventory == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);

            var operatorId = _identityAcl.GetCurrentUserId();
            inventory.Increase(command.Count, operatorId, command.Description);
            _inventoryRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public async Task<OperationResult> Decrease(DecreaseInventory command)
        {
            var operation = new OperationResult();

            var inventory = await _inventoryRepository.GetByIdAsync(command.InventoryId);
            if (inventory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            try
            {
                var operatorId = _identityAcl.GetCurrentUserId();
                inventory.Decrease(command.Count, operatorId, command.Description, command.LendId);

                _inventoryRepository.SaveChanges();
                return operation.Succeeded();
            }
            catch (InvalidOperationException ex)
            {
                return operation.Failed(ex.Message);
            }
        }

        public OperationResult Decrease(List<DecreaseInventory> command)
        {
            var operation = new OperationResult();
            var operatorId = _identityAcl.GetCurrentUserId();
            foreach (var item in command)
            {
                var inventory = _inventoryRepository.GetBy(item.BookId);
                inventory.Decrease(item.Count, operatorId, item.Description, item.LendId);
            }
            _inventoryRepository.SaveChanges();
            return operation.Succeeded();
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            return _inventoryRepository.Search(searchModel);
        }

        public OperationResult Returning(ReturnBook command)
        {
            OperationResult operationResult = new();

            var inventory = _inventoryRepository.GetBy(command.BookId);
            if (inventory == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);

            //if (inventory.IsLoaned == false)
            //    return operationResult.Failed(ApplicationMessages.BookWasAlreadyReturned);

            var returnResult = ReturnToInventory(inventory, command.Count, command.Description, command.LendId);
            if (returnResult.IsSucceeded)
            {
                _inventoryRepository.SaveChanges();
            }
            else
            {
                return operationResult.Failed(ApplicationMessages.ReturnFailed);
            }
            return operationResult.Succeeded();
        }

        private OperationResult ReturnToInventory(Inventory inventory, long count, string description, int lendId)
        {
            OperationResult operation = new();
            try
            {
                inventory.IsLoaned = false;
                inventory.Return(count, _identityAcl.GetCurrentUserId(), description, lendId);
                return operation.Succeeded();
            }
            catch
            {
                return operation.Failed(ApplicationMessages.ReturnFailed);
            }
        }

        public OperationResult Lending(DecreaseInventory command)
        {
            OperationResult operationResult = new();

            var inventory = _inventoryRepository.GetBy(command.BookId);
            if (inventory == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);

            if (inventory.IsLoaned == true && inventory.CalculateCurrentCount() <= 0)
                return operationResult.Failed(ApplicationMessages.BookIsLoaned);

            var lendResult = LoanFromInventory(inventory, command.Count, command.Description, command.LendId);

            if (lendResult.IsSucceeded)
            {
                _inventoryRepository.SaveChanges();
            }
            else
            {
                return lendResult.Failed(ApplicationMessages.LendFailed);
            }

            return operationResult.Succeeded();
        }

        private OperationResult LoanFromInventory(Inventory inventory, long count, string description, int lendId)
        {
            OperationResult operation = new();
            try
            {
                inventory.IsLoaned = true;
                inventory.Decrease(count, _identityAcl.GetCurrentUserId(), description, lendId);
                return operation.Succeeded();
            }
            catch /*(InvalidOperationException ex)*/
            {
                return operation.Failed(ApplicationMessages.TheBookIsNotInStock);
            }
        }
    }
}
