using Library.Application.Contracts;
using Library.Application.DTOs.Book;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Library.Application.CQRS.Commands.Book;

public record UpdateBookCommand(BookViewModel Dto, IFormFile pictureFile) : IRequest<bool>
{
}

internal sealed class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, bool>
{
    private readonly IBookService _bookService;

    public UpdateBookCommandHandler(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task<bool> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var result = await _bookService.Update(request.Dto, request.pictureFile);
        if( result.IsSucceeded)
            return true;    
        return false;
    }
}
