using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MediatR;

using Shared.DTO;
using CompanyEmployees.Presentation.Filters.ActionFilters;
using Application.Authentication.Commands;

namespace CompanyEmployees.Presentation.Controllers;


[Route("api/token")] 
[ApiController] 
public class TokenController : ControllerBase 
{ 
    private readonly ISender _sender; 
    private readonly IPublisher _publisher;

    public TokenController(ISender sender, IPublisher publisher) 
    { 
        _sender = sender; 
        _publisher = publisher; 
    }

    [HttpPost("refresh")] 
    [ServiceFilter(typeof(ValidationFilterAttribute))]
     public async Task<IActionResult> Refresh([FromBody]TokenDto tokenDto) 
     { 
        var user = await _sender.Send(new RefreshTokenCommand(tokenDto)); 
        
        var tokenDtoToReturn = await _sender.Send(new CreateTokenCommand(user, false)); 

        return Ok(tokenDtoToReturn); 
    }
}