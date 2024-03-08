using Library.Application.DTOs.Books;
using Library.Domain.Entities.BookAgg;
using Riok.Mapperly.Abstractions;

namespace Library.Persistance.Services.Mapperly;

//// Enums of source and target have different numeric values -> use ByName strategy to map them
//[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
[Mapper]
public static partial class BookMapper
{
    //[MapProperty(nameof(Book.Manufacturer), nameof(BookViewModel.Producer))]
    public static partial BookViewModel BookToBookDto(Book book);
    public static partial Book BookDtoToBook(BookViewModel book);
}
