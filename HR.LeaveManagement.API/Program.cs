using HR.LeaveManagement.API;
using HR.LeaveManagement.API.Middlewares;
using HR.LeaveManagement.Application;
using HR.LeaveManagement.Identity;
using HR.LeaveManagement.Infrastructure;
using HR.LeaveManagement.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));

    options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));

    options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));

    options.OutputFormatters.RemoveType<StringOutputFormatter>();

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureServices(builder.Configuration);
builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.ConfigureApiServices();
builder.Services.ConfigureIdentity(builder.Configuration);

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<HRLeaveManagementDbContext>();
await dbContext.Database.MigrateAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
