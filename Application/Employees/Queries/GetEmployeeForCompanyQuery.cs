using MediatR; 

using Shared.DTO;

namespace Application.Employees.Queries;

public sealed record GetEmployeeForCompanyQuery(Guid companyId, Guid id, bool trackChanges) : IRequest<EmployeeDto>;