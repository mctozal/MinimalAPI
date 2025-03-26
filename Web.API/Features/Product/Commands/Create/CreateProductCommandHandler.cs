using API.Features.Product.Dtos;
using API.Persistence;
using MediatR;

namespace API.Features.Product.Commands.Create;

public class CreateProductCommandHandler(AppDbContext dbContext) : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await dbContext.Products.AddAsync(new Domain.Product(name: request.Name,
                description: request.Description, price: request.Price), cancellationToken);
        
         
            await dbContext.SaveChangesAsync(cancellationToken);
        
            
            return new ProductDto(product.Entity.Id,Name:product.Entity.Name, Description:product.Entity.Description, Price:product.Entity.Price);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
};