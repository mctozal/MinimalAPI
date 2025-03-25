using API.Persistence;
using MediatR;

namespace API.Features.Product.Commands.Delete;

public class DeleteProductCommandHandler(AppDbContext dbContext) : IRequestHandler<DeleteProductCommand,Guid>
{
    public async Task<Guid> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = dbContext.Products.SingleOrDefault(x => x.Id == request.Id);
            if (product == null)
                return Guid.Empty;
            
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
            
            return product.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Guid.Empty;
        }
    }
    
}