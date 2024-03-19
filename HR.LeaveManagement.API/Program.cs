using HR.LeaveManagement.API;
using HR.LeaveManagement.Application;
using HR.LeaveManagement.Infrastructure;
using HR.LeaveManagement.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers(options =>
{
    options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));

    options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));

    options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));

    options.OutputFormatters.RemoveType<StringOutputFormatter>();

})
    .ConfigureApiBehaviorOptions(setupAction =>
    {
        setupAction.InvalidModelStateResponseFactory = context =>
        {
            var problemDetailsFactory = context.HttpContext.RequestServices
            .GetRequiredService<ProblemDetailsFactory>();

            var validationProblemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext,
                context.ModelState);

            validationProblemDetails.Detail = "See the errors field for details";
            validationProblemDetails.Instance = context.HttpContext.Request.Path;

            var actionExecutingContext = context as ActionExecutingContext;
            if ((context.ModelState.ErrorCount > 0)
                && (actionExecutingContext?.ActionArguments.Count ==
                context.ActionDescriptor.Parameters.Count))
            {
                validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                validationProblemDetails.Title = "One or more validation errors occurred";

                return new UnprocessableEntityObjectResult(validationProblemDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };
            }

            validationProblemDetails.Status = StatusCodes.Status400BadRequest;
            validationProblemDetails.Title = "One or more error on input occurred";
            return new BadRequestObjectResult(validationProblemDetails)
            {
                ContentTypes = { "application/problem+json" }
            };
        };
    });

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.ConfigureApplicationServices();
services.ConfigureInfrastructureServices(configuration);
services.ConfigurePersistenceServices(configuration);
services.ConfigureApiServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
