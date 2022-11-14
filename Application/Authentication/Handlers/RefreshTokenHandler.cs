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


internal sealed class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, User> 
{ 
    private readonly UserManager<User> _userManager; 
    private readonly JwtConfiguration _jwtConfiguration;

    public RefreshTokenHandler(UserManager<User> userManager, 
                               JwtConfiguration jwtConfiguration) 
    { 
        _userManager = userManager;
        _jwtConfiguration = jwtConfiguration;
    } 
    
    public async Task<User> Handle(RefreshTokenCommand request, CancellationToken cancellationToken) 
    { 
        var principal = GetPrincipalFromExpiredToken(request.tokenDto.AccessToken); 
        var user = await _userManager.FindByNameAsync(principal.Identity!.Name!); 
        
        if (user == null || user.RefreshToken != request.tokenDto.RefreshToken || 
            user.RefreshTokenExpiryTime <= DateTime.Now) 
            throw new RefreshTokenBadRequest(); 
        
        return user; 
    }
    
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token) 
    { 
        var tokenValidationParameters = new TokenValidationParameters 
        { 
            ValidateAudience = true, 
            ValidateIssuer = true, 
            ValidateIssuerSigningKey = true, 
            IssuerSigningKey = new SymmetricSecurityKey( 
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!)), 
            ValidateLifetime = true, 
            ValidIssuer = _jwtConfiguration.ValidIssuer, 
            ValidAudience = _jwtConfiguration.ValidAudience
        }; 

        var tokenHandler = new JwtSecurityTokenHandler(); 
        SecurityToken securityToken; 
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken); 
        var jwtSecurityToken = securityToken as JwtSecurityToken; 
        
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(
            SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        { 
            throw new SecurityTokenException("Invalid token"); 
        } 
        return principal; 
    }

}