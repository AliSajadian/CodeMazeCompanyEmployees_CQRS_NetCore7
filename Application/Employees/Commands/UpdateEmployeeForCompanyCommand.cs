using MediatR; 

using Shared.DTO;

namespace Application.Employees.Commands;


public sealed record UpdateEmployeeForCompanyCommand(Guid companyId,Guid id, 
    EmployeeForUpdateDto employee, bool companyTrackChanges, bool employeeTrackChanges) : IRequest;