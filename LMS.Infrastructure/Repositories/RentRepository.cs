using LI.Infrastructure;

namespace LMS.Infrastructure.Repositories;

public class RentRepository
{
    private readonly BookDbContext _bookContext;
    private readonly LiIdentityDbContext _userContext;

    public RentRepository(BookDbContext context, LiIdentityDbContext accountContext) : base(context)
    {
        _bookContext = context;
        _userContext = accountContext;
    }

    public double GetAmountBy(string id)
    {
        var order = _bookContext.Orders
            .Select(x => new { x.PayAmount, x.Id })
            .FirstOrDefault(x => x.Id == id);
        if (order != null)
            return order.PayAmount;
        return 0;
    }

    public List<OrderItemViewModel> GetItems(long orderId)
    {
        var products = _bookContext.Products.Select(x => new { x.Id, x.Name }).ToList();
        var order = _bookContext.Orders.FirstOrDefault(x => x.Id == orderId);
        if (order == null)
            return new List<OrderItemViewModel>();

        var items = order.Items.Select(x => new OrderItemViewModel
        {
            Id = x.Id,
            Count = x.Count,
            DiscountRate = x.DiscountRate,
            OrderId = x.OrderId,
            ProductId = x.ProductId,
            UnitPrice = x.UnitPrice
        }).ToList();

        foreach (var item in items)
        {
            item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name;
        }

        return items;
    }

    public List<OrderViewModel> Search(OrderSearchModel searchModel)
    {
        var accounts = _userContext.Accounts.Select(x => new { x.Id, x.FullName }).ToList();
        var query = _bookContext.Orders.Select(x => new OrderViewModel
        {
            Id = x.Id,
            AccountId = x.AccountId,
            DiscountAmount = x.DiscountAmount,
            IsCanceled = x.IsCanceled,
            IsPaid = x.IsPaid,
            IssueTrackingNo = x.IssueTrackingNo,
            PayAmount = x.PayAmount,
            PaymentMethodId = x.PaymentMethod,
            RefId = x.RefId,
            TotalAmount = x.TotalAmount,
            CreationDate = x.CreationDate.ToFarsi()
        });

        query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);

        if (searchModel.AccountId > 0) query = query.Where(x => x.AccountId == searchModel.AccountId);

        var orders = query.OrderByDescending(x => x.Id).ToList();
        foreach (var order in orders)
        {
            order.AccountFullName = accounts.FirstOrDefault(x => x.Id == order.AccountId)?.FullName;
            order.PaymentMethod = PaymentMethod.GetBy(order.PaymentMethodId).Name;
        }

        return orders;
    }
}
}
