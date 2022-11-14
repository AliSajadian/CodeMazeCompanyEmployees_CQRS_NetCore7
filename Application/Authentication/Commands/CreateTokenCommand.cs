using MediatR; 

using Entities.Models;
using Shared.DTO;

namespace Application.Authentication.Commands;


public sealed record CreateTokenCommand(User user, bool populateExp) : IRequest<TokenDto>;