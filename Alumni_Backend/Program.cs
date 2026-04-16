using Alumni_Portal.Entity_Directories;
using Alumni_Portal.Exceptions;
using Alumni_Portal.FileUploads;
using Alumni_Portal.OpenPortalPages.Feed;
using Alumni_Portal.OpenPortalPages.ProjectFeed;
using Alumni_Portal.Profiles;
using Alumni_Portal.TenantConfiguration;
using Project.Infrastructure;
using Quartz;
using Shared.Auth;
using Shared.Custom_Exceptions;
using Shared.TenantService;
using StackExchange.Redis;
using System.Text.Json;
using Users.Infrastructure;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddScoped<ITenantService, TenantService>();
//builder.Services.AddScoped<FileService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
        
});
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<AttachmentService>();



builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));



//builder.Services.AddQuartz(q =>
//{
   

    
//    q.UsePersistentStore(s =>
//    {
//        s.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//        s.UseJsonSerializer();
//    });
//});

//// Start the Quartz hosted service
//builder.Services.AddQuartzHostedService(opt =>
//{
//    opt.WaitForJobsToComplete = true;
//    opt.AwaitApplicationStarted = true; // important: wait for DI to be ready
//});


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Keeps original casing
    });

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
builder.Services.AddEntityDirectoriesInfrastructure(builder.Configuration);
builder.Services.AddFeedInfrastructure(builder.Configuration);
builder.Services.AddProjectFeedInfrastructure(builder.Configuration);
builder.Services.AddProfileInfrastructure(builder.Configuration);
builder.Services.AddTenantConfigurationInfrastructure(builder.Configuration);


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("FrontendPolicy");

app.UseHttpsRedirection();
app.UseExceptionHandler();
//app.UseAuthentication();
//app.UseMiddleware<TenantMiddleware>();
//app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();
