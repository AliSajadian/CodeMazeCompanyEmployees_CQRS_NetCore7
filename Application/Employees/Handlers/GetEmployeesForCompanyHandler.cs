using MediatR;
using AutoMapper;

using Entities.LinkModels;
using Shared.RequestFeatures;
using Entities.Exceptions;
using Contracts;
using Shared.DTO;
using Application.Employees.Queries;

namespace Application.Employees.Handlers;


internal sealed class GetEmployeesForCompanyHandler : IRequestHandler<GetEmployeesForCompanyQuery, (LinkResponse linkResponse, MetaData metaData)> 
{ 
    private readonly IRepositoryManager _repository; 
    private readonly IMapper _mapper; 
    private readonly IEmployeeLinks _employeeLinks; 
    private readonly EmployeeHandlerHelper _helper;
    public GetEmployeesForCompanyHandler(IRepositoryManager repository, IMapper mapper, IEmployeeLinks employeeLinks) 
    { 
        _repository = repository; 
        _mapper = mapper;
        _employeeLinks = employeeLinks;
        _helper = new EmployeeHandlerHelper(repository);
    } 
    
    public async Task<(LinkResponse linkResponse, MetaData metaData)> Handle(GetEmployeesForCompanyQuery request, CancellationToken cancellationToken) 
    { 
        if (!request.linkParameters.EmployeeParameters.ValidAgeRange) 
            throw new MaxAgeRangeBadRequestException(); 
        
        await _helper.CheckIfCompanyExists(request.companyId, request.trackChanges); 

        var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync( 
            request.companyId, request.linkParameters.EmployeeParameters, request.trackChanges); 

        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
        
        var links = _employeeLinks.TryGenerateLinks(employeesDto, 
            request.linkParameters.EmployeeParameters.Fields!, request.companyId, request.linkParameters.Context); 
        
        return (linkResponse: links, metaData: employeesWithMetaData.MetaData); 

    }
}