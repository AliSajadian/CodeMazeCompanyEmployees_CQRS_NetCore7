using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MediatR;

using Shared.DTO;
using CompanyEmployees.Presentation.Filters.ActionFilters;
using Application.Authentication.Commands;

namespace CompanyEmployees.Presentation.Controllers;


[Route("api/authentication")] 
[ApiController] 
public class AuthenticationController : ControllerBase 
{
    private readonly ISender _sender; 
    private readonly IPublisher _publisher;

    public AuthenticationController(ISender sender, IPublisher publisher) 
    { 
        _sender = sender; 
        _publisher = publisher; 
    }
    
    [HttpPost] 
    [ServiceFilter(typeof(ValidationFilterAttribute))] 
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration) 
    { 
        var result = await _sender.Send(new RegisterUserCommand(userForRegistration)); 
        
        if (!result.Succeeded)
        { 
            foreach (var error in result.Errors) 
            { 
                ModelState.TryAddModelError(error.Code, error.Description); 
            } 
            return BadRequest(ModelState); 
        } 
        return StatusCode(201); 
    }

    [HttpPost("login")] 
    [ServiceFilter(typeof(ValidationFilterAttribute))] 
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user) 
    {
        var _user = await _sender.Send(new ValidateUserCommand(user));
        if (_user == null) 
            return Unauthorized(); 
            
        var tokenDto = await _sender.Send(new CreateTokenCommand(_user, true));
        return Ok(tokenDto);
    }
}