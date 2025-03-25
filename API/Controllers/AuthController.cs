using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    // GET
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(String userName, String password)
    {
        var resp =  await _mediator.Send(new AuthUserCommand(userName, password));
        
            if(resp is null) return Unauthorized();
            if(String.IsNullOrEmpty(resp)) return Unauthorized();
            
        return Ok(resp);
    }
}