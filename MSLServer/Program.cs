using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using MSLServer.Data;
using MSLServer.Logic;
using MSLServer.Middlewares;
using MSLServer.Models;
using MSLServer.Models.Policy;
using MSLServer.Services.EmailService;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IServerRepository, ServerRepository>();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
builder.Services.AddTransient<ServerListDBContext, ServerListDBContext>();
builder.Services.AddTransient<IFileManager, FileManager>();
/* Services */
builder.Services.AddScoped<IEmailService, EmailService>();

// Adding background worker services
builder.Services.AddSingleton(new PeriodicTimer(TimeSpan.FromSeconds(60)));
builder.Services.AddHostedService<ServerStatusCheckerBackgroundWorker>();


builder.Services.AddSignalR();
builder.Services.AddCors(options => options.AddDefaultPolicy(
    builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    }
));


builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Resource.Criptkey)),
        ValidateIssuer = false,
        ValidateAudience = false

    };

});

builder.Services.AddAuthorization(configure =>
{
    configure.AddPolicy("DeletePolicy", policyBuilder =>
    {
        policyBuilder.AddRequirements(new IdRequirement(""));
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme { 
        Description="Standart authorization header using the bearer scheme (\"bearer {token}\")",
        In=Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name="Authorization",
        Type=Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseStaticFiles(new StaticFileOptions { 
    FileProvider = new PhysicalFileProvider(Path.GetFullPath("E:\\Programing\\MSLProject\\Resources\\Files")),
    RequestPath="/Files"
});


app.MapControllers();

app.Run();
