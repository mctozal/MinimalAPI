using API.Features.Product.Dtos;
using MediatR;

namespace API.Features.Product.Queries.Get;

public record GetProductQuery(Guid Id) : IRequest<ProductDto>;
