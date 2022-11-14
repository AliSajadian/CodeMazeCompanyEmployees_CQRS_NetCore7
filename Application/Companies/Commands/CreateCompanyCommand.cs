using MediatR; 

using Shared.DTO;

namespace Application.Companies.Commands;


public sealed record CreateCompanyCommand(CompanyForCreationDto Company) : IRequest<CompanyDto>;