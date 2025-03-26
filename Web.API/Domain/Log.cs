namespace API.Domain;

public class Log{
    
    public Guid Id { get; set; }
    public  string? RequestBody { get; set; }
    public  string? ResponseBody { get; set; }
    public  DateTimeOffset RequestDate { get; set; }
    public DateTimeOffset ResponseDate { get; set; }


    public Log(string? requestBody, string? responseBody, DateTimeOffset requestDate,
        DateTimeOffset responseDate)
    {
        Id = Guid.NewGuid();
        RequestBody = requestBody;
        ResponseBody = responseBody;
        RequestDate = requestDate;
        ResponseDate = responseDate;
    }
    
    private Log() { }
}