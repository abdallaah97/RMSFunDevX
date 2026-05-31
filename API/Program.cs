using Application.Repositories;
using Application.Servces.AuthService;
using Application.Servces.CurrentUserService;
using Application.Servces.RequestService;
using Application.Servces.TechnicianUserService;
using Application.Servces.UserService;
using Infrastructre.Context;
using Infrastructre.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using API.Hubs;
using API.Providers;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
    );

//JWT JSON WEB TOKEN

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters.ValidateIssuer = true;
    options.TokenValidationParameters.ValidateAudience = true;
    options.TokenValidationParameters.ValidateLifetime = true;
    options.TokenValidationParameters.ValidateIssuerSigningKey = true;
    options.TokenValidationParameters.ValidIssuer = builder.Configuration["jwt:issuer"];
    options.TokenValidationParameters.ValidAudience = builder.Configuration["jwt:audience"];
    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"]));

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MRS API",
        Version = "v1"
    });


    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Put **_ONLY_** your JWT Bearer token hera",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityReq = new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { } }
    };
    c.AddSecurityRequirement(securityReq);
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IUserService), typeof(UserService));
builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
builder.Services.AddScoped(typeof(ICurrentUserService), typeof(CurrentUserService));
builder.Services.AddScoped(typeof(IRequestService), typeof(RequestService));
builder.Services.AddScoped(typeof(ITechnicianUserService), typeof(TechnicianUserService));
builder.Services.AddScoped(typeof(Application.Servces.ChatService.IChatService), typeof(Application.Servces.ChatService.ChatService));
builder.Services.AddScoped(typeof(Application.Servces.FileService.IFileService), typeof(Application.Servces.FileService.FileService));

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .SetIsOriginAllowed((host) => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    UserSeedData.UserSeed(services);
}
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
