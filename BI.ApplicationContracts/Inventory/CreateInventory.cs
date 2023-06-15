using AppFramework.Application;
using LMS.Contracts.Book;
using System.ComponentModel.DataAnnotations;

namespace BI.ApplicationContracts.Inventory
{
    public class CreateInventory
    {
        [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]
        public Guid BookId { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
        public double UnitPrice { get; set; }
        public List<BookViewModel> Books { get; set; } =new List<BookViewModel>();
    }
}
