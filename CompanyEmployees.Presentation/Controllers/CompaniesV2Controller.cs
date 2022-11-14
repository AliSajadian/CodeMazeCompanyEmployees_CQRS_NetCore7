using Microsoft.AspNetCore.Mvc;
using MediatR;

using Application.Companies.Queries;

namespace CompanyEmployees.Presentation.Controllers;


/// <summary> 
/// Gets the list of all companies 
/// </summary> 
/// <returns>The companies list</returns>
// [ApiVersion("2.0")] 
// [Route("api/{v:apiversion}/companies")]
[Route("api/companies")] 
[ApiController] 
[ApiExplorerSettings(GroupName = "v2")]
public class CompaniesV2Controller : ControllerBase 
{ 
    private readonly ISender _sender; 
    private readonly IPublisher _publisher;

    public CompaniesV2Controller(ISender sender, IPublisher publisher) 
    { 
        _sender = sender; 
        _publisher = publisher; 
    }

    /// <summary> 
    /// Gets the list of all companies 
    /// </summary> 
    /// <returns>The companies list</returns>
    [HttpGet(Name = "GetCompanies")]
    public async Task<IActionResult> GetCompanies() 
    { 
        var companies = await _sender.Send(new GetCompaniesQuery(TrackChanges: false)); 
        var companiesV2 = companies.Select(x => $"{x.Name} V2"); 

        return Ok(companiesV2); 

    } 
}