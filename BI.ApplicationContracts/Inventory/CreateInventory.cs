using AppFramework.Application;
using LMS.Contracts.Book;
using System.ComponentModel.DataAnnotations;

namespace BI.ApplicationContracts.Inventory
{
    public class CreateInventory
    {
        [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]
        public long BookId { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
        public double UnitPrice { get; set; }
        public List<BookDto> Books { get; set; }
    }
}
