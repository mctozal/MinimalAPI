using API.Features.Product.Dtos;
using API.Persistence;
using MediatR;

namespace API.Features.Product.Commands.Update;

public class UpdateProductCommandHandler(AppDbContext dbContext) : IRequestHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await dbContext.Products.FindAsync(request.Id);
            if (product == null)
                return null;
                    
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            await dbContext.SaveChangesAsync(cancellationToken);
       
            return new ProductDto(product.Id, product.Name, product.Description, product.Price);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}