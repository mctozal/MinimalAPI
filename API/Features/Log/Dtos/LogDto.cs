namespace API.Features.Log.Dtos;

public record LogDto(Guid Id, string? RequestBody,string? ResponseBody, DateTimeOffset RequestDate, DateTimeOffset ResponseDate);