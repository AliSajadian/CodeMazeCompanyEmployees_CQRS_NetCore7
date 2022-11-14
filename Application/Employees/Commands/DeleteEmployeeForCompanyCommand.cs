using MediatR; 

namespace Application.Employees.Commands;


public record DeleteEmployeeForCompanyCommand(Guid companyId, Guid id, bool trackChanges) : IRequest;