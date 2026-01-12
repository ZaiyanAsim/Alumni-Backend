
using Admin.Application.Abstractions;
using Admin.Application.Handlers;
using Admin.Application.Services;
using Admin.Infrastructure.Data;
using Admin.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Project.Infrastructure;
using Shared.Auth;
using Shared.Infrastrcuture;
using Shared.TenantService;
using Shared.Custom_Exceptions;
using Users.Infrastructure;
using Microsoft.Extensions.Configuration;
using Alumni_Portal.Exceptions;
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
//builder.Services.AddScoped<userHandler>();
//builder.Services.AddScoped<projectHandler>();
//builder.Services.AddScoped<IUserService, userRepository>();
//builder.Services.AddScoped<IProjectService, projectRepository>();
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDbContext<SharedDbContext>(options =>
//                options.UseSqlServer(
//                    configuration.GetConnectionString("DefaultConnection"),
//                    b => b.MigrationsAssembly("Shared.Infrastructure")
//                )
//            );
builder.Services.AddProblemDetails();
builder.Services.AddUsersInfrastructure(builder.Configuration);
builder.Services.AddProjectsInfrastructure(builder.Configuration);
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseExceptionHandler();
//app.UseAuthentication();
//app.UseMiddleware<TenantMiddleware>();
//app.UseAuthorization();


app.MapControllers();

app.Run();
