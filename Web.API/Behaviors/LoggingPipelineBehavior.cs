using API.Domain;
using API.Persistence;
using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.IO;

namespace API.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;
    private readonly AppDbContext _dbContext;
    public LoggingPipelineBehavior(
        AppDbContext dbContext, ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
       
        var dbLog = new Log(request.ToString(), null, DateTimeOffset.Now, DateTimeOffset.Now);

        //Request
        _logger.LogInformation(
            "Handling {Name}. Request Body: {@RequestBody}. {@Date}",
            requestName,
            request, 
            DateTime.UtcNow);        
        var result = await next();
        
        //Response
        _logger.LogInformation(
            "Completed {Name}. Request Body: {@RequestBody}. Response Body: {@ResponseBody}. {@Date}",
            requestName,
            request,
            result, 
            DateTime.UtcNow);
        

        dbLog.ResponseBody =  String.Format("{0}", result);
        dbLog.ResponseDate = DateTime.UtcNow;
        
        await _dbContext.Logs.AddAsync(dbLog);
        await _dbContext.SaveChangesAsync();
        
        return result;
    }
}