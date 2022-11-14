using MediatR; 

using Shared.DTO;

namespace Application.Employees.Commands;


public sealed record CreateEmployeeForCompanyCommand(Guid companyId, 
    EmployeeForCreationDto employee, bool trackChanges) : IRequest<EmployeeDto>;