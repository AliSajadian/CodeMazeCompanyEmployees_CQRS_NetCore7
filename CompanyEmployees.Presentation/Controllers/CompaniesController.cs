using Microsoft.AspNetCore.Mvc; 
using MediatR;
using Marvin.Cache.Headers;

using Shared.DTO;
using Application.Companies.Queries;
using Application.Companies.Commands;
using Application.Notifications;
using CompanyEmployees.Presentation.ModelBinders;
using CompanyEmployees.Presentation.Filters.ActionFilters;

namespace CompanyEmployees.Presentation.Controllers;


[Route("api/companies")] 
[ApiController] 
public class CompaniesController : ControllerBase 
{ 
    private readonly ISender _sender; 
    private readonly IPublisher _publisher;

    public CompaniesController(ISender sender, IPublisher publisher) 
    { 
        _sender = sender; 
        _publisher = publisher; 
    }

    /// <summary> 
    /// Gets the list of all companies 
    /// </summary> 
    /// <returns>The companies list</returns>
    [HttpGet] 
    public async Task<IActionResult> GetCompanies() 
    { 
        var companies = await _sender.Send(new GetCompaniesQuery(TrackChanges: false)); 
        return Ok(companies); 
    }

    /// <summary> 
    /// Gets the list of a collection of companies 
    /// </summary> 
    /// <param name="ids"></param>
    /// <returns>The companies collection</returns>
    /// <response code="400">If the item is null</response>
    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection (
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids) 
    { 
        var companies = await _sender.Send(new GetCompanyCollectionQuery(ids, TrackChanges: false)); 
        return Ok(companies); 
    }

    /// <summary> 
    /// Gets the specified company 
    /// </summary> 
    /// <param name="id"></param>
    /// <returns>The company</returns>
    [HttpGet("{id:guid}", Name = "CompanyById")]
    // [ResponseCache(CacheProfileName = "120SecondsDuration")]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)] 
    [HttpCacheValidation(MustRevalidate = false)]
    public async Task<IActionResult> GetCompany(Guid id) 
    { 
        var company = await _sender.Send(new GetCompanyQuery(id, TrackChanges: false));
        return Ok(company); 
    }

    /// <summary> 
    /// Creates a newly created company 
    /// </summary> 
    /// <param name="companyForCreationDto"></param>
    /// <returns>A newly created company</returns> 
    /// <response code="201">Returns the newly created item</response> 
    /// <response code="400">If the item is null</response> 
    /// <response code="422">If the model is invalid</response> 
    [HttpPost(Name = "CreateCompany")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)] 
    [ProducesResponseType(422)]
    [ServiceFilter(typeof(ValidationFilterAttribute))] 
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto companyForCreationDto) 
    { 
        if (companyForCreationDto is null) 
            return BadRequest("CompanyForCreationDto object is null"); 
            
        var company = await _sender.Send(new CreateCompanyCommand(companyForCreationDto)); 
        
        return CreatedAtRoute("CompanyById", new { id = company.Id }, company); 
    }

    /// <summary> 
    /// Creates a collection of created companies 
    /// </summary> 
    /// <param name="companyCollection"></param>
    /// <returns>The collection of created companies</returns> 
    /// <response code="201">Returns collection of created item</response> 
    /// <response code="400">If the item is null</response> 
    /// <response code="422">If the model is invalid</response> 
    [HttpPost("collection")] 
    [ProducesResponseType(201)]
    [ProducesResponseType(400)] 
    [ProducesResponseType(422)]
    public async Task<IActionResult> CreateCompanyCollection (
        [FromBody] IEnumerable<CompanyForCreationDto> companyCollection) 
    { 
        var result = await _sender.Send(new CreateCompanyCollectionCommand(companyCollection));
        return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies); 
    }

    /// <summary> 
    /// Updates a company 
    /// </summary> 
    /// <param name="id"></param>
    /// <param name="companyForUpdateDto"></param>
    /// <returns>An updated company</returns> 
    /// <response code="200">Returns the updated item</response> 
    /// <response code="400">If the item is null</response> 
    /// <response code="422">If the model is invalid</response> 
    [HttpPut("{id:guid}")] 
    [ProducesResponseType(200)]
    [ProducesResponseType(400)] 
    [ProducesResponseType(422)]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid id, CompanyForUpdateDto companyForUpdateDto)
    { 
        if (companyForUpdateDto is null) 
            return BadRequest("CompanyForUpdateDto object is null"); 
        
        await _sender.Send(new UpdateCompanyCommand(id, companyForUpdateDto, TrackChanges: true)); 
        
        return NoContent(); 
    }

    /// <summary> 
    /// Delete the specified company 
    /// </summary> 
    /// <param name="id"></param>
    /// <returns>Nothing</returns> 
    /// <response code="200">Nothing</response> 
    /// <response code="400">If the id is null</response> 
    [HttpDelete("{id:guid}")] 
    [ProducesResponseType(200)]
    [ProducesResponseType(400)] 
    public async Task<IActionResult> DeleteCompany(Guid id) 
    { 
        //await _sender.Send(new DeleteCompanyCommand(id, TrackChanges: false)); 
        await _publisher.Publish(new CompanyDeletedNotification(id, TrackChanges: false));

        return NoContent(); 
    }
}

