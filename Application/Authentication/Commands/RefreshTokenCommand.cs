using MediatR; 

using Entities.Models;
using Shared.DTO;

namespace Application.Authentication.Commands;


public sealed record RefreshTokenCommand(TokenDto tokenDto) : IRequest<User>;