using API.Features.Product.Dtos;
using MediatR;

namespace API.Features.Product.Commands.Update;

public record UpdateProductCommand(Guid Id, string Name,string Description,decimal Price) : IRequest<ProductDto>;