using MediatR;
using AutoMapper;

using Entities.Models;
using Contracts;
using Shared.DTO;
using Application.Employees.Commands;

namespace Application.Employees.Handlers;


internal sealed class DeleteEmployeeForCompanyHandler : IRequestHandler<DeleteEmployeeForCompanyCommand, Unit> 
{ 
    private readonly IRepositoryManager _repository; 
    private readonly IMapper _mapper; 
    private readonly IEmployeeLinks _employeeLinks; 
    private readonly EmployeeHandlerHelper _helper;
    public DeleteEmployeeForCompanyHandler(IRepositoryManager repository, IMapper mapper, IEmployeeLinks employeeLinks) 
    { 
        _repository = repository; 
        _mapper = mapper;
        _employeeLinks = employeeLinks;
        _helper = new EmployeeHandlerHelper(repository);
    } 
    
    public async Task<Unit> Handle(DeleteEmployeeForCompanyCommand request, CancellationToken cancellationToken) 
    { 
        await _helper.CheckIfCompanyExists(request.companyId, request.trackChanges); 
                
        var employeeForCompany = await _helper.GetEmployeeAndCheckIfItExists(request.companyId, request.id, request.trackChanges); 
                
        _repository.Employee.DeleteEmployee(employeeForCompany); 
        _repository.Save(); 

        return Unit.Value;
    }
}