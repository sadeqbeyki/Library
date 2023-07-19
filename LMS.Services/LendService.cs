using AppFramework.Application;
using AutoMapper;
using LI.ApplicationContracts.Auth;
using LMS.Contracts.Lend;
using LMS.Domain.LendAgg;
using LMS.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace LMS.Services;

public class LendService : ILendService
{
    private readonly ILendRepository _lendRepository;
    private readonly IMapper _mapper;
    private readonly IAuthHelper _authHelper;
    private readonly ILibraryInventoryAcl _inventoryAcl;


    public LendService(ILendRepository lendRepository, IMapper mapper, IAuthHelper authHelper, ILibraryInventoryAcl inventoryAcl)
    {
        _lendRepository = lendRepository;
        _mapper = mapper;
        _authHelper = authHelper;
        _inventoryAcl = inventoryAcl;
    }

    #region Create
    //public async Task<OperationResult> Create(LendDto dto)
    //{
    //    OperationResult operationResult = new();
    //    var currentEmployeeId = _authHelper.CurrentAccountId();
    //    Lend lend = new()
    //    {
    //        Id = dto.Id,
    //        MemberID = dto.MemberID,
    //        EmployeeId = dto.EmployeeId,
    //        LendDate = dto.LendDate,
    //        IdealReturnDate = dto.IdealReturnDate,
    //        ReturnDate = dto.ReturnDate,
    //        Description = dto.Description,
    //        Items = new List<LendItem>(dto.Items.Select(i => new LendItem
    //        {
    //            BookId = i.BookId,
    //            Count = i.Count,
    //            LendId = i.LendId
    //        }).ToList())
    //    };

    //    await _lendRepository.CreateAsync(lend);
    //    return operationResult.Succeeded();
    //}
    public async Task<OperationResult> Lending(LendDto dto)
    {
        OperationResult result = new();
        var currentEmployeeId = _authHelper.CurrentAccountId();
        Lend lend = new(dto.MemberID, currentEmployeeId, dto.LendDate, dto.IdealReturnDate,
            dto.ReturnEmployeeID, dto.ReturnDate, dto.Description);

        foreach (var lendItem in lend.Items)
        {
            LendItem lendItems = new(lendItem.BookId, lendItem.Count);
            lend.AddItem(lendItems);
        }

        await _lendRepository.CreateAsync(lend);
        return result.Succeeded();
    }

    public async Task<OperationResult> LendingRegistration(Guid lendId)
    {
        OperationResult operationResult = new();
        Lend lend = await _lendRepository.GetByIdAsync(lendId);
        if (lend == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        _inventoryAcl.LendFromInventory(lend.Items);
        _lendRepository.SaveChanges();
        return operationResult.Succeeded();
    }
    #endregion

    #region Read


    public async Task<List<LendDto>> GetAllLends()
    {
        var loans = await _lendRepository.GetAll().ToListAsync();
        return _mapper.Map<List<LendDto>>(loans);
    }
    public List<LendItemDto> GetItems(Guid lendId)
    {
        return _lendRepository.GetItems(lendId);
    }

    public async Task<LendDto> GetLendById(Guid id)
    {
        var loan = await _lendRepository.GetByIdAsync(id);
        return _mapper.Map<LendDto>(loan);
    }

    public async Task<IEnumerable<LendDto>> GetLendsByMemberId(string memberId)
    {
        List<Lend> loans = await _lendRepository.GetAll().ToListAsync();
        var result = loans.Where(l => l.MemberID == memberId).ToList();
        return _mapper.Map<List<LendDto>>(result);
    }

    public async Task<IEnumerable<LendDto>> GetLendsByEmployeeId(string employeeId)
    {
        List<Lend> loans = await _lendRepository.GetAll().ToListAsync();
        var result = loans.Where(l => l.EmployeeId == employeeId).ToList();
        return _mapper.Map<List<LendDto>>(result);
    }

    public async Task<IEnumerable<LendDto>> GetOverdueLends()
    {
        List<Lend> loans = await _lendRepository.GetAll().ToListAsync();
        var result = loans.Where(l => l.ReturnDate == null && l.IdealReturnDate < DateTime.Now);
        return _mapper.Map<List<LendDto>>(result);
    }

    #endregion

    #region Update
    public async Task<OperationResult> Update(LendDto dto)
    {
        OperationResult operationResult = new();

        var loan = await _lendRepository.GetByIdAsync(dto.Id);
        if (loan == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        loan.Edit(dto.MemberID, dto.EmployeeId, dto.LendDate, dto.IdealReturnDate,
            dto.ReturnEmployeeID, dto.ReturnDate, dto.Description);

        await _lendRepository.UpdateAsync(loan);
        return operationResult.Succeeded();
    }
    #endregion

    #region Delete
    public async Task Delete(Guid id)
    {
        var lend = await _lendRepository.GetByIdAsync(id);
        await _lendRepository.DeleteAsync(lend);
    }
    #endregion

    #region Search
    public List<LendDto> Search(LendSearchModel searchModel)
    {
        return _lendRepository.Search(searchModel);
    }
    #endregion

}
