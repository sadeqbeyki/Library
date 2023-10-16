using AppFramework.Application;
using LibIdentity.Domain.UserAgg;
using LibIdentity.DomainContracts.Auth;
using LibInventory.Domain.InventoryAgg;
using LibInventory.DomainContracts.Inventory;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LibInventory.ApplicationServices
{
    public class InventoryService : IInventoryService
    {
        private readonly IAuthHelper _authHelper;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public InventoryService(IInventoryRepository inventoryRepository, IAuthHelper authHelper, IHttpContextAccessor contextAccessor)
        {
            _inventoryRepository = inventoryRepository;
            _authHelper = authHelper;
            _contextAccessor = contextAccessor;
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

            //string operatorId = _authHelper.CurrentAccountId();
            var operatorId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
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
                var operatorId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                inventory.Decrease(command.Count, operatorId, command.Description, command.LendId);

                _inventoryRepository.SaveChanges();
                return operation.Succeeded();
            }
            catch (InvalidOperationException ex)
            {
                // در صورتی که تعداد کاهش بیشتر از موجودی باشد، این قسمت اجرا می‌شود.
                return operation.Failed(ex.Message);
            }
        }

        public OperationResult Decrease(List<DecreaseInventory> command)
        {
            var operation = new OperationResult();
            var operatorId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
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
            var operatorId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var inventory = _inventoryRepository.GetBy(command.BookId);
            if (inventory == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);

            inventory.Return(command.Count, operatorId, command.Description, command.LendId);
            _inventoryRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        //public OperationResult Borrowing(DecreaseInventory command)
        //{
        //    var operation = new OperationResult();
        //    var inventory = _inventoryRepository.GetBy(command.BookId);
        //    if (inventory == null)
        //        return operation.Failed(ApplicationMessages.RecordNotFound);

        //    var operatorId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    try
        //    {
        //        inventory.Decrease(command.Count, operatorId, command.Description, command.LendId);
        //        _inventoryRepository.SaveChanges();
        //        return operation.Succeeded();

        //    }
        //    catch
        //    {
        //        return operation.Failed("عدم موجودی کتاب");
        //    }
        //}

        public OperationResult Borrowing(DecreaseInventory command)
        {
            var operation = new OperationResult();
            var inventory = _inventoryRepository.GetBy(command.BookId);
            if (inventory == null)
            {
                return operation.Failed(ApplicationMessages.RecordNotFound);
            }

            var decreaseResult = DecreaseInventorySafely(inventory, command.Count, command.Description, command.LendId);

            if (decreaseResult.IsSucceeded)
            {
                _inventoryRepository.SaveChanges();
            }
            else
            {
                return decreaseResult;
            }

            return operation.Succeeded();
        }


        private OperationResult DecreaseInventorySafely(Inventory inventory, long count, string description, int lendId)
        {
            OperationResult operation = new();
            try
            {
                inventory.Decrease(count, GetCurrentOperatorId(), description, lendId);
                return operation.Succeeded();
            }
            catch (InvalidOperationException ex)
            {
                return operation.Failed(ex.Message);
            }
        }

        private string GetCurrentOperatorId()
        {
            return _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}
