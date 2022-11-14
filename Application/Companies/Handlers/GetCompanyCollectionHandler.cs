using MediatR;
using AutoMapper;

using Entities.Exceptions;
using Contracts;
using Shared.DTO;
using Application.Companies.Queries;

namespace Application.Companies.Handlers;


sealed class GetCompanyCollectionHandler : IRequestHandler<GetCompanyCollectionQuery, IEnumerable<CompanyDto>> 
{ 
    private readonly IRepositoryManager _repository; 
    private readonly IMapper _mapper; 

    public GetCompanyCollectionHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository; 
        _mapper = mapper; 
    }

    public async Task<IEnumerable<CompanyDto>> Handle(GetCompanyCollectionQuery request, CancellationToken cancellationToken) 
    { 
        if (request.ids is null) 
            throw new IdParametersBadRequestException(); 

        var companies = await _repository.Company.GetByIdsAsync(request.ids, request.TrackChanges); 
        
        if (request.ids.Count() != companies.Count()) 
            throw new CollectionByIdsBadRequestException(); 
        
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies); 
        
        return companiesDto;
    } 
}