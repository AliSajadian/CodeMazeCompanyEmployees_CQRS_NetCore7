using AutoMapper;
using Microsoft.AspNetCore.Identity; 
using System.IdentityModel.Tokens.Jwt; 
using Microsoft.IdentityModel.Tokens; 
using System.Text; 
using System.Security.Claims;
using Entities.ConfigurationModels; 
using MediatR;

using Entities.Models; 
using Entities.Exceptions;
using Application.Authentication.Commands;

namespace Application.Authentication.Handlers;


internal sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, IdentityResult> 
{ 
    private readonly IMapper _mapper; 
    private readonly UserManager<User> _userManager; 
    private readonly JwtConfiguration _jwtConfiguration;

    public RegisterUserHandler(IMapper mapper,
                               UserManager<User> userManager, 
                               JwtConfiguration jwtConfiguration) 
    {
        _mapper = mapper;
        _userManager = userManager;
        _jwtConfiguration = jwtConfiguration;
    } 
    
    public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken) 
    { 
        var user = _mapper.Map<User>(request.user); 
        var result = await _userManager.CreateAsync(user, request.user.Password!); 
        
        if (result.Succeeded) 
            await _userManager.AddToRolesAsync(user, request.user.Roles!);
            
        return result; 
    }
}