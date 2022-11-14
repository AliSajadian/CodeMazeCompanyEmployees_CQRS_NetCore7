using Microsoft.AspNetCore.Identity; 
using System.IdentityModel.Tokens.Jwt; 
using Microsoft.IdentityModel.Tokens; 
using System.Text; 
using System.Security.Claims;
using System.Security.Cryptography;
using Entities.ConfigurationModels; 
using MediatR;

using Entities.Models; 
using Contracts; 
using Shared.DTO;
using Application.Authentication.Commands;

namespace Application.Authentication.Handlers;


internal sealed class ValidateUserHandler : IRequestHandler<ValidateUserCommand, User?> 
{ 
    private readonly ILoggerManager _logger; 
    private readonly UserManager<User> _userManager; 
    private readonly JwtConfiguration _jwtConfiguration;

    public ValidateUserHandler(ILoggerManager logger,
                               UserManager<User> userManager, 
                               JwtConfiguration jwtConfiguration) 
    {
        _logger = logger;
        _userManager = userManager;
        _jwtConfiguration = jwtConfiguration;
    } 
    
    public async Task<User?> Handle(ValidateUserCommand request, CancellationToken cancellationToken) 
    { 
        var _user = await _userManager.FindByNameAsync(request.user.UserName!); 
         
        
        if (!(_user != null && await _userManager.CheckPasswordAsync(_user, request.user.Password!)))
        {
            _logger.LogWarn($"Authentication failed. Wrong user name or password."); 
            _user = null;
            return _user;
        }

        return _user; 
    }   
}