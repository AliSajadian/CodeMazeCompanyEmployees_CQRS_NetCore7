using MediatR;
using AutoMapper;

using Entities.Exceptions;
using Contracts;
using Shared.DTO;
using Application.Companies.Queries;

namespace Application.Companies.Handlers;


internal sealed class GetCompanyHandler : IRequestHandler<GetCompanyQuery, CompanyDto> 
{ 
    private readonly IRepositoryManager _repository; 
    private readonly IMapper _mapper; 

    public GetCompanyHandler(IRepositoryManager repository, IMapper mapper) 
    { 
        _repository = repository; _mapper = mapper; 
    } 
    
    public async Task<CompanyDto> Handle(GetCompanyQuery request, CancellationToken cancellationToken) 
    { 
        var company = await _repository.Company.GetCompanyAsync(request.Id, request.TrackChanges); 
        
        if (company is null) 
            throw new CompanyNotFoundException(request.Id); 
            
        var companyDto = _mapper.Map<CompanyDto>(company); 
        
        return companyDto;
    }
}