using System.Reflection;
using API.Behaviors;
using API.Features.Log.Queries.List;
using API.Features.Product.Commands.Create;
using API.Features.Product.Commands.Delete;
using API.Features.Product.Commands.Update;
using API.Features.Product.Queries.Get;
using API.Features.Product.Queries.List;
using API.Persistence;
using MediatR;
using Scalar.AspNetCore;
using Serilog;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        });
        
        builder.Services.AddDbContext<AppDbContext>();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        
        builder.Services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
        });
        
        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
   
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.UseSerilogRequestLogging();
        
        app.MapGet("/products/{id:guid}", async (Guid id, ISender mediatr) =>
        {
            var product = await mediatr.Send(new GetProductQuery(id));
            if (product == null) return Results.NotFound();
            return Results.Ok(product);
        });

        app.MapGet("/products", async (ISender mediatr) =>
        {
            var products = await mediatr.Send(new ListProductsQuery());
            return Results.Ok(products);
        });

        app.MapPost("/products", async (CreateProductCommand command, ISender mediatr) =>
        {
            var product = await mediatr.Send(command);
            if (product == null) return Results.BadRequest();
            
            return Results.Created($"/products/{product.Id}", new { id = product.Id });
        });

        app.MapPut("/products", async (UpdateProductCommand command, ISender mediatr) =>
        {
            var product = await mediatr.Send(command);
            if (product == null) return Results.BadRequest();
            return Results.NoContent();

        });
        
        app.MapDelete("/products/{id:guid}", async (Guid id, ISender mediatr) =>
        {
            await mediatr.Send(new DeleteProductCommand(id));
            if(id==Guid.Empty) return Results.BadRequest();
            return Results.NoContent();
        });

        app.MapGet("/logs", async (ISender mediatr) =>
        {
           var logs= await mediatr.Send(new ListLogsQuery());
            return Results.Ok(logs);
        });

        app.Run();
    }
}