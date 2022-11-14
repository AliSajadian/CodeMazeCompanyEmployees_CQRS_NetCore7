using MediatR; 

using Entities.LinkModels;
using Shared.RequestFeatures;

namespace Application.Employees.Queries;

public sealed record GetEmployeesForCompanyQuery(Guid companyId, 
        LinkParameters linkParameters, bool trackChanges) : IRequest<(LinkResponse linkResponse, MetaData metaData)>;