using MediatR;
using AutoMapper;

using Entities.Models;
using Contracts;
using Shared.DTO;
using Application.Employees.Commands;

namespace Application.Employees.Handlers;


internal sealed class UpdateEmployeeForCompanyHandler : IRequestHandler<UpdateEmployeeForCompanyCommand, Unit> 
{ 
    private readonly IRepositoryManager _repository; 
    private readonly IMapper _mapper; 
    private readonly IEmployeeLinks _employeeLinks; 
    private readonly EmployeeHandlerHelper _helper;
    public UpdateEmployeeForCompanyHandler(IRepositoryManager repository, IMapper mapper, IEmployeeLinks employeeLinks) 
    { 
        _repository = repository; 
        _mapper = mapper;
        _employeeLinks = employeeLinks;
        _helper = new EmployeeHandlerHelper(repository);
    } 
    
    public async Task<Unit> Handle(UpdateEmployeeForCompanyCommand request, CancellationToken cancellationToken) 
    { 
       await _helper.CheckIfCompanyExists(request.companyId, request.companyTrackChanges); 
        
        var employee = await _helper.GetEmployeeAndCheckIfItExists(request.companyId, request.id, request.employeeTrackChanges); 
                
        _mapper.Map(request.employee, employee); 
        _repository.Save(); 

        return Unit.Value;
    }
}