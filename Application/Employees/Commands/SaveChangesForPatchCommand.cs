using MediatR; 

using Entities.Models;
using Shared.DTO;

namespace Application.Employees.Commands;


public sealed record SaveChangesForPatchCommand(
    EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) : IRequest;