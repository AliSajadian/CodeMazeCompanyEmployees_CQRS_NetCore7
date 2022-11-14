using MediatR; 

using Shared.DTO;

namespace Application.Companies.Commands;


public sealed record UpdateCompanyCommand (Guid Id, CompanyForUpdateDto Company, bool TrackChanges) : IRequest;