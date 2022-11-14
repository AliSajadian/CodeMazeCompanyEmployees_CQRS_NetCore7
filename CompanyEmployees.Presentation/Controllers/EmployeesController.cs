using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.JsonPatch;
using MediatR;
using Marvin.Cache.Headers;

using Shared.DTO;
using CompanyEmployees.Presentation.Filters.ActionFilters;
using Shared.RequestFeatures;
using System.Text.Json;
using Entities.LinkModels;
using Application.Employees.Queries;
using Application.Employees.Commands;

namespace CompanyEmployees.Presentation.Controllers;


// [ApiVersion("1.0")]
[Route("api/companies/{companyId}/employees")] 
[ApiController] 
public class EmployeesController : ControllerBase 
{ 
    private readonly ISender _sender; 
    private readonly IPublisher _publisher;

    public EmployeesController(ISender sender, IPublisher publisher) 
    { 
        _sender = sender; 
        _publisher = publisher; 
    }

    /// <summary> 
    /// Gets the list of employees for specified company 
    /// </summary> 
    /// <returns>the list of employees for specified company</returns>
    [HttpGet] 
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, 
        [FromQuery] EmployeeParameters employeeParameters) 
    { 
        var linkParams = new LinkParameters(employeeParameters, HttpContext); 
        var result = await _sender.Send(new GetEmployeesForCompanyQuery(companyId, linkParams, trackChanges: false)); 
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));
        
        return result.linkResponse.HasLinks ? 
            Ok(result.linkResponse.LinkedEntities) : 
            Ok(result.linkResponse.ShapedEntities);
    }

    /// <summary> 
    /// Gets an employee for specified company 
    /// </summary> 
    /// <returns>An employee for specified company</returns>
    [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
    public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id) 
    { 
        var employee = await _sender.Send(new GetEmployeeForCompanyQuery(companyId, id, trackChanges: false)); 
        return Ok(employee);
    }

    /// <summary> 
    /// Create an employee for specified company 
    /// </summary> 
    /// <returns>An created employee for specified company</returns>
    [HttpPost] 
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee) 
    { 
        var employeeToReturn = await _sender.Send(
            new CreateEmployeeForCompanyCommand(companyId, employee, trackChanges: false)); 
        
        return CreatedAtRoute("GetEmployeeForCompany", 
                              new { companyId, id = employeeToReturn.Id }, 
                              employeeToReturn); 
    }
    
    /// <summary> 
    /// Delete an employee for specified company 
    /// </summary> 
    /// <returns>Nothing</returns>
    [HttpDelete("{id:guid}")] 
    public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id) 
    { 
        await _sender.Send(new DeleteEmployeeForCompanyCommand(companyId, id, trackChanges: false)); 
        return NoContent();
    }

    /// <summary> 
    /// Updates a employee 
    /// </summary> 
    /// <param name="companyId"></param>
    /// <param name="id"></param>
    /// <param name="employee"></param>
    /// <returns>An updated employee</returns> 
    /// <response code="200">Returns the updated item</response> 
    /// <response code="400">If the item is null</response> 
    /// <response code="422">If the model is invalid</response> 
    [HttpPut("{id:guid}")] 
    [ProducesResponseType(200)]
    [ProducesResponseType(400)] 
    [ProducesResponseType(422)]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee) 
    { 
        await _sender.Send(new UpdateEmployeeForCompanyCommand(companyId, id, employee, 
            companyTrackChanges: false, employeeTrackChanges: true)); 
            
        return NoContent(); 
    }

    /// <summary> 
    /// Updates a employee 
    /// </summary> 
    /// <param name="companyId"></param>
    /// <param name="id"></param>
    /// <param name="patchDoc"></param>
    /// <returns>An updated employee</returns> 
    /// <response code="200">Returns the updated item</response> 
    /// <response code="400">If the item is null</response> 
    /// <response code="422">If the model is invalid</response> 
    [HttpPatch("{id:guid}")] 
    [ProducesResponseType(200)]
    [ProducesResponseType(400)] 
    [ProducesResponseType(422)]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, 
        [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc) 
    { 
        if (patchDoc is null)
            return BadRequest("patchDoc object sent from client is null."); 

        var result = await _sender.Send(new PartiallyUpdateEmployeeForCompanyCommand(
            companyId, id, companyTrackChanges: false, employeeTrackChanges: true)); 
        
        patchDoc.ApplyTo(result.employeeToPatch, ModelState); 
        
        TryValidateModel(result.employeeToPatch);

        if (!ModelState.IsValid) 
            return UnprocessableEntity(ModelState);

        await _sender.Send(new SaveChangesForPatchCommand(result.employeeToPatch, result.employeeEntity)); 
        return NoContent(); 
    }
}