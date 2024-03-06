using MediatR;

namespace Library.Application.CQRS.Queries.Lend;

public record SearchLendQuery:IRequest<List>
{
}
