using API.Features.Product.Dtos;
using MediatR;

namespace API.Features.Product.Commands.Create;

public record CreateProductCommand(string Name, string Description, decimal Price) : IRequest<ProductDto>;