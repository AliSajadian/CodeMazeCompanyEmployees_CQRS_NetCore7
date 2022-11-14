using MediatR; 

using Shared.DTO;

namespace Application.Companies.Commands;


public sealed record CreateCompanyCollectionCommand(
    IEnumerable<CompanyForCreationDto> companyCollection) : IRequest<(IEnumerable<CompanyDto> companies, string ids)>;