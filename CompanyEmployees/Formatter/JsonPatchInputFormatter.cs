using Microsoft.AspNetCore.Mvc.Formatters; 
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.JsonPatchRequestHandler;

public static class JsonPatchInputFormatter
{
    public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() => 
    new ServiceCollection()
            .AddLogging()
            .AddMvc()
            .AddNewtonsoftJson() 
            .Services.BuildServiceProvider() 
            .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters 
            .OfType<NewtonsoftJsonPatchInputFormatter>()
            .First();
}