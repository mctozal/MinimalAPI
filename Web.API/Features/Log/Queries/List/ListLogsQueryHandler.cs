using API.Features.Log.Dtos;
using API.Features.Product.Dtos;
using API.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Log.Queries.List;

public class ListLogsQueryHandler(AppDbContext dbContext) : IRequestHandler<ListLogsQuery, List<LogDto>>  
{
    public async Task<List<LogDto>> Handle(ListLogsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Logs.Select(x => new LogDto(x.Id,
            x.RequestBody,
            x.ResponseBody,
            x.RequestDate,
            x.ResponseDate)).ToListAsync();
    }
}