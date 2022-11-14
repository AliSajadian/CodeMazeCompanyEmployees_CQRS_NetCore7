using NLog;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreRateLimit;
using MediatR;
using FluentValidation;

using Contracts;
using LoggerService;
using Shared.DTO;
using Application.Behaviors;
using Application.DataShaping;
using CompanyEmployees.Exceptions;
using CompanyEmployees.Extensions;
using CompanyEmployees.JsonPatchRequestHandler;
using CompanyEmployees.Presentation.Filters.ActionFilters;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
// Add services to the container.
builder.Services.ConfigureRepositoryManager();
builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers(config => {
    config.RespectBrowserAcceptHeader = true; 
    config.ReturnHttpNotAcceptable = true; 
    config.InputFormatters.Insert(0, JsonPatchInputFormatter.GetJsonPatchInputFormatter());
    config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
  }).AddXmlDataContractSerializerFormatters()
    .AddCustomCSVFormatter()
    .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);
builder.Services.AddCustomMediaTypes();
builder.Services.ConfigureSqlContext(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen();
builder.Services.ConfigureCors();
builder.Services.ConfigureIIS();
builder.Services.AddLoggerService();
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
builder.Services.AddScoped<ValidateMediaTypeAttribute>();
builder.Services.AddScoped<IEmployeeLinks, EmployeeLinks>();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(); 
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.AddMediatR(typeof(Application.AssemblyReference).Assembly);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>(); 
app.ConfigureExceptionHandler(logger); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(s =>
    { 
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "Code Maze API v1"); 
        s.SwaggerEndpoint("/swagger/v2/swagger.json", "Code Maze API v2"); 
    });

    // ===
    // app.UseDeveloperExceptionPage();
}
else 
    app.UseHsts();
// ===

app.UseHttpsRedirection();

// ===
app.UseStaticFiles(); 
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All 
});
app.UseIpRateLimiting();
app.UseCors("CorsPolicy");
// ===
app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseAuthentication();
app.UseAuthorization();

// ===
// app.Use(async (context, next) => 
// {//https://localhost:5001/weatherforecast
//     Console.WriteLine($"Logic before executing the next delegate in the Use method"); 
//     await next.Invoke(); 
//     Console.WriteLine($"Logic after executing the next delegate in the Use method"); 
// }); 
// app.Map("/usingmapbranch", builder => 
// {//https://localhost:5001/usingmapbranch
//     builder.Use(async (context, next) => 
//     { 
//         Console.WriteLine("Map branch logic in the Use method before the next delegate"); 
//         await next.Invoke(); 
//         Console.WriteLine("Map branch logic in the Use method after the next delegate"); 
//     });
//     builder.Run(async context =>
//     { 
//         Console.WriteLine($"Map branch response to the client in the Run method"); 
//         await context.Response.WriteAsync("Hello from the map branch.");
//     });
// });
// app.MapWhen(context => 
//     context.Request.Query.ContainsKey("testquerystring"), builder =>
//     {
//         builder.Run(async context =>
//         {//https://localhost:5001?testquerystring=test:
//             await context.Response.WriteAsync("Hello from the MapWhen branch.");
//         }); 
//     });
// app.Run(async context => 
// { 
//     Console.WriteLine($"Writing the response to the client in the Run method"); 
//     // context.Response.StatusCode = 200; 
//     await context.Response.WriteAsync("Hello from middleware component."); 
// });
// ===


app.MapControllers();

app.Run();