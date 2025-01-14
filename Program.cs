using back_dotnet;
using back_dotnet.Mappings;
using back_dotnet.Models.Domain;
using back_dotnet.Repositories;
using back_dotnet.Repositories.Auth;
using back_dotnet.Repositories.Files;
using back_dotnet.Repositories.Users;
using back_dotnet.Repositories.Leave;
using back_dotnet.Seeds;
using back_dotnet.Services;
using back_dotnet.Services.Auth;
using back_dotnet.Services.Email;
using back_dotnet.Services.Files;
using back_dotnet.Services.Leave;
using back_dotnet.Services.Password;
using back_dotnet.Services.Permission;
using back_dotnet.Services.Token;
using back_dotnet.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using back_dotnet.Repositories.LeaveAuth;
using back_dotnet.Services.LeaveAuth;
using Quartz;
using back_dotnet.Services.Scheduler;
using Quartz.Impl;
using Quartz.Spi;
using System.Collections.Specialized;
using back_dotnet.Services.Chat;
using back_dotnet.Repositories.Chat;
using back_dotnet.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string
builder
  .Services
  .AddDbContext<HairunSiContext>(
    options => options
    .UseNpgsql(builder.Configuration.GetConnectionString("Hairun_SI"))
  );

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<HairunSiContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<ILeaveAuthRepository, LeaveAuthRepository>();
builder.Services.AddScoped<ILeaveAuthService, LeaveAuthService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddScoped<LeaveScheduler>();
builder.Services.AddScoped<IJobFactory, LeaveJobFactory>();
builder.Services.AddScoped<LeaveJob>();

builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();

builder.Services.AddSingleton<ChatDB>();


builder.Services.AddScoped<IScheduler>(provider =>
{
    NameValueCollection props = new NameValueCollection
    {
        { "quartz.serializer.type", "json" },
        { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
        { "quartz.jobStore.dataSource", "default" },
        { "quartz.dataSource.default.provider", "Npgsql" }, // Utiliser Npgsql pour PostgreSQL
        { "quartz.dataSource.default.connectionString", builder.Configuration.GetConnectionString("Hairun_SI") },
    };
    IScheduler scheduler = new StdSchedulerFactory(props).GetScheduler().Result;
    scheduler.JobFactory = provider.GetRequiredService<IJobFactory>();
    scheduler.Start().Wait();
    return scheduler;
});



builder.Services.AddQuartz(q =>
{
    q.UseInMemoryStore();
    q.UseMicrosoftDependencyInjectionJobFactory();
    
    q.UsePersistentStore(s =>
    {
        s.UseProperties = true;
        s.UseClustering();
        s.UsePostgres(options =>
        {
            options.ConnectionString = builder.Configuration.GetConnectionString("Hairun_SI");
            options.TablePrefix = "qrtz_";
        });
        s.UseNewtonsoftJsonSerializer();
    });
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigin", policy =>
    {
        policy
         .WithOrigins(builder.Configuration.GetSection("AllowedHosts").Get<string[]>())
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


var logger = new LoggerConfiguration()
    .WriteTo.File("Logs/info-.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information)
    .WriteTo.File("logs/error-.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();


// Seeding data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<HairunSiContext>();
    ILeaveRepository leaveRepository = services.GetRequiredService<ILeaveRepository>();

    context.Database.Migrate();
    await SuperAdminSeeder.SeedAsync(context);
    await EmployeSeeder.SeedEmployeAsync(context);
    await LeaveSeeder.SeedAsync(context);
    await leaveRepository.RefreshNonScheduledLeave();
    await ChatSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowedOrigin");
app.MapControllers();

app.MapHub<ChatHub>( "/socket" );
app.Run();
