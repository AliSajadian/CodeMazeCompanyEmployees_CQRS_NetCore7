using MediatR;
using AutoMapper;

using Entities.Models;
using Contracts;
using Shared.DTO;
using Application.Employees.Commands;

namespace Application.Employees.Handlers;


internal sealed class PartiallyUpdateEmployeeForCompanyHandler : 
    IRequestHandler<PartiallyUpdateEmployeeForCompanyCommand, 
    (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> 
{ 
    private readonly IRepositoryManager _repository; 
    private readonly IMapper _mapper; 
    private readonly IEmployeeLinks _employeeLinks; 
    private readonly EmployeeHandlerHelper _helper;
    public PartiallyUpdateEmployeeForCompanyHandler(IRepositoryManager repository, IMapper mapper, IEmployeeLinks employeeLinks) 
    { 
        _repository = repository; 
        _mapper = mapper;
        _employeeLinks = employeeLinks;
        _helper = new EmployeeHandlerHelper(repository);
    } 
    
    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> Handle(
        PartiallyUpdateEmployeeForCompanyCommand request, CancellationToken cancellationToken) 
    { 
        await _helper.CheckIfCompanyExists(request.companyId, request.companyTrackChanges); 
            
        var employeeDb = await _helper.GetEmployeeAndCheckIfItExists(
            request.companyId, request.id, request.employeeTrackChanges); 
            
        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeDb);
        
        return (employeeToPatch: employeeToPatch, employeeEntity: employeeDb); 
    }
}