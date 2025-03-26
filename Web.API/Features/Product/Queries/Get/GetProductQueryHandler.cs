using API.Features.Product.Dtos;
using API.Persistence;
using MediatR;

namespace API.Features.Product.Queries.Get;

public class GetProductQueryHandler(AppDbContext dbContext) : IRequestHandler<GetProductQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
       var product = await dbContext.Products.FindAsync(request.Id);
       if(product == null)
           return null; 
       
       return new ProductDto(Id:product.Id, Name:product.Name, Description:product.Description,Price:product.Price);
    }
    
}