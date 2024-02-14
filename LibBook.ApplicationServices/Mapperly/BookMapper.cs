using LibBook.Domain.BookAgg;
using LibBook.DomainContracts.Book;
using Riok.Mapperly.Abstractions;

namespace LibBook.ApplicationServices.Mapperly;

//// Enums of source and target have different numeric values -> use ByName strategy to map them
//[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
[Mapper]
public static partial class BookMapper
{
    //[MapProperty(nameof(Book.Manufacturer), nameof(BookViewModel.Producer))]
    public static partial BookViewModel BookToBookDto(Book book);
    public static partial Book BookDtoToBook(BookViewModel book);
}
