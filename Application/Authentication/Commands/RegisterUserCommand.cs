using Microsoft.AspNetCore.Identity;
using MediatR; 

using Shared.DTO;

namespace Application.Authentication.Commands;


public sealed record RegisterUserCommand(UserForRegistrationDto user) : IRequest<IdentityResult>;