using API.Features.Product.Dtos;
using MediatR;

namespace API.Features.Product.Queries.List;

public record ListProductsQuery : IRequest<List<ProductDto>>;

