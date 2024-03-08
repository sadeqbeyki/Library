using AppFramework.Application;
using Library.Application.DTOs.Books;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Application.DTOs.Inventories;

public class InventoryDto
{
    [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]
    public int BookId { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
    public double UnitPrice { get; set; }
    public List<BookViewModel> Books { get; set; } = new List<BookViewModel>();
}
