using MediatR; 

using Entities.Models;
using Shared.DTO;

namespace Application.Employees.Commands;


public sealed record PartiallyUpdateEmployeeForCompanyCommand( 
        Guid companyId, Guid id, bool companyTrackChanges, bool employeeTrackChanges) : 
        IRequest<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)>;
