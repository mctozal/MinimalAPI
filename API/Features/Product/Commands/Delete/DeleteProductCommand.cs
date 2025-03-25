using MediatR;

namespace API.Features.Product.Commands.Delete;

public record DeleteProductCommand(Guid Id) : IRequest<Guid>;