using MediatR; 

using Entities.Models;
using Shared.DTO;

namespace Application.Authentication.Commands;


public sealed record ValidateUserCommand(UserForAuthenticationDto user) : IRequest<User>;