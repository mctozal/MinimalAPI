using API.Features.Log.Dtos;
using MediatR;

namespace API.Features.Log.Queries.List;

public record ListLogsQuery() : IRequest<List<LogDto>> { }