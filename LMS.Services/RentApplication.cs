using AppFramework.Application;
using LI.ApplicationContracts.Auth;
using LMS.Contracts.Rent;
using LMS.Domain.OrderAgg;
using Microsoft.Extensions.Configuration;

namespace LMS.Services;

public class RentApplication : IRentApplication
{
    private readonly IAuthHelper _authHelper;
    private readonly IConfiguration _configuration;
    private readonly IRentRepository _rentRepository;
    private readonly ILibraryInventoryAcl _libraryInventoryAcl;

    private readonly IShopAccountAcl _shopAccountAcl;

    public RentApplication(IAuthHelper authHelper,
        IConfiguration configuration,
        IOrderRepository rentRepository,
        IShopInventoryAcl shopInventoryAcl,
        ISmsService smsService,
        IShopAccountAcl shopAccountAcl)
    {
        _authHelper = authHelper;
        _configuration = configuration;
        _rentRepository = rentRepository;
        _libraryInventoryAcl = shopInventoryAcl;
        _smsService = smsService;
        _shopAccountAcl = shopAccountAcl;
    }

    public long PlaceOrder(Cart cart)
    {
        var currentAccountId = _authHelper.CurrentAccountId();
        var order = new Order(currentAccountId, cart.PaymentMethod, cart.TotalAmount, cart.DiscountAmount,
            cart.PayAmount);

        foreach (var cartItem in cart.Items)
        {
            var orderItem = new OrderItem(cartItem.Id, cartItem.Count, cartItem.UnitPrice, cartItem.DiscountRate);
            order.AddItem(orderItem);
        }

        _rentRepository.Create(order);
        _rentRepository.SaveChanges();
        return order.Id;
    }

    public double GetAmountBy(long id)
    {
        return _rentRepository.GetAmountBy(id);
    }

    public void Cancel(long id)
    {
        var order = _rentRepository.Get(id);
        order.Cancel();
        _rentRepository.SaveChanges();
    }

    public string PaymentSucceeded(long orderId, long refId)
    {
        var order = _rentRepository.Get(orderId);
        order.PaymentSucceeded(refId);
        var symbol = _configuration.GetValue<string>("Symbol");
        var issueTrackingNo = CodeGenerator.Generate(symbol);
        order.SetIssueTrackingNo(issueTrackingNo);
        if (!_libraryInventoryAcl.ReduceFromInventory(order.Items)) return "";

        _rentRepository.SaveChanges();

        var (name) = _shopAccountAcl.GetAccountBy(order.AccountId);


        return issueTrackingNo;
    }

    public List<OrderItemViewModel> GetItems(long orderId)
    {
        return _rentRepository.GetItems(orderId);
    }

    public List<OrderViewModel> Search(OrderSearchModel searchModel)
    {
        return _rentRepository.Search(searchModel);
    }

}