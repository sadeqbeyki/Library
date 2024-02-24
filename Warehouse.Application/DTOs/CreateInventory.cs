using AppFramework.Application;
using LibBook.DomainContracts.Book;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Application.DTOs;

public class CreateInventory
{
    [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]
    public int BookId { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
    public double UnitPrice { get; set; }
    public List<BookViewModel> Books { get; set; } = new List<BookViewModel>();
}
