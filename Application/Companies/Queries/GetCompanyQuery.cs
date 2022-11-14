using MediatR; 

using Shared.DTO;

namespace Application.Companies.Queries;


public sealed record GetCompanyQuery(Guid Id, bool TrackChanges) : IRequest<CompanyDto>;