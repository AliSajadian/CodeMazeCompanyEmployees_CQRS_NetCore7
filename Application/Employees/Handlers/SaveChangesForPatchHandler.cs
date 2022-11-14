using MediatR;
using AutoMapper;

using Entities.Models;
using Contracts;
using Shared.DTO;
using Application.Employees.Commands;

namespace Application.Employees.Handlers;


internal sealed class SaveChangesForPatchHandler : IRequestHandler<SaveChangesForPatchCommand, Unit> 
{ 
    private readonly IRepositoryManager _repository; 
    private readonly IMapper _mapper; 
    private readonly IEmployeeLinks _employeeLinks; 
    private readonly EmployeeHandlerHelper _helper;
    public SaveChangesForPatchHandler(IRepositoryManager repository, IMapper mapper, IEmployeeLinks employeeLinks) 
    { 
        _repository = repository; 
        _mapper = mapper;
        _employeeLinks = employeeLinks;
        _helper = new EmployeeHandlerHelper(repository);
    } 
    
    public async Task<Unit> Handle(SaveChangesForPatchCommand request, CancellationToken cancellationToken) 
    { 
        _mapper.Map(request.employeeToPatch, request.employeeEntity); 
        await _repository.SaveAsync(); 

        return Unit.Value;
    }
}