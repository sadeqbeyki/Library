using AppFramework.Application;
using AutoMapper;
using LI.ApplicationContracts.Auth;
using LMS.Contracts.Lend;
using LMS.Domain.LendAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LMS.Services;

public class LendService : ILendService
{
    private readonly ILendRepository _lendRepository;
    private readonly IMapper _mapper;
    private readonly IAuthHelper _authHelper;

    public LendService(ILendRepository lendRepository, IMapper mapper, IAuthHelper authHelper)
    {
        _lendRepository = lendRepository;
        _mapper = mapper;
        _authHelper = authHelper;
    }

    public async Task<List<LendDto>> GetAllLends()
    {
        var loans = await _lendRepository.GetAll().ToListAsync();
        return _mapper.Map<List<LendDto>>(loans);
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

    public async Task<LendDto> Create(LendDto dto)
    {
        Lend loan = new(dto.BookId, dto.MemberID, dto.EmployeeId, dto.LendDate, dto.IdealReturnDate,
            dto.ReturnEmployeeID, dto.ReturnDate, dto.Description);

        var result = await _lendRepository.CreateAsync(loan);
        return _mapper.Map<LendDto>(result);
    }

    public async Task<OperationResult> Update(LendDto dto)
    {
        OperationResult operationResult = new();

        var loan = await _lendRepository.GetByIdAsync(dto.Id);
        if (loan == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        loan.Edit(dto.BookId, dto.MemberID, dto.EmployeeId, dto.LendDate, dto.IdealReturnDate,
            dto.ReturnEmployeeID, dto.ReturnDate, dto.Description);

        await _lendRepository.UpdateAsync(loan);
        return operationResult.Succeeded();
    }

    public async Task Delete(Guid id)
    {
        var loan = await _lendRepository.GetByIdAsync(id);
        await _lendRepository.DeleteAsync(loan);
    }


    public long PlaceOrder(Cart dto)
    {
        var currentAccountId = _authHelper.CurrentAccountId();
        //Lend lend = new(dto.BookId, dto.MemberID, dto.EmployeeId, dto.LendDate,
        //    dto.IdealReturnDate.ToFarsi(), dto.ReturnEmployeeID, dto.ReturnDate, dto.Description);
        Lend cart = new();
        foreach (var lendItem in dto.Items)
        {
            LendItem lendItems = new(lendItem.Id, lendItem.Count);
            cart.Add(lendItems);
        }

        _lendRepository.Create(lend);
        _lendRepository.SaveChanges();
        return lend.Id;
    }

    public string PaymentSucceeded(long orderId, long refId)
    {
        var order = _orderRepository.Get(orderId);
        order.PaymentSucceeded(refId);
        var symbol = _configuration.GetValue<string>("Symbol");
        var issueTrackingNo = CodeGenerator.Generate(symbol);
        order.SetIssueTrackingNo(issueTrackingNo);
        if (!_shopInventoryAcl.ReduceFromInventory(order.Items)) return "";

        _orderRepository.SaveChanges();

        var (name, mobile) = _shopAccountAcl.GetAccountBy(order.AccountId);

        _smsService.Send(mobile,
            $"{name} گرامی سفارش شما با شماره پیگیری {issueTrackingNo} با موفقیت پرداخت شد و ارسال خواهد شد.");
        return issueTrackingNo;
    }

    public List<LendItemDto> GetItems(Guid lendId)
    {
        return _lendRepository.GetItems(lendId);
    }

    public List<LendDto> Search(LendSearchModel searchModel)
    {
        return _lendRepository.Search(searchModel);
    }
}
