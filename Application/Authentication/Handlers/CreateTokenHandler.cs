using Microsoft.AspNetCore.Identity; 
using System.IdentityModel.Tokens.Jwt; 
using Microsoft.IdentityModel.Tokens; 
using System.Text; 
using System.Security.Claims;
using System.Security.Cryptography;
using Entities.ConfigurationModels; 
using MediatR;

using Entities.Models; 
using Shared.DTO;
using Application.Authentication.Commands;

namespace Application.Authentication.Handlers;


internal sealed class CreateTokenHandler : IRequestHandler<CreateTokenCommand, TokenDto> 
{ 
    private readonly UserManager<User> _userManager; 
    private readonly JwtConfiguration _jwtConfiguration;

    public CreateTokenHandler(UserManager<User> userManager, 
                               JwtConfiguration jwtConfiguration) 
    { 
        _userManager = userManager;
        _jwtConfiguration = jwtConfiguration;
    } 
    
    public async Task<TokenDto> Handle(CreateTokenCommand request, CancellationToken cancellationToken) 
    { 
        var signingCredentials = GetSigningCredentials(); 

        var claims = await GetClaims(request.user); 
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims); 
        
        var refreshToken = GenerateRefreshToken(); 
        request.user.RefreshToken = refreshToken; 

        if(request.populateExp) 
            request.user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); 
        
        await _userManager.UpdateAsync(request.user); 
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions); 
        return new TokenDto(accessToken, refreshToken);
    }
    
    private SigningCredentials GetSigningCredentials() 
    { 
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!); 
        var secret = new SymmetricSecurityKey(key); 
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256); 
    } 
    private async Task<List<Claim>> GetClaims(User user) 
    { 
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName!) }; 
        var roles = await _userManager.GetRolesAsync(user); foreach (var role in roles) 
        { 
            claims.Add(new Claim(ClaimTypes.Role, role));
        } 
        return claims; 
    }
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims) 
    { 
        var tokenOptions = new JwtSecurityToken ( 
                issuer: _jwtConfiguration.ValidIssuer, 
                audience: _jwtConfiguration.ValidAudience, 
                claims: claims, 
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
                signingCredentials: signingCredentials 
            ); 
        return tokenOptions; 
    }
    private string GenerateRefreshToken() 
    { 
        var randomNumber = new byte[32]; 
        using (var rng = RandomNumberGenerator.Create()) 
        { 
            rng.GetBytes(randomNumber); 
            return Convert.ToBase64String(randomNumber);
        } 
    } 
}