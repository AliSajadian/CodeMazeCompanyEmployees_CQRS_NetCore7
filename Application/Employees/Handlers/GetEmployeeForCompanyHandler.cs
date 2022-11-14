using MediatR;
using AutoMapper;

using Entities.LinkModels;
using Shared.RequestFeatures;
using Entities.Exceptions;
using Contracts;
using Shared.DTO;
using Application.Employees.Queries;

namespace Application.Employees.Handlers;


internal sealed class GetEmployeeForCompanyHandler : IRequestHandler<GetEmployeeForCompanyQuery, EmployeeDto> 
{ 
    private readonly IRepositoryManager _repository; 
    private readonly IMapper _mapper; 
    private readonly IEmployeeLinks _employeeLinks; 
    private readonly EmployeeHandlerHelper _helper;
    public GetEmployeeForCompanyHandler(IRepositoryManager repository, IMapper mapper, IEmployeeLinks employeeLinks) 
    { 
        _repository = repository; 
        _mapper = mapper;
        _employeeLinks = employeeLinks;
        _helper = new EmployeeHandlerHelper(repository);
    } 
    
    public async Task<EmployeeDto> Handle(GetEmployeeForCompanyQuery request, CancellationToken cancellationToken) 
    { 
        await _helper.CheckIfCompanyExists(request.companyId, request.trackChanges); 
        
        var employeeDb = await _helper.GetEmployeeAndCheckIfItExists(
                request.companyId, request.id, request.trackChanges); 

        var employee = _mapper.Map<EmployeeDto>(employeeDb); 
        return employee;
    }
}