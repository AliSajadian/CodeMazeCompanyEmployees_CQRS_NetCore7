using MediatR;
using AutoMapper;

using Entities.Models;
using Contracts;
using Shared.DTO;
using Application.Employees.Commands;

namespace Application.Employees.Handlers;


internal sealed class CreateEmployeeForCompanyHandler : IRequestHandler<CreateEmployeeForCompanyCommand, EmployeeDto> 
{ 
    private readonly IRepositoryManager _repository; 
    private readonly IMapper _mapper; 
    private readonly IEmployeeLinks _employeeLinks; 
    private readonly EmployeeHandlerHelper _helper;
    public CreateEmployeeForCompanyHandler(IRepositoryManager repository, IMapper mapper, IEmployeeLinks employeeLinks) 
    { 
        _repository = repository; 
        _mapper = mapper;
        _employeeLinks = employeeLinks;
        _helper = new EmployeeHandlerHelper(repository);
    } 
    
    public async Task<EmployeeDto> Handle(CreateEmployeeForCompanyCommand request, CancellationToken cancellationToken) 
    { 
        await _helper.CheckIfCompanyExists(request.companyId, request.trackChanges); 
            
        var employeeEntity = _mapper.Map<Employee>(request.employee); 
        _repository.Employee.CreateEmployeeForCompany(request.companyId, employeeEntity); 
        await _repository.SaveAsync(); 
        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity); 
        
        return employeeToReturn;
    }
}