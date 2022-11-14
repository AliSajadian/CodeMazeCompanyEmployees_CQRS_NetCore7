using MediatR; 

using Shared.DTO;

namespace Application.Companies.Queries;

public sealed record GetCompanyCollectionQuery(IEnumerable<Guid> ids, 
        bool TrackChanges) : IRequest<IEnumerable<CompanyDto>>;