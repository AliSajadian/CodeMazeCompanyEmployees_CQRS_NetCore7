using Entities.Models;
using Entities.Exceptions;
using Contracts; 

namespace Application.Employees.Handlers;


internal class EmployeeHandlerHelper
{
    private readonly IRepositoryManager _repository; 
    public EmployeeHandlerHelper(IRepositoryManager repository)
    { 
        _repository = repository; 

    }
    public async Task CheckIfCompanyExists(Guid id, bool trackChanges) 
    { 
        var company = await _repository.Company.GetCompanyAsync(id, trackChanges); 
        if (company is null) 
            throw new CompanyNotFoundException(id); 
    }

    public async Task<Employee> GetEmployeeAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges) 
    { 
        var Employee = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges); 
        if (Employee is null) 
            throw new EmployeeNotFoundException(id); 

        return Employee; 
    }

}
