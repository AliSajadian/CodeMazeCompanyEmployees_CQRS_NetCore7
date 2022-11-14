using Microsoft.AspNetCore.Http;

using Entities.LinkModels;
using Shared.DTO;

public interface IEmployeeLinks 
{ 
    LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto, 
            string fields, Guid companyId, HttpContext httpContext); 
}